[CmdletBinding()]
param(
    [string]$ProjectRoot,
    [string]$UnityExe,
    [string]$BuildMethod = 'BattleRaja.Editor.BuildEntrypoints.BuildAndroidDevelopment'
)

$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($ProjectRoot)) { $ProjectRoot = Join-Path $PSScriptRoot '..\..\..' }
$ProjectRoot = [System.IO.Path]::GetFullPath((Resolve-Path -LiteralPath $ProjectRoot).Path)
if ([string]::IsNullOrWhiteSpace($UnityExe)) {
    $unityCommand = Get-Command Unity.exe -ErrorAction SilentlyContinue
    if ($null -ne $unityCommand) { $UnityExe = $unityCommand.Source }
}
if ([string]::IsNullOrWhiteSpace($UnityExe) -or -not (Test-Path -LiteralPath $UnityExe)) {
    throw 'Unity.exe was not supplied or is not discoverable. Install the approved Unity editor before running the Android build.'
}
if (-not (Test-Path -LiteralPath (Join-Path $ProjectRoot 'ProjectSettings\ProjectVersion.txt'))) {
    throw 'The repository is not yet a Unity project: ProjectSettings\ProjectVersion.txt is missing.'
}

$artifactRoot = Join-Path $ProjectRoot 'Builds\M6\Android'
$logRoot = Join-Path $ProjectRoot 'Builds\M6\Logs'
New-Item -ItemType Directory -Force -Path $artifactRoot, $logRoot | Out-Null
$logPath = Join-Path $logRoot 'android-build.log'
$unityArguments = @(
    '-batchmode', '-nographics', '-quit',
    '-projectPath', $ProjectRoot,
    '-executeMethod', $BuildMethod,
    '-buildTarget', 'Android',
    '-logFile', $logPath
)
$unityProcess = Start-Process -FilePath $UnityExe -ArgumentList $unityArguments -WorkingDirectory $ProjectRoot -Wait -PassThru
if ($unityProcess.ExitCode -ne 0) { throw "Unity Android build failed with exit code $($unityProcess.ExitCode). See $logPath" }
Write-Host "Android build completed. The editor entrypoint should place the APK under $artifactRoot."
