[CmdletBinding()]
param(
    [string]$AdbPath,
    [string]$DeviceSerial = 'ST5GDW23LB004392',
    [string]$PackageId = 'com.example.battleraja.m11',
    [string]$OutputDirectory,
    [int]$DurationSeconds = 30,
    [int]$IntervalSeconds = 5,
    [switch]$SkipLaunch
)

$ErrorActionPreference = 'Stop'

if ($DeviceSerial -eq 'b60e53b3') {
    throw 'The Oppo device is not approved for BattleRaja evidence. Use the Lava serial ST5GDW23LB004392.'
}

if ($DurationSeconds -lt 1) { throw 'DurationSeconds must be at least 1.' }
if ($IntervalSeconds -lt 1) { throw 'IntervalSeconds must be at least 1.' }

if ([string]::IsNullOrWhiteSpace($AdbPath)) {
    $adbCommand = Get-Command adb.exe -ErrorAction SilentlyContinue
    if ($null -ne $adbCommand) {
        $AdbPath = $adbCommand.Source
    } else {
        $knownAdb = Join-Path $env:LOCALAPPDATA 'Android\Sdk\platform-tools\adb.exe'
        if (Test-Path -LiteralPath $knownAdb) {
            $AdbPath = $knownAdb
        } else {
            throw 'adb.exe was not found. Pass -AdbPath with the approved Android SDK platform-tools executable.'
        }
    }
}

if (-not (Test-Path -LiteralPath $AdbPath)) {
    throw "adb.exe does not exist: $AdbPath"
}

if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
    $OutputDirectory = Join-Path (Join-Path (Get-Location) 'Builds\Local\Device\Performance') $stamp
}

$OutputDirectory = [System.IO.Path]::GetFullPath($OutputDirectory)
New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

function Invoke-Adb {
    param([Parameter(Mandatory = $true)][string[]]$Arguments)
    $result = & $AdbPath @Arguments 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "adb failed ($LASTEXITCODE): adb $($Arguments -join ' ')`n$($result -join [Environment]::NewLine)"
    }
    return @($result)
}

function Save-AdbCapture {
    param(
        [Parameter(Mandatory = $true)][string[]]$Arguments,
        [Parameter(Mandatory = $true)][string]$Path
    )
    $result = Invoke-Adb -Arguments $Arguments
    [System.IO.File]::WriteAllLines($Path, $result)
}

$deviceRows = Invoke-Adb -Arguments @('devices')
$deviceLine = $deviceRows | Where-Object { $_ -match "^$([regex]::Escape($DeviceSerial))\s+device\s*$" } | Select-Object -First 1
if ($null -eq $deviceLine) {
    throw "Approved device $DeviceSerial is not connected and ready. Connected rows:`n$($deviceRows -join [Environment]::NewLine)"
}

$deviceInfo = Invoke-Adb -Arguments @('-s', $DeviceSerial, 'shell', 'getprop', 'ro.product.model', 'ro.build.version.sdk')
[System.IO.File]::WriteAllLines((Join-Path $OutputDirectory 'device-info.txt'), $deviceInfo)

$resolved = Invoke-Adb -Arguments @('-s', $DeviceSerial, 'shell', 'cmd', 'package', 'resolve-activity', '--brief', $PackageId)
$activity = $resolved | Where-Object { $_ -match '/' -and $_ -notmatch '^priority=' } | Select-Object -Last 1
if ([string]::IsNullOrWhiteSpace($activity)) {
    throw "Could not resolve a launch activity for package $PackageId. Output:`n$($resolved -join [Environment]::NewLine)"
}

Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'getprop') (Join-Path $OutputDirectory 'getprop.txt')
Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'package', $PackageId) (Join-Path $OutputDirectory 'package.txt')
Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'battery') (Join-Path $OutputDirectory 'battery-before.txt')
Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'thermalservice') (Join-Path $OutputDirectory 'thermal-before.txt')

