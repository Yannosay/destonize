@echo off
setlocal

set PROJECT_ROOT=%~dp0
set PROJECT_FILE=%PROJECT_ROOT%src\Destonize\Destonize.csproj
set DOTNET_CMD=%USERPROFILE%\.dotnet\dotnet.exe

if exist "%DOTNET_CMD%" (
    echo Using local .NET SDK: %DOTNET_CMD%
) else (
    where dotnet >nul 2>nul
    if %ERRORLEVEL% EQU 0 (
        set DOTNET_CMD=dotnet
    ) else (
        echo dotnet not found. Install .NET 8 SDK and try again.
        pause
        exit /b 1
    )
)

echo Building Destonize...
"%DOTNET_CMD%" build "%PROJECT_FILE%" -c Release
if %ERRORLEVEL% NEQ 0 (
    echo Build failed.
    pause
    exit /b 1
)

echo.
echo Destonize Build completed successfully.
echo Executable: %PROJECT_ROOT%src\Destonize\bin\Release\net8.0-windows\Destonize.exe
pause
exit /b 0