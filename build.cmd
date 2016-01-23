@ECHO OFF

SETLOCAL

CALL ..\..\..\build\set35vars.bat

%msbuildexe% Cyotek.Data.Nbt.sln /p:Configuration=Release /verbosity:minimal /nologo /t:Clean,Build
CALL signcmd Cyotek.Data.Nbt\bin\Release\Cyotek.Data.Nbt.dll

PUSHD %CD%

MD nuget > NUL
CD nuget

NUGET pack ..\Cyotek.Data.Nbt\Cyotek.Data.Nbt.csproj -Prop Configuration=Release

POPD

ENDLOCAL
