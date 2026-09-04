[CmdletBinding()]
param(
    [string]$ProjectRoot = (Get-Location).Path,
    [string]$Destination
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$resolvedProjectRoot = (Resolve-Path -LiteralPath $ProjectRoot).Path
$kitRelative = 'Docs\AGENT_KIT\T3_BattleRaja_Agent_Package'
$kitRoot = Join-Path $resolvedProjectRoot $kitRelative

$requiredProjectFiles = @(
    'AGENTS.md',
    'PROJECT_STATUS.md',
    'PROMPTS\README.md',
    'PROMPTS\99_MASTER_V1_GOAL.md',
    'Docs\MASTER_VISION.md',
    'Docs\ARCHITECTURE.md',
    'Docs\DECISIONS.md',
    'Docs\RESEARCH_LOG.md',
    (Join-Path $kitRelative 'README.md'),
    (Join-Path $kitRelative 'T3_FRESH_AGENT_START_PROMPT.md'),
    (Join-Path $kitRelative 'SKILLS_AND_MCP_MANIFEST.json'),
    (Join-Path $kitRelative 'MCP_INSTALL.md')
)

$missing = @($requiredProjectFiles | Where-Object {
    -not (Test-Path -LiteralPath (Join-Path $resolvedProjectRoot $_) -PathType Leaf)
})
if ($missing.Count -gt 0) {
    throw "BattleRaja T3 kit preflight failed; missing: $($missing -join ', ')"
}

$manifestPath = Join-Path $kitRoot 'SKILLS_AND_MCP_MANIFEST.json'
$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json

if (-not [string]::IsNullOrWhiteSpace($Destination)) {
    $resolvedDestination = [System.IO.Path]::GetFullPath($Destination)
    New-Item -ItemType Directory -Force -Path $resolvedDestination | Out-Null
    foreach ($entry in (Get-ChildItem -LiteralPath $kitRoot -Force)) {
        Copy-Item -LiteralPath $entry.FullName -Destination (Join-Path $resolvedDestination $entry.Name) -Recurse -Force
    }
    Write-Output "Copied BattleRaja T3 agent kit to $resolvedDestination"
}

Write-Output "BattleRaja T3 agent kit verified at $kitRoot"
Write-Output "Manifest version: $($manifest.version)"
Write-Output "Recorded head hint: $($manifest.latestRecordedHead) (actual HEAD always wins)"
Write-Output "Required skills: $($manifest.requiredSkills.Count)"
Write-Output "Declared MCP plugins: $($manifest.mcpPlugins.Count)"
Write-Output "Approved device: $($manifest.environment.approvedDeviceSerial) ($($manifest.environment.approvedDeviceModel))"
Write-Output 'Remote MCP installation remains host-level; use MCP_INSTALL.md and the Codex plugin picker.'
