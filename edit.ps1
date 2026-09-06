[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = $PSScriptRoot
Set-Location -Path $projectRoot

$remoteUrl = "https://github.com/Yannosay/Destonize.git"

$remoteOutput = & git remote get-url origin 2>$null
if ($LASTEXITCODE -eq 0) {
    $currentRemote = ($remoteOutput | Select-Object -First 1).Trim()
    if ($currentRemote -ne $remoteUrl) {
        & git remote set-url origin $remoteUrl
        Write-Host "Updated origin remote to $remoteUrl" -ForegroundColor Green
    } else {
        Write-Host "Origin remote already correct." -ForegroundColor Yellow
    }
} else {
    & git remote add origin $remoteUrl
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to add remote origin"
    }
    Write-Host "Added origin remote $remoteUrl" -ForegroundColor Green
}

$currentBranch = & git rev-parse --abbrev-ref HEAD
if ($LASTEXITCODE -ne 0) {
    throw "Failed to get current branch"
}
$currentBranch = ($currentBranch | Select-Object -First 1).Trim()

if ($currentBranch -ne "main") {
    & git branch -M main
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to rename branch to main"
    }
    Write-Host "Renamed branch to main." -ForegroundColor Green
}

& git add .
if ($LASTEXITCODE -ne 0) {
    throw "git add failed"
}

$status = & git status --porcelain
if ($status -and $status.Trim().Length -gt 0) {
    & git commit -m "Update project with installer and corrected links"
    if ($LASTEXITCODE -ne 0) {
        throw "git commit failed"
    }
} else {
    Write-Host "Nothing to commit, working tree clean." -ForegroundColor Yellow
}

& git push -u origin main
if ($LASTEXITCODE -ne 0) {
    throw "git push failed"
}

Write-Host "`nPushed to $remoteUrl successfully." -ForegroundColor Green