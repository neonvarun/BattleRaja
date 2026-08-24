[CmdletBinding()]
param(
    [string]$ProjectRoot,
    [switch]$RequireUnityProject,
    [string]$UnityExe
)

$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($ProjectRoot)) {
    $ProjectRoot = Join-Path $PSScriptRoot '..\..'
}
$ProjectRoot = [System.IO.Path]::GetFullPath((Resolve-Path -LiteralPath $ProjectRoot).Path)

$errors = [System.Collections.Generic.List[string]]::new()
$warnings = [System.Collections.Generic.List[string]]::new()

function Add-ValidationError([string]$Message) { [void]$errors.Add($Message) }
function Add-ValidationWarning([string]$Message) { [void]$warnings.Add($Message) }

Write-Host "BattleRaja validation: $ProjectRoot"

$requiredPaths = @(
    'AGENTS.md',
    'START_HERE.md',
    'PROJECT_STATUS.md',
    'PROJECT_CONTEXT.json',
    'Docs\MASTER_VISION.md',
    'Docs\WEB_PLATFORM.md',
    'Docs\MILESTONE_0_EXECUTION_PLAN.md',
    'Assets\BattleRaja',
    'Assets\WebGLTemplates\BattleRaja\index.html',
    '.gitattributes',
    '.gitignore'
)

foreach ($relativePath in $requiredPaths) {
    if (-not (Test-Path -LiteralPath (Join-Path $ProjectRoot $relativePath))) {
        Add-ValidationError "Required path is missing: $relativePath"
    }
}

$webTemplatePath = Join-Path $ProjectRoot 'Assets\WebGLTemplates\BattleRaja\index.html'
if (Test-Path -LiteralPath $webTemplatePath) {
    $webTemplate = Get-Content -LiteralPath $webTemplatePath -Raw
    if ($webTemplate -notmatch 'id="unity-canvas"[^>]*tabindex="0"') {
        Add-ValidationError 'WebGL template canvas must be keyboard-focusable with tabindex="0".'
    }
    if ($webTemplate -notmatch 'canvas\.addEventListener\("pointerdown"') {
        Add-ValidationError 'WebGL template must restore canvas focus on pointer interaction.'
    }
}

$unityProjectMarkers = @(
    'ProjectSettings\ProjectVersion.txt',
    'Packages\manifest.json',
    'Packages\packages-lock.json'
)
$missingUnityMarkers = @($unityProjectMarkers | Where-Object { -not (Test-Path -LiteralPath (Join-Path $ProjectRoot $_)) })
if ($missingUnityMarkers.Count -gt 0) {
    $message = "Unity project markers are missing: $($missingUnityMarkers -join ', ')"
    if ($RequireUnityProject) { Add-ValidationError $message } else { Add-ValidationWarning $message }
}

$attributes = Get-Content -LiteralPath (Join-Path $ProjectRoot '.gitattributes') -Raw
foreach ($pattern in @('*.fbx', '*.blend', '*.psd', '*.png', '*.jpg', '*.jpeg', '*.tga', '*.wav', '*.mp3', '*.ogg', '*.mp4', '*.mov')) {
    if ($attributes -notmatch [regex]::Escape($pattern)) {
        Add-ValidationWarning ".gitattributes does not list the expected LFS pattern: $pattern"
    }
}

$manifestPath = Join-Path $ProjectRoot 'Packages\manifest.json'
if (Test-Path -LiteralPath $manifestPath) {
    try {
        $manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
        $packageText = ($manifest.dependencies | ConvertTo-Json -Depth 20)
        if ($packageText -match '(?i)photon|playfab') {
            Add-ValidationError 'Packages/manifest.json contains a prohibited Photon or PlayFab dependency.'
        }
    } catch {
        Add-ValidationError "Packages/manifest.json is not valid JSON: $($_.Exception.Message)"
    }
}

