[Setup]
AppName=Destonize
AppVersion=0.0.1
AppPublisher=Yannosay Productions
AppPublisherURL=https://github.com/YannosayProductions/Destonize
AppSupportURL=https://github.com/YannosayProductions/Destonize
AppUpdatesURL=https://github.com/YannosayProductions/Destonize
DefaultDirName={autopf}\Destonize
DefaultGroupName=Destonize
OutputBaseFilename=DestonizeSetup
OutputDir=.
Compression=lzma
SolidCompression=yes
WizardStyle=modern
UninstallDisplayIcon={app}\Destonize.exe
LicenseFile=..\..\docs\LICENSE
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=no
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Additional icons:"

[Files]
Source: "..\output\Destonize.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\output\Destonize.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\output\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\output\*.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\output\assets\*"; DestDir: "{app}\assets"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\..\assets\app-icon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\assets\splash-screen.png"; DestDir: "{app}\assets"; Flags: ignoreversion

[Icons]
Name: "{group}\Destonize"; Filename: "{app}\Destonize.exe"
Name: "{group}\Uninstall Destonize"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Destonize"; Filename: "{app}\Destonize.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\Destonize.exe"; Description: "Launch Destonize"; Flags: nowait postinstall skipifsilent

[Code]
var
  AccessPage: TInputQueryWizardPage;
  ConsentCheck: TNewCheckBox;

procedure InitializeWizard;
begin
  AccessPage := CreateInputQueryPage(wpLicense,
    'Access Notice', 'Important information about Destonize',
    'Destonize will access your Desktop folder to organize files according to your rules.' + #13#10 +
    'It will create folders and move files based on your configuration.' + #13#10 +
    'You can undo the last move and customize all rules from the settings.');
  AccessPage.Add('', False, '');

  ConsentCheck := TNewCheckBox.Create(Page);
  ConsentCheck.Parent := AccessPage.Surface;
  ConsentCheck.Top := 140;
  ConsentCheck.Left := 0;
  ConsentCheck.Width := AccessPage.SurfaceWidth;
  ConsentCheck.Caption := 'I understand and accept access to my Desktop';
  ConsentCheck.Checked := False;
  ConsentCheck.OnClick := @ConsentCheckOnClick;
end;

procedure ConsentCheckOnClick(Sender: TObject);
begin
  WizardForm.NextButton.Enabled := ConsentCheck.Checked;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
  if CurPageID = AccessPage.ID then
    Result := ConsentCheck.Checked;
end;