@ECHO OFF

SETLOCAL

CALL ..\..\..\build\set35vars.bat

%msbuildexe% Cyotek.Data.Nbt.sln /p:Configuration=Release /verbosity:minimal /nologo /t:Clean,Build
src\Benchmarks\bin\Release\Benchmarks.exe

ENDLOCAL
