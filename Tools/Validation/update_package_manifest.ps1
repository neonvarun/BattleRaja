[CmdletBinding()]
param(
    [string]$ProjectRoot,
    [string]$ManifestPath
)

$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($ProjectRoot)) { $ProjectRoot = Join-Path $PSScriptRoot '..\..' }
$ProjectRoot = [System.IO.Path]::GetFullPath((Resolve-Path -LiteralPath $ProjectRoot).Path)
if ([string]::IsNullOrWhiteSpace($ManifestPath)) { $ManifestPath = Join-Path $ProjectRoot 'PACKAGE_MANIFEST.json' }

$manifest = Get-Content -LiteralPath $ManifestPath -Raw | ConvertFrom-Json
$entries = [System.Collections.Generic.List[object]]::new()
$known = @{}
$excludedDirectories = @('.git', 'lfs', 'Library', 'Temp', 'Obj', 'Build', 'Builds', 'Logs', 'UserSettings', '.utmp')

foreach ($entry in @($manifest.files)) {
    $normalized = ($entry.path -replace '\\', '/')
    $firstPart = ($normalized -split '/')[0]
    if ($firstPart -in $excludedDirectories) { continue }
    $absolute = Join-Path $ProjectRoot ($normalized -replace '/', '\')
    if (-not (Test-Path -LiteralPath $absolute -PathType Leaf)) {
        throw "Manifest entry is missing from the workspace: $normalized"
    }
    $hash = Get-FileHash -Algorithm SHA256 -LiteralPath $absolute
    $updated = [ordered]@{
        path = $normalized
        bytes = (Get-Item -LiteralPath $absolute).Length
        sha256 = $hash.Hash.ToLowerInvariant()
    }
    $entries.Add([pscustomobject]$updated)
    $known[$normalized] = $true
}

$files = Get-ChildItem -LiteralPath $ProjectRoot -Recurse -File | Where-Object {
    $relative = [System.IO.Path]::GetRelativePath($ProjectRoot, $_.FullName)
    $parts = $relative -split '[\\/]'
    ($parts[0] -notin $excludedDirectories) -and
    $_.FullName -ne (Resolve-Path -LiteralPath $ManifestPath).Path
}

foreach ($file in ($files | Sort-Object FullName)) {
    $relative = ([System.IO.Path]::GetRelativePath($ProjectRoot, $file.FullName) -replace '\\', '/')
    if ($known.ContainsKey($relative)) { continue }
    $hash = Get-FileHash -Algorithm SHA256 -LiteralPath $file.FullName
    $entries.Add([pscustomobject][ordered]@{
        path = $relative
        bytes = $file.Length
        sha256 = $hash.Hash.ToLowerInvariant()
    })
    $known[$relative] = $true
}

$manifest.files = @($entries)
$json = $manifest | ConvertTo-Json -Depth 20
Set-Content -LiteralPath $ManifestPath -Value $json -Encoding utf8NoBOM
Write-Host "Updated $ManifestPath with $($entries.Count) file entries."
