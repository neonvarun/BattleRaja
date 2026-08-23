[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$AabPath,
    [switch]$RequireArm64
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $AabPath -PathType Leaf)) {
    throw "AAB not found: $AabPath"
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
$resolved = (Resolve-Path -LiteralPath $AabPath).Path
$file = Get-Item -LiteralPath $resolved
$hash = (Get-FileHash -LiteralPath $resolved -Algorithm SHA256).Hash
$archive = [System.IO.Compression.ZipFile]::OpenRead($resolved)
try {
    $names = @($archive.Entries | ForEach-Object { $_.FullName })
    $nativeArm64 = @($names | Where-Object { $_ -like 'base/lib/arm64-v8a/*.so' })
    $nativeOther = @($names | Where-Object { $_ -like 'base/lib/*/*.so' -and $_ -notlike 'base/lib/arm64-v8a/*.so' })
    $manifest = $names | Where-Object { $_ -eq 'base/manifest/AndroidManifest.xml' }

    if ($RequireArm64 -and $nativeArm64.Count -eq 0) {
        throw 'No arm64-v8a native libraries were found in the base module.'
    }

    [pscustomobject]@{
        Path = $resolved
        Bytes = $file.Length
        Sha256 = $hash
        HasBaseManifest = ($manifest.Count -gt 0)
        Arm64NativeLibraries = $nativeArm64.Count
        OtherNativeLibraries = $nativeOther.Count
        EntryCount = $names.Count
    } | Format-List
}
finally {
    $archive.Dispose()
}
