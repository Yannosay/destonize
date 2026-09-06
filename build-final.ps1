[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$projectRoot = $PSScriptRoot
$srcDir = Join-Path -Path $projectRoot -ChildPath "src\Destonize"
$publishDir = Join-Path -Path $projectRoot -ChildPath "publish"
$distDir = Join-Path -Path $projectRoot -ChildPath "dist"
$issPath = Join-Path -Path $projectRoot -ChildPath "installer.iss"
$assetsDir = Join-Path -Path $projectRoot -ChildPath "assets"
$version = "0.0.1"
$companyName = "Yannosay Productions"
$productName = "Destonize"
$exeName = "Destonize.exe"
$githubUrl = "https://github.com/Yannosay/Destonize"

function Write-Utf8NoBom {
    param([string]$Path, [string]$Content)
    $parent = Split-Path -Path $Path -Parent
    if (-not (Test-Path -Path $parent)) {
        New-Item -Path $parent -ItemType Directory -Force | Out-Null
    }
    [System.IO.File]::WriteAllText($Path, $Content, [System.Text.UTF8Encoding]::new($false))
}

function Resolve-DotNetCommand {
    $cmd = Get-Command dotnet -ErrorAction SilentlyContinue
    if ($cmd) {
        try {
            $null = & dotnet --version 2>$null
            if ($LASTEXITCODE -eq 0) { return "dotnet" }
        } catch {}
    }
    $userDotnetExe = Join-Path -Path $env:USERPROFILE -ChildPath ".dotnet\dotnet.exe"
    if (Test-Path -Path $userDotnetExe) {
        $env:PATH = "$(Split-Path $userDotnetExe -Parent);$env:PATH"
        return $userDotnetExe
    }
    throw "dotnet command not found. Install .NET 8 SDK and try again."
}

function Find-InnoSetupCompiler {
    $candidatePaths = @(
        "C:\Program Files\Inno Setup 7\ISCC.exe",
        "C:\Program Files (x86)\Inno Setup 7\ISCC.exe",
        "C:\Program Files\Inno Setup 6\ISCC.exe",
        "C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
        "C:\Program Files\Inno Setup 5\ISCC.exe",
        "C:\Program Files (x86)\Inno Setup 5\ISCC.exe"
    )
    foreach ($path in $candidatePaths) {
        if (Test-Path -Path $path) {
            return $path
        }
    }

    $programFiles = @("C:\Program Files", "C:\Program Files (x86)")
    foreach ($pf in $programFiles) {
        if (Test-Path -Path $pf) {
            $found = Get-ChildItem -Path $pf -Filter "ISCC.exe" -Recurse -ErrorAction SilentlyContinue |
                     Where-Object { $_.FullName -match "Inno Setup" } |
                     Select-Object -First 1
            if ($found) {
                return $found.FullName
            }
        }
    }

    return $null
}

Write-Host "Cleaning previous build artifacts..." -ForegroundColor Cyan
if (Test-Path -Path $publishDir) { Remove-Item -Path $publishDir -Recurse -Force }
if (Test-Path -Path $distDir) { Remove-Item -Path $distDir -Recurse -Force }

Write-Host "Publishing application..." -ForegroundColor Cyan
$dotnetCommand = Resolve-DotNetCommand
& $dotnetCommand publish "$srcDir\Destonize.csproj" -c Release -r win-x64 --self-contained false -o "$publishDir"
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed with exit code $LASTEXITCODE"
}

Write-Host "Copying assets..." -ForegroundColor Cyan
$publishAssetsDir = Join-Path -Path $publishDir -ChildPath "assets"
if (-not (Test-Path -Path $publishAssetsDir)) { New-Item -Path $publishAssetsDir -ItemType Directory -Force | Out-Null }
Copy-Item -Path (Join-Path -Path $assetsDir -ChildPath "app-icon.ico") -Destination $publishAssetsDir -Force
Copy-Item -Path (Join-Path -Path $assetsDir -ChildPath "splash-screen.png") -Destination $publishAssetsDir -Force

Write-Host "Generating Inno Setup script..." -ForegroundColor Cyan
$issContent = @"
#define MyAppName "$productName"
#define MyAppVersion "$version"
#define MyAppPublisher "$companyName"
#define MyAppExeName "$exeName"

[Setup]
AppId={{8F2A6F8E-6F3E-4B3A-9D9B-1F2C3A4E5B6C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=LICENSE.txt
InfoBeforeFile=ACCESS_NOTICE.txt
OutputDir=$distDir
OutputBaseFilename=DestonizeSetup
SetupIconFile=$publishAssetsDir\app-icon.ico
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "$publishDir\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
"@
Write-Utf8NoBom -Path $issPath -Content $issContent

$licensePath = Join-Path -Path $projectRoot -ChildPath "LICENSE.txt"
$licenseText = @"
GNU GENERAL PUBLIC LICENSE
Version 3, 29 June 2007

Copyright (C) 2024 Yannosay Productions

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <https://www.gnu.org/licenses/>.
"@
Write-Utf8NoBom -Path $licensePath -Content $licenseText

$accessNoticePath = Join-Path -Path $projectRoot -ChildPath "ACCESS_NOTICE.txt"
$accessNotice = @"
ACCESS NOTICE

Destonize requires access to your Desktop folder and the ability to move files within it.
The application will never delete files or alter files outside the Desktop unless explicitly directed by you.
By continuing with the installation, you acknowledge that you understand and accept this access.

For more information, visit $githubUrl
"@
Write-Utf8NoBom -Path $accessNoticePath -Content $accessNotice

Write-Host "Checking for Inno Setup compiler..." -ForegroundColor Cyan
$isccPath = Find-InnoSetupCompiler

if (-not $isccPath) {
    Write-Host "Inno Setup compiler not found. Please install Inno Setup from https://jrsoftware.org/isinfo.php" -ForegroundColor Yellow
    Write-Host "If installed in a custom location, set the ISCC_PATH environment variable." -ForegroundColor Yellow
    return
}

Write-Host "Using Inno Setup compiler: $isccPath" -ForegroundColor Cyan
& $isccPath $issPath
if ($LASTEXITCODE -ne 0) {
    throw "Inno Setup compilation failed with exit code $LASTEXITCODE"
}

Write-Host "`nInstaller created successfully." -ForegroundColor Green
Write-Host "Setup file: $distDir\DestonizeSetup.exe" -ForegroundColor White