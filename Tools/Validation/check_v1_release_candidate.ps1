[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$ApkPath,
    [Parameter(Mandatory = $true)]
    [string]$AabPath,
    [Parameter(Mandatory = $true)]
    [string]$AaptPath,
    [Parameter(Mandatory = $true)]
    [string]$ReadElfPath,
    [string]$ProjectRoot,
    [string]$UnityExe,
    [string]$ExpectedPackageId,
    [string]$ExpectedVersionName = '1.0.0',
    [int]$ExpectedVersionCode = 100,
    [int]$ExpectedMinSdk = 28,
    [int]$ExpectedTargetSdk = 36,
    [switch]$RequireCleanWorktree,
    [switch]$AllowNetworkPermissions
)

$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($ProjectRoot)) {
    $ProjectRoot = Join-Path $PSScriptRoot '..\..'
}
$ProjectRoot = (Resolve-Path -LiteralPath $ProjectRoot).Path

function Resolve-RequiredFile {
    param([Parameter(Mandatory = $true)][string]$Path, [Parameter(Mandatory = $true)][string]$Label)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "$Label not found: $Path"
    }
    return (Resolve-Path -LiteralPath $Path).Path
}

$apk = Resolve-RequiredFile -Path $ApkPath -Label 'APK'
$aab = Resolve-RequiredFile -Path $AabPath -Label 'AAB'
$aapt = Resolve-RequiredFile -Path $AaptPath -Label 'aapt.exe'
$readElf = Resolve-RequiredFile -Path $ReadElfPath -Label 'llvm-readelf.exe'

$validationScript = Join-Path $PSScriptRoot 'validate.ps1'
$manifestScript = Join-Path $PSScriptRoot 'check_android_manifest.ps1'
$bundleScript = Join-Path $PSScriptRoot 'check_android_bundle.ps1'
$creativeScript = Join-Path $PSScriptRoot 'check_store_creative.ps1'

Write-Host "BattleRaja V1 release gate: $ProjectRoot"

if ([string]::IsNullOrWhiteSpace($UnityExe)) {
    & $validationScript -ProjectRoot $ProjectRoot -RequireUnityProject
} else {
    & $validationScript -ProjectRoot $ProjectRoot -RequireUnityProject -UnityExe $UnityExe
}
if ($LASTEXITCODE -ne 0) { throw "Repository validation failed with exit code $LASTEXITCODE." }

$manifestArguments = @{
    ApkPath = $apk
    AaptPath = $aapt
    ExpectedVersionName = $ExpectedVersionName
    ExpectedVersionCode = $ExpectedVersionCode
    ExpectedMinSdk = $ExpectedMinSdk
    ExpectedTargetSdk = $ExpectedTargetSdk
}
if (-not [string]::IsNullOrWhiteSpace($ExpectedPackageId)) { $manifestArguments.ExpectedPackageId = $ExpectedPackageId }
if ($AllowNetworkPermissions) { $manifestArguments.AllowNetworkPermissions = $true }
& $manifestScript @manifestArguments
if ($LASTEXITCODE -ne 0) { throw "Android manifest gate failed with exit code $LASTEXITCODE." }

& $bundleScript -AabPath $aab -RequireArm64 -Require16KPageAlignment -ReadElfPath $readElf
if ($LASTEXITCODE -ne 0) { throw "Android bundle gate failed with exit code $LASTEXITCODE." }

$iconPath = Join-Path $ProjectRoot 'Assets\BattleRaja\Art\V1\BattleRaja-AppIcon-PlayStore.png'
$featurePath = Join-Path $ProjectRoot 'Assets\BattleRaja\Art\V1\BattleRaja-FeatureGraphic-PlayStore.png'
& $creativeScript -IconPath $iconPath -FeatureGraphicPath $featurePath
if ($LASTEXITCODE -ne 0) { throw "Store creative technical gate failed with exit code $LASTEXITCODE." }

$status = @(git -C $ProjectRoot status --porcelain=v1)
if ($RequireCleanWorktree -and $status.Count -gt 0) {
    throw "Worktree is not clean; review before treating the artifact as a final candidate:`n$($status -join [Environment]::NewLine)"
}

$summary = [ordered]@{
    ProjectRoot = $ProjectRoot
    ApkPath = $apk
    ApkSha256 = (Get-FileHash -LiteralPath $apk -Algorithm SHA256).Hash
    AabPath = $aab
    AabSha256 = (Get-FileHash -LiteralPath $aab -Algorithm SHA256).Hash
    RepositoryValidation = 'Passed'
    AndroidManifest = if ($AllowNetworkPermissions) { 'Passed with network-permission override' } else { 'Passed with offline network-permission gate' }
    AndroidBundle = 'Passed ARM64 and 16 KB static checks'
    StoreCreativeTechnicalDimensions = 'Passed'
    Worktree = if ($status.Count -eq 0) { 'Clean' } else { "Dirty ($($status.Count) change(s)); review required" }
    HumanApproval = 'Still required for final package identity, signing, privacy/Data Safety, content rating, cultural review and Play submission.'
}

Write-Host 'V1 release candidate technical gate passed.'
$summary | ConvertTo-Json -Depth 4