# V1 is an offline, no-analytics/no-ads candidate. Keep Unity services disabled
# in the checked-in project settings so the release privacy worksheet matches the
# build configuration. Future online/service milestones must change this rule
# deliberately with an updated data-safety decision.
$projectSettingsPath = Join-Path $ProjectRoot 'ProjectSettings\ProjectSettings.asset'
if (Test-Path -LiteralPath $projectSettingsPath) {
    $projectSettingsText = Get-Content -LiteralPath $projectSettingsPath -Raw
    if ($projectSettingsText -match '(?m)^\s*submitAnalytics:\s*1\s*$') {
        Add-ValidationError 'V1 offline candidate must keep submitAnalytics disabled in ProjectSettings.asset.'
    }
}

$unityConnectSettingsPath = Join-Path $ProjectRoot 'ProjectSettings\UnityConnectSettings.asset'
if (Test-Path -LiteralPath $unityConnectSettingsPath) {
    $unityConnectText = Get-Content -LiteralPath $unityConnectSettingsPath -Raw
    foreach ($servicePattern in @(
        '(?ms)^\s*m_Enabled:\s*1\s*$.*?UnityAnalyticsSettings:',
        '(?ms)^\s*UnityAnalyticsSettings:\s*.*?^\s*m_Enabled:\s*1\s*$'
    )) {
        if ($unityConnectText -match $servicePattern) {
            Add-ValidationError 'V1 offline candidate must keep Unity Analytics disabled in UnityConnectSettings.asset.'
            break
        }
    }
    if ($unityConnectText -match '(?ms)^\s*UnityAdsSettings:\s*.*?^\s*m_Enabled:\s*1\s*$') {
        Add-ValidationError 'V1 offline candidate must keep Unity Ads disabled in UnityConnectSettings.asset.'
    }
    if ($unityConnectText -match '(?ms)^\s*PerformanceReportingSettings:\s*.*?^\s*m_Enabled:\s*1\s*$') {
        Add-ValidationError 'V1 offline candidate must keep Performance Reporting disabled in UnityConnectSettings.asset.'
    }
}

# Core must remain replaceable by a network/server transport. Keep its two
# assemblies free of Unity/vendor dependencies and reject presentation code
# reaching directly into simulation mutators.
$coreRoots = @(
    (Join-Path $ProjectRoot 'Assets\BattleRaja\Core\Domain'),
    (Join-Path $ProjectRoot 'Assets\BattleRaja\Core\Application')
) | Where-Object { Test-Path -LiteralPath $_ }
$coreForbiddenPattern = '(?i)\b(UnityEngine|UnityEditor|Photon|Fusion|PlayFab)\b'
foreach ($coreRoot in $coreRoots) {
    Get-ChildItem -LiteralPath $coreRoot -Recurse -File -Filter '*.cs' -ErrorAction SilentlyContinue | ForEach-Object {
        if (Select-String -LiteralPath $_.FullName -Pattern $coreForbiddenPattern -Quiet -ErrorAction SilentlyContinue) {
            Add-ValidationError "Core code contains a Unity/vendor dependency: $([System.IO.Path]::GetRelativePath($ProjectRoot, $_.FullName))"
        }
    }
}

foreach ($asmdefRelativePath in @(
    'Assets\BattleRaja\Core\BattleRaja.Core.Domain.asmdef',
    'Assets\BattleRaja\Core\Application\BattleRaja.Core.Application.asmdef'
)) {
    $asmdefPath = Join-Path $ProjectRoot $asmdefRelativePath
    if (-not (Test-Path -LiteralPath $asmdefPath)) { continue }
    try {
        $asmdef = Get-Content -LiteralPath $asmdefPath -Raw | ConvertFrom-Json
        if ($asmdef.noEngineReferences -ne $true) {
            Add-ValidationError "Core assembly must set noEngineReferences=true: $asmdefRelativePath"
        }
        if (($asmdef.references | ConvertTo-Json -Depth 10) -match '(?i)photon|fusion|playfab|unityengine') {
            Add-ValidationError "Core assembly has a prohibited reference: $asmdefRelativePath"
        }
    } catch {
        Add-ValidationError "Core assembly definition is not valid JSON: $asmdefRelativePath"
    }
}

$presentationRoots = @(
    (Join-Path $ProjectRoot 'Assets\BattleRaja\Presentation')
) | Where-Object { Test-Path -LiteralPath $_ }
$presentationSimulationMutationPattern = '(?i)\b(?:Simulation|simulation)\.(?:SyncHealth|Heal|ApplyDamage|RecordDamage|SetPosition|Advance|Restart|Start)\s*\('
foreach ($presentationRoot in $presentationRoots) {
    Get-ChildItem -LiteralPath $presentationRoot -Recurse -File -Filter '*.cs' -ErrorAction SilentlyContinue | ForEach-Object {
        if (Select-String -LiteralPath $_.FullName -Pattern $presentationSimulationMutationPattern -Quiet -ErrorAction SilentlyContinue) {
            Add-ValidationError "Presentation code directly mutates OfflineMatchSimulation: $([System.IO.Path]::GetRelativePath($ProjectRoot, $_.FullName))"
        }
    }
}

$gitCommand = Get-Command git -ErrorAction SilentlyContinue
$isGitWorktree = $false
if ($null -ne $gitCommand) {
    $gitResult = (& $gitCommand.Source -C $ProjectRoot rev-parse --is-inside-work-tree 2>$null | Select-Object -First 1)
    $isGitWorktree = ($gitResult -eq 'true')
}
if ($isGitWorktree) {
    $tracked = @(& $gitCommand.Source -C $ProjectRoot ls-files)
    foreach ($forbiddenName in @('Library', 'Temp', 'Obj', 'Build', 'Builds', 'Logs', 'UserSettings', '.utmp')) {
        $prefix = "$forbiddenName/"
        if ($tracked | Where-Object { $_ -like "$prefix*" }) {
            Add-ValidationError "Forbidden generated path is tracked: $forbiddenName"
        }
    }
} else {
    Add-ValidationWarning 'Git worktree is not initialized; tracked-file forbidden-path scan was skipped.'
}

$scanRoots = @('Assets', 'Packages', 'ProjectSettings', 'Tools') | ForEach-Object { Join-Path $ProjectRoot $_ } | Where-Object { Test-Path -LiteralPath $_ }
$scanExtensions = @('.cs', '.asmdef', '.json', '.xml', '.yaml', '.yml', '.asset', '.ps1', '.props', '.targets', '.gradle', '.config')
$secretPattern = '(?i)(api[_-]?key|client[_-]?secret|access[_-]?token|private[_-]?key|password)\s*[:=]\s*\S'
foreach ($scanRoot in $scanRoots) {
    Get-ChildItem -LiteralPath $scanRoot -Recurse -File -ErrorAction SilentlyContinue | Where-Object { $scanExtensions -contains $_.Extension.ToLowerInvariant() } | ForEach-Object {
        $matches = Select-String -LiteralPath $_.FullName -Pattern $secretPattern -AllMatches -ErrorAction SilentlyContinue
        if ($matches) {
            Add-ValidationError "Potential secret assignment found in $([System.IO.Path]::GetRelativePath($ProjectRoot, $_.FullName))"
        }
    }
}

if ($RequireUnityProject) {
    if (-not [string]::IsNullOrWhiteSpace($UnityExe)) {
        if (-not (Test-Path -LiteralPath $UnityExe)) {
            Add-ValidationError "Supplied Unity executable does not exist: $UnityExe"
        }
    } else {
        $unityCommand = Get-Command Unity.exe -ErrorAction SilentlyContinue
        if ($null -eq $unityCommand) {
            Add-ValidationError 'Unity.exe is not discoverable on PATH; pass -UnityExe with the approved editor path.'
        }
    }
}

foreach ($warning in $warnings) { Write-Warning $warning }
foreach ($errorMessage in $errors) { Write-Host "ERROR: $errorMessage" -ForegroundColor Red }

Write-Host ("Validation summary: {0} error(s), {1} warning(s)." -f $errors.Count, $warnings.Count)
if ($errors.Count -gt 0) { exit 2 }
exit 0