if (-not $SkipLaunch) {
    # Reset only diagnostic state and the app process; do not uninstall, clear data,
    # or alter the owner's device state beyond launching the requested candidate.
    & $AdbPath '-s' $DeviceSerial 'logcat' '-c' | Out-Null
    if ($LASTEXITCODE -ne 0) { throw 'adb logcat -c failed.' }
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'gfxinfo', $PackageId, 'reset') (Join-Path $OutputDirectory 'gfxinfo-reset.txt')
    Invoke-Adb @('-s', $DeviceSerial, 'shell', 'am', 'force-stop', $PackageId) | Out-Null
    Invoke-Adb @('-s', $DeviceSerial, 'shell', 'am', 'start', '-n', $activity) | Out-Null
}

$sampleCount = [Math]::Max(1, [Math]::Ceiling($DurationSeconds / [double]$IntervalSeconds))
$samples = [System.Collections.Generic.List[object]]::new()
for ($sampleIndex = 0; $sampleIndex -lt $sampleCount; $sampleIndex++) {
    $sampleName = 'sample-{0:000}' -f ($sampleIndex + 1)
    $sampleDirectory = Join-Path $OutputDirectory $sampleName
    New-Item -ItemType Directory -Force -Path $sampleDirectory | Out-Null
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'date', '+%s') (Join-Path $sampleDirectory 'epoch-seconds.txt')
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'meminfo', $PackageId) (Join-Path $sampleDirectory 'meminfo.txt')
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'gfxinfo', $PackageId) (Join-Path $sampleDirectory 'gfxinfo.txt')
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'cpuinfo') (Join-Path $sampleDirectory 'cpuinfo.txt')
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'top', '-b', '-n', '1') (Join-Path $sampleDirectory 'top.txt')
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'thermalservice') (Join-Path $sampleDirectory 'thermal.txt')
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'battery') (Join-Path $sampleDirectory 'battery.txt')
    Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'activity', 'activities') (Join-Path $sampleDirectory 'activities.txt')
    $samples.Add([ordered]@{
        index = $sampleIndex + 1
        relativePath = $sampleName
        capturedAtUtc = [DateTime]::UtcNow.ToString('o')
    })
    if ($sampleIndex + 1 -lt $sampleCount) {
        Start-Sleep -Seconds $IntervalSeconds
    }
}

Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'battery') (Join-Path $OutputDirectory 'battery-after.txt')
Save-AdbCapture @('-s', $DeviceSerial, 'shell', 'dumpsys', 'thermalservice') (Join-Path $OutputDirectory 'thermal-after.txt')
Save-AdbCapture @('-s', $DeviceSerial, 'logcat', '-d', '-v', 'threadtime') (Join-Path $OutputDirectory 'logcat.txt')

$logcatText = Get-Content -LiteralPath (Join-Path $OutputDirectory 'logcat.txt') -Raw
$fatalMarkers = @('FATAL EXCEPTION', 'ANR in', 'SIGSEGV', 'SIGABRT', 'NullReferenceException', 'UnityException')
$fatalMatches = foreach ($marker in $fatalMarkers) {
    if ($logcatText.IndexOf($marker, [StringComparison]::OrdinalIgnoreCase) -ge 0) { $marker }
}

$manifest = [ordered]@{
    schema = 1
    packageId = $PackageId
    deviceSerial = $DeviceSerial
    activity = $activity
    durationSeconds = $DurationSeconds
    intervalSeconds = $IntervalSeconds
    sampleCount = $samples.Count
    capturedAtUtc = [DateTime]::UtcNow.ToString('o')
    fatalMarkers = @($fatalMatches)
    samples = @($samples)
}
$manifest | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath (Join-Path $OutputDirectory 'manifest.json') -Encoding UTF8

Write-Host "BattleRaja Android performance capture complete: $OutputDirectory"
Write-Host "Device: $DeviceSerial | Package: $PackageId | Samples: $($samples.Count)"
if ($fatalMatches.Count -gt 0) {
    Write-Warning "Potential fatal markers found in logcat: $($fatalMatches -join ', ')"
} else {
    Write-Host 'Crash-marker scan: no configured fatal markers found.'
}
