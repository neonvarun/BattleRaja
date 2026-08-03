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
