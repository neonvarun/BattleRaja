[CmdletBinding()]
param(
    [string]$IconPath = 'Assets/BattleRaja/Art/V1/BattleRaja-AppIcon-Candidate.png',
    [string]$FeatureGraphicPath = 'Assets/BattleRaja/Art/V1/BattleRaja-FeatureArt-Candidate.png',
    [string]$ScreenshotDirectory,
    [switch]$RequireFinal
)

$ErrorActionPreference = 'Stop'

function Read-PngDimensions {
    param([Parameter(Mandatory = $true)][string]$Path)

    $bytes = [System.IO.File]::ReadAllBytes((Resolve-Path -LiteralPath $Path).Path)
    if ($bytes.Length -lt 24 -or $bytes[0] -ne 0x89 -or $bytes[1] -ne 0x50 -or
        $bytes[2] -ne 0x4e -or $bytes[3] -ne 0x47 -or $bytes[12] -ne 0x49 -or
        $bytes[13] -ne 0x48 -or $bytes[14] -ne 0x44 -or $bytes[15] -ne 0x52) {
        throw "Only PNG files with a valid IHDR are supported: $Path"
    }

    $width = [System.Net.IPAddress]::NetworkToHostOrder([BitConverter]::ToInt32($bytes, 16))
    $height = [System.Net.IPAddress]::NetworkToHostOrder([BitConverter]::ToInt32($bytes, 20))
    [pscustomobject]@{ Width = $width; Height = $height }
}

function Test-Asset {
    param(
        [Parameter(Mandatory = $true)][string]$Label,
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][int]$ExpectedWidth,
        [Parameter(Mandatory = $true)][int]$ExpectedHeight
    )

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        return [pscustomobject]@{ Asset = $Label; Path = $Path; Dimensions = 'missing'; Status = 'Missing' }
    }

    $dimensions = Read-PngDimensions -Path $Path
    $status = if ($dimensions.Width -eq $ExpectedWidth -and $dimensions.Height -eq $ExpectedHeight) {
        'Pass'
    } else {
        'Draft-size mismatch'
    }

    [pscustomobject]@{
        Asset = $Label
        Path = (Resolve-Path -LiteralPath $Path).Path
        Dimensions = "$($dimensions.Width)x$($dimensions.Height)"
        Status = $status
    }
}

$results = @(
    (Test-Asset -Label 'App icon' -Path $IconPath -ExpectedWidth 512 -ExpectedHeight 512),
    (Test-Asset -Label 'Feature graphic' -Path $FeatureGraphicPath -ExpectedWidth 1024 -ExpectedHeight 500)
)

if ($ScreenshotDirectory) {
    if (-not (Test-Path -LiteralPath $ScreenshotDirectory -PathType Container)) {
        $results += [pscustomobject]@{ Asset = 'Screenshots'; Path = $ScreenshotDirectory; Dimensions = 'missing'; Status = 'Missing' }
    } else {
        $screenshots = @(Get-ChildItem -LiteralPath $ScreenshotDirectory -Filter '*.png' -File)
        if ($screenshots.Count -eq 0) {
            $results += [pscustomobject]@{ Asset = 'Screenshots'; Path = $ScreenshotDirectory; Dimensions = 'none'; Status = 'Missing' }
        } else {
            foreach ($screenshot in $screenshots) {
                $dimensions = Read-PngDimensions -Path $screenshot.FullName
                $valid = $dimensions.Width -ge 320 -and $dimensions.Height -ge 320
                $results += [pscustomobject]@{
                    Asset = "Screenshot: $($screenshot.Name)"
                    Path = $screenshot.FullName
                    Dimensions = "$($dimensions.Width)x$($dimensions.Height)"
                    Status = if ($valid) { 'Pass' } else { 'Too small' }
                }
            }
        }
    }
}

$results | Format-Table -AutoSize
$failures = @($results | Where-Object { $_.Status -ne 'Pass' })
if ($RequireFinal -and $failures.Count -gt 0) {
    throw "Store creative gate failed: $($failures.Count) item(s) need owner-approved final assets or dimensions."
}

if ($failures.Count -gt 0) {
    Write-Output "Store creative status: draft/open ($($failures.Count) item(s) require review)."
} else {
    Write-Output 'Store creative status: all supplied assets meet the technical dimension gate.'
}
