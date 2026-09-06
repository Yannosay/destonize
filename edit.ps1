[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = $PSScriptRoot
$srcDir = Join-Path -Path $projectRoot -ChildPath "src\Destonize"
$organizeXamlPath = Join-Path -Path $srcDir -ChildPath "OrganizeWindow.xaml"

function Write-Utf8NoBom {
    param([string]$Path, [string]$Content)
    $parent = Split-Path -Path $Path -Parent
    if (-not (Test-Path -Path $parent)) {
        New-Item -Path $parent -ItemType Directory -Force | Out-Null
    }
    [System.IO.File]::WriteAllText($Path, $Content, [System.Text.UTF8Encoding]::new($false))
}

if (Test-Path -Path $organizeXamlPath) {
    $content = [System.IO.File]::ReadAllText($organizeXamlPath)
    $content = $content.Replace('Exclusions & Inclusions', 'Exclusions and Inclusions')
    Write-Utf8NoBom -Path $organizeXamlPath -Content $content
    Write-Host "Fixed XAML entity issue in OrganizeWindow.xaml" -ForegroundColor Green
} else {
    Write-Host "OrganizeWindow.xaml not found" -ForegroundColor Yellow
}

Write-Host "Initializing Git repository..." -ForegroundColor Cyan
Set-Location -Path $projectRoot

if (-not (Test-Path -Path ".git")) {
    & git init
    if ($LASTEXITCODE -ne 0) {
        throw "git init failed with exit code $LASTEXITCODE"
    }
    Write-Host "Git repository initialized." -ForegroundColor Green
} else {
    Write-Host "Git repository already exists." -ForegroundColor Yellow
}

& git add .
if ($LASTEXITCODE -ne 0) {
    throw "git add failed with exit code $LASTEXITCODE"
}

& git commit -m "Initial commit of Destonize project"
if ($LASTEXITCODE -ne 0) {
    throw "git commit failed with exit code $LASTEXITCODE"
}

Write-Host "`nGit repository created and initial commit done." -ForegroundColor Green
Write-Host "To create a remote repository on GitHub:" -ForegroundColor Cyan
Write-Host "1. Go to https://github.com/new and create a repository named 'Destonize' under 'YannosayProductions'." -ForegroundColor White
Write-Host "2. Run the following commands to push:" -ForegroundColor White
Write-Host "   git remote add origin https://github.com/YannosayProductions/Destonize.git" -ForegroundColor Gray
Write-Host "   git branch -M main" -ForegroundColor Gray
Write-Host "   git push -u origin main" -ForegroundColor Gray