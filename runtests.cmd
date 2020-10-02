@ECHO OFF

SETLOCAL

SET SCRIPTPATH=%~dp0
SET SCRIPTPATH=%SCRIPTPATH:~0,-1%

CD %SCRIPTPATH%

CALL ..\..\..\build\set35vars.bat

SET RESULTDIR=%1
IF "%RESULTDIR%"=="" SET RESULTDIR=testresults

IF NOT EXIST %RESULTDIR% MKDIR %RESULTDIR%

SET DLLNAME=Cyotek.Data.Nbt.Tests
SET SRCDIR=tests\
SET SLNNAME=Cyotek.Data.Nbt.sln

dotnet build %SLNNAME% --configuration Release
IF %ERRORLEVEL% NEQ 0 GOTO :testsfailed

CALL :runtests net35
CALL :runtests net40
CALL :runtests net45
CALL :runtests net452
CALL :runtests net462
CALL :runtests net472
CALL :runtests net48
REM CALL :runtests netstandard2.0
REM CALL :runtests netstandard2.1
REM CALL :runtests netcoreapp2.1
REM CALL :runtests netcoreapp3.1
REM CALL :runtests netcoreapp2.2

ENDLOCAL

GOTO :eof

:runtests
SET TRG=%1
%nunitexe% %SRCDIR%bin\x86\release\%TRG%\%DLLNAME%.dll /result=testresults\%DLLNAME%.%TRG%.xml %nunitargs%
IF %ERRORLEVEL% NEQ 0 GOTO :testsfailed
GOTO :eof

:testsfailed
CECHO {0c}ERROR: *** TEST RUN FAILED ***{#}{\n}
EXIT /b 1
