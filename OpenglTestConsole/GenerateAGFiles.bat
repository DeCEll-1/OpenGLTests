@echo off
set path1=%~dp0OpenglTestConsole\Resources\Resources.hjson
set path2=%~dp0OpenglTestConsole\Generated
set path3=%~dp0OpenglTestConsole\
echo %path1%
echo %path2%
echo %path3%

%~dp0ResourcesClassGenerator\bin\Debug\net8.0\ResourcesClassGenerator.exe %path1% %path2% %path3% "OpenglTestConsole" "AppResources"

set path1=%~dp0RGL\Resources\Resources.hjson
set path2=%~dp0RGL\Generated
set path3=%~dp0RGL\
echo %path1%
echo %path2%
echo %path3%

%~dp0ResourcesClassGenerator\bin\Debug\net8.0\ResourcesClassGenerator.exe %path1% %path2% %path3% "RGL" "RGLResources"
