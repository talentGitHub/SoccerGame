@echo off
REM Build script for Futsal Game
echo ================================================
echo Building Futsal Game...
echo ================================================

cd FutsalGame

echo.
echo Restoring dependencies...
dotnet restore

echo.
echo Building in Release mode...
dotnet build -c Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ================================================
    echo Build succeeded!
    echo ================================================
    echo.
    echo To run the game:
    echo   cd FutsalGame
    echo   dotnet run
    echo.
    echo Or run the executable from:
    echo   FutsalGame\bin\Release\net10.0-windows\FutsalGame.exe
    echo.
) else (
    echo.
    echo ================================================
    echo Build failed!
    echo ================================================
    echo.
)

pause
