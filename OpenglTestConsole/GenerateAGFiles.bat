@echo off
set path1=%~dp0OpenglTestConsole\Resources\Resources.json
set path2=%~dp0OpenglTestConsole\Generated
echo %path1%
echo %path2%

%~dp0ResourcesClassGenerator\bin\Debug\net8.0\ResourcesClassGenerator.exe %path1% %path2%