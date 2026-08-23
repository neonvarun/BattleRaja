[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$AabPath,
    [switch]$RequireArm64,
    [string]$ReadElfPath,
    [switch]$Require16KPageAlignment
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $AabPath -PathType Leaf)) {
    throw "AAB not found: $AabPath"
}

if ($Require16KPageAlignment) {
    if ([string]::IsNullOrWhiteSpace($ReadElfPath) -or -not (Test-Path -LiteralPath $ReadElfPath -PathType Leaf)) {
        throw 'Require16KPageAlignment needs a valid -ReadElfPath pointing to llvm-readelf.exe.'
    }
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
$resolved = (Resolve-Path -LiteralPath $AabPath).Path
$file = Get-Item -LiteralPath $resolved
$hash = (Get-FileHash -LiteralPath $resolved -Algorithm SHA256).Hash
$archive = [System.IO.Compression.ZipFile]::OpenRead($resolved)
$extractionRoot = $null
try {
    $names = @($archive.Entries | ForEach-Object { $_.FullName })
    $nativeArm64 = @($names | Where-Object { $_ -like 'base/lib/arm64-v8a/*.so' })
    $nativeOther = @($names | Where-Object { $_ -like 'base/lib/*/*.so' -and $_ -notlike 'base/lib/arm64-v8a/*.so' })
    $manifest = $names | Where-Object { $_ -eq 'base/manifest/AndroidManifest.xml' }

    if ($RequireArm64 -and $nativeArm64.Count -eq 0) {
        throw 'No arm64-v8a native libraries were found in the base module.'
    }

    $alignment = @()
    if ($Require16KPageAlignment) {
        $extractionRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("battleraja-aab-" + [guid]::NewGuid().ToString('N'))
        [System.IO.Compression.ZipFile]::ExtractToDirectory($resolved, $extractionRoot)
        $arm64Root = Join-Path $extractionRoot 'base/lib/arm64-v8a'
        $arm64Libraries = @(Get-ChildItem -LiteralPath $arm64Root -Filter '*.so' -File)
        if ($arm64Libraries.Count -eq 0) {
            throw 'No extracted arm64-v8a libraries were found for 16 KB alignment inspection.'
        }

        foreach ($library in $arm64Libraries) {
            $readElfOutput = (& $ReadElfPath -l $library.FullName 2>&1 | Out-String)
            if ($LASTEXITCODE -ne 0) {
                throw "llvm-readelf failed for $($library.Name)."
            }

            $loadAlignments = @()
            foreach ($line in ($readElfOutput -split "`r?`n")) {
                if ($line -match '\bLOAD\b' -and $line -match '(0x[0-9A-Fa-f]+)\s*$') {
                    $loadAlignments += $matches[1]
                }
            }

            if ($loadAlignments.Count -eq 0) {
                throw "No ELF LOAD alignment was found for $($library.Name)."
            }

            $invalid = @($loadAlignments | Where-Object {
                [Convert]::ToInt64($_.Substring(2), 16) -lt 0x4000 -or
                ([Convert]::ToInt64($_.Substring(2), 16) % 0x4000) -ne 0
            })
            $alignment += [pscustomobject]@{
                Library = $library.Name
                LoadAlignments = ($loadAlignments -join ',')
                AlignedTo16K = ($invalid.Count -eq 0)
            }

            if ($invalid.Count -gt 0) {
                throw "ELF LOAD segments for $($library.Name) are not aligned to 16 KB: $($invalid -join ',')."
            }
        }
    }

    $result = [pscustomobject]@{
        Path = $resolved
        Bytes = $file.Length
        Sha256 = $hash
        HasBaseManifest = ($manifest.Count -gt 0)
        Arm64NativeLibraries = $nativeArm64.Count
        OtherNativeLibraries = $nativeOther.Count
        EntryCount = $names.Count
        SixteenKPageAlignment = if ($Require16KPageAlignment) { 'Passed' } else { 'Not checked' }
    }
    $result | Format-List
    if ($alignment.Count -gt 0) {
        Write-Output 'Native ELF alignment:'
        $alignment | Format-Table -AutoSize
    }
}
finally {
    $archive.Dispose()
    if ($extractionRoot -and (Test-Path -LiteralPath $extractionRoot)) {
        Remove-Item -LiteralPath $extractionRoot -Recurse -Force
    }
}
