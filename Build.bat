@echo off

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe ".\ACA.sln" /t:Build /p:Configuration=Release /distributedFileLogger
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe ".\AMCA.sln" /t:Build /p:Configuration=Release /distributedFileLogger
if %errorlevel% EQU 0 goto deploy 
if %errorlevel% NEQ 0 goto end

:deploy
echo compile is successful, starting deploy
echo 1.deleting PrecompiledWeb directory
rd /s/q PrecompiledWeb  

echo 2.creating PrecompiledWeb directory
md PrecompiledWeb 

echo 3.deploying file to PrecompiledWeb directory
xcopy /r/y/s /exclude:deploy-exclude.txt Web PrecompiledWeb
xcopy /r/y/s Web\*.css PrecompiledWeb
xcopy /r/y/s Web\ClientBin\AuthorizedAgentClient.cs PrecompiledWeb\ClientBin

echo 4.creating Customize component related directories if necessary
IF NOT EXIST .\PrecompiledWeb\Customize\DLL md PrecompiledWeb\Customize\DLL
IF NOT EXIST .\PrecompiledWeb\Customize\UserControls md PrecompiledWeb\Customize\UserControls

if %errorlevel% EQU 0 echo deploy successfully 
if %errorlevel% GTR 0 echo deploy failed, please check destination directory

goto end

:end
if [%1] NEQ [nopause] pause


