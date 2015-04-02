@echo off

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe ".\ACA.sln" /t:Build /p:Configuration=Release /distributedFileLogger
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe ".\AMCA.sln" /t:Build /p:Configuration=Release /distributedFileLogger
if %errorlevel% EQU 0 goto deploy 
if %errorlevel% NEQ 0 goto end

:deploy
echo compile is successful, starting deploy
echo 1.deleting PrecompiledAMCAWeb directory
rd /s/q PrecompiledAMCAWeb  

echo 2.creating PrecompiledAMCAWeb directory
md PrecompiledAMCAWeb
md PrecompiledAMCAWeb\AMCA
md PrecompiledAMCAWeb\App_GlobalResources
md PrecompiledAMCAWeb\Bin
md PrecompiledAMCAWeb\Config

echo 3.deploying file to PrecompiledAMCAWeb directory
xcopy /r/y/s /exclude:deploy-exclude-AMCA.txt Web\AMCA PrecompiledAMCAWeb\AMCA
xcopy /r/y/s /exclude:deploy-exclude-AMCA.txt Web\App_GlobalResources PrecompiledAMCAWeb\App_GlobalResources
xcopy /r/y/s Web\Bin PrecompiledAMCAWeb\Bin
xcopy /r/y/s Web\Config PrecompiledAMCAWeb\Config
xcopy /r/y/s Web\Web.config PrecompiledAMCAWeb\
xcopy /r/y/s Web\Error.aspx PrecompiledAMCAWeb\
xcopy /r/y/s Web\Global.asax PrecompiledAMCAWeb\


if %errorlevel% EQU 0 echo deploy successfully 
if %errorlevel% GTR 0 echo deploy failed, please check destination directory

goto end

:end
if [%1] NEQ [nopause] pause


