@echo off
REM Publish script for Futsal Game - Creates standalone executable
echo ================================================
echo Publishing Futsal Game...
echo ================================================
echo.
echo This will create a standalone Windows executable
echo with all dependencies included.
echo.

cd FutsalGame

echo Publishing...
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ================================================
    echo Publish succeeded!
    echo ================================================
    echo.
    echo Standalone executable created at:
    echo   FutsalGame\bin\Release\net10.0-windows\win-x64\publish\FutsalGame.exe
    echo.
    echo You can copy this file to any Windows PC and run it
    echo without installing .NET.
    echo.
) else (
    echo.
    echo ================================================
    echo Publish failed!
    echo ================================================
    echo.
)

pause
