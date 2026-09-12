#define MyAppName "Destonize"
#define MyAppVersion "0.0.1"
#define MyAppPublisher "Yannosay Productions"
#define MyAppExeName "Destonize.exe"

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
OutputDir=C:\Users\Yannosay\Desktop\STUFF\YOUTUBE PROJECT\destonize\dist
OutputBaseFilename=DestonizeSetup
SetupIconFile=C:\Users\Yannosay\Desktop\STUFF\YOUTUBE PROJECT\destonize\publish\assets\app-icon.ico
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
Source: "C:\Users\Yannosay\Desktop\STUFF\YOUTUBE PROJECT\destonize\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent