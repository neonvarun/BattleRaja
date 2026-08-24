[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$UnityExe,

    [string]$ProjectRoot = (Get-Location).Path,

    [ValidateSet('editmode', 'playmode')]
    [string]$TestPlatform = 'editmode',

    [string]$TestResults,

    [string]$LogFile
)

$ErrorActionPreference = 'Stop'

$resolvedProjectRoot = (Resolve-Path -LiteralPath $ProjectRoot).Path
$resolvedUnityExe = (Resolve-Path -LiteralPath $UnityExe).Path

if (-not $TestResults) {
    $TestResults = Join-Path $resolvedProjectRoot "Builds\Local\TestResults\$TestPlatform.xml"
}

if (-not $LogFile) {
    $LogFile = Join-Path $resolvedProjectRoot "Builds\Local\Logs\$TestPlatform.log"
}

$resultsParent = Split-Path -Parent $TestResults
$logParent = Split-Path -Parent $LogFile
if ($resultsParent) {
    New-Item -ItemType Directory -Force -Path $resultsParent | Out-Null
}
if ($logParent) {
    New-Item -ItemType Directory -Force -Path $logParent | Out-Null
}

$unityArguments = @(
    '-batchmode'
    '-nographics'
    '-projectPath'
    $resolvedProjectRoot
    '-runTests'
    '-testPlatform'
    $TestPlatform
    '-testResults'
    $TestResults
    '-logFile'
    $LogFile
)

# Do not add -quit here. Unity's test runner exits after it writes the report;
# combining -quit with -runTests can terminate the editor before tests execute.
$process = Start-Process -FilePath $resolvedUnityExe `
    -ArgumentList $unityArguments `
    -WorkingDirectory $resolvedProjectRoot `
    -Wait `
    -PassThru

if ($process.ExitCode -ne 0) {
    throw "Unity $TestPlatform tests exited with code $($process.ExitCode). See $LogFile"
}

if (-not (Test-Path -LiteralPath $TestResults -PathType Leaf)) {
    throw "Unity did not produce the expected $TestPlatform test report: $TestResults"
}

[xml]$report = Get-Content -LiteralPath $TestResults -Raw
$run = $report.'test-run'
if (-not $run) {
    throw "The Unity test report does not contain a test-run element: $TestResults"
}

$total = [int]$run.total
$passed = [int]$run.passed
$failed = [int]$run.failed
$skipped = [int]$run.skipped

if ($run.result -ne 'Passed' -or $failed -ne 0) {
    throw "Unity $TestPlatform tests did not pass (result=$($run.result), total=$total, passed=$passed, failed=$failed, skipped=$skipped). See $TestResults"
}

[pscustomobject]@{
    Platform = $TestPlatform
    Result = $run.result
    Total = $total
    Passed = $passed
    Failed = $failed
    Skipped = $skipped
    TestResults = (Resolve-Path -LiteralPath $TestResults).Path
    LogFile = (Resolve-Path -LiteralPath $LogFile -ErrorAction SilentlyContinue).Path
}
