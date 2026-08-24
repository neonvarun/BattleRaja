[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ApkPath,
    [Parameter(Mandatory = $true)]
    [string]$AaptPath,
    [string]$ExpectedPackageId,
    [string]$ExpectedVersionName,
    [int]$ExpectedVersionCode = 0,
    [int]$ExpectedMinSdk = 0,
    [int]$ExpectedTargetSdk = 0,
    [switch]$AllowNetworkPermissions
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $ApkPath -PathType Leaf)) {
    throw "APK not found: $ApkPath"
}

if (-not (Test-Path -LiteralPath $AaptPath -PathType Leaf)) {
    throw "aapt.exe not found: $AaptPath"
}

$resolvedApk = (Resolve-Path -LiteralPath $ApkPath).Path
$aaptOutput = @(& $AaptPath dump badging $resolvedApk 2>&1)
if ($LASTEXITCODE -ne 0) {
    throw "aapt dump badging failed ($LASTEXITCODE): $($aaptOutput -join [Environment]::NewLine)"
}

$packageLine = $aaptOutput | Where-Object { $_ -match '^package:' } | Select-Object -First 1
if ([string]::IsNullOrWhiteSpace($packageLine)) {
    throw 'aapt output did not contain a package line.'
}

if ($packageLine -notmatch "name='([^']+)'") {
    throw "Could not parse package ID from: $packageLine"
}
$packageId = $matches[1]

if ($packageLine -notmatch "versionCode='([^']+)'") {
    throw "Could not parse version code from: $packageLine"
}
$versionCode = [int]$matches[1]

if ($packageLine -notmatch "versionName='([^']+)'") {
    throw "Could not parse version name from: $packageLine"
}
$versionName = $matches[1]

$sdkLine = $aaptOutput | Where-Object { $_ -match '^sdkVersion:' } | Select-Object -First 1
$targetSdkLine = $aaptOutput | Where-Object { $_ -match '^targetSdkVersion:' } | Select-Object -First 1
$minSdk = 0
$targetSdk = 0
if ($sdkLine -and $sdkLine -match "sdkVersion:'([^']+)'") {
    $minSdk = [int]$matches[1]
}
if ($targetSdkLine -and $targetSdkLine -match "targetSdkVersion:'([^']+)'") {
    $targetSdk = [int]$matches[1]
}

$permissions = @(
    $aaptOutput |
        Where-Object { $_ -match "^uses-permission: name='([^']+)'" } |
        ForEach-Object {
            if ($_ -match "^uses-permission: name='([^']+)'") { $matches[1] }
        }
)

if (-not [string]::IsNullOrWhiteSpace($ExpectedPackageId) -and $packageId -ne $ExpectedPackageId) {
    throw "Package ID mismatch. Expected '$ExpectedPackageId', found '$packageId'."
}
if (-not [string]::IsNullOrWhiteSpace($ExpectedVersionName) -and $versionName -ne $ExpectedVersionName) {
    throw "Version name mismatch. Expected '$ExpectedVersionName', found '$versionName'."
}
if ($ExpectedVersionCode -gt 0 -and $versionCode -ne $ExpectedVersionCode) {
    throw "Version code mismatch. Expected '$ExpectedVersionCode', found '$versionCode'."
}
if ($ExpectedMinSdk -gt 0 -and $minSdk -ne $ExpectedMinSdk) {
    throw "Minimum SDK mismatch. Expected '$ExpectedMinSdk', found '$minSdk'."
}
if ($ExpectedTargetSdk -gt 0 -and $targetSdk -ne $ExpectedTargetSdk) {
    throw "Target SDK mismatch. Expected '$ExpectedTargetSdk', found '$targetSdk'."
}

if (-not $AllowNetworkPermissions) {
    $forbidden = @(
        'android.permission.INTERNET',
        'android.permission.ACCESS_NETWORK_STATE'
    )
    $unexpected = @($permissions | Where-Object { $_ -in $forbidden })
    if ($unexpected.Count -gt 0) {
        throw "Offline Android candidate contains network permission(s): $($unexpected -join ', ')"
    }
}

$result = [pscustomobject]@{
    Path = $resolvedApk
    Bytes = (Get-Item -LiteralPath $resolvedApk).Length
    Sha256 = (Get-FileHash -LiteralPath $resolvedApk -Algorithm SHA256).Hash
    PackageId = $packageId
    VersionName = $versionName
    VersionCode = $versionCode
    MinSdk = $minSdk
    TargetSdk = $targetSdk
    Permissions = if ($permissions.Count -gt 0) { $permissions -join ', ' } else { '(none)' }
    NetworkPermissions = if ($AllowNetworkPermissions) { 'Allowed by switch' } else { 'Forbidden and absent' }
}
$result | Format-List
