@echo off

%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "Accela.AuthorizedAgent.Client.sln" /t:Build /p:Configuration=Release /distributedFileLogger
if %errorlevel% EQU 0 goto deploy 
if %errorlevel% NEQ 0 goto end

:deploy
echo compile is successful, starting deploy
echo 1.deleting PrecompiledWeb directory
rd /s/q AuthorizedAgentClient  

echo 2.creating PrecompiledWeb directory
md AuthorizedAgentClient 

echo 3.deploying file to PrecompiledWeb directory
xcopy /r/y/s /exclude:deploy-exclude.txt "Accela.AuthorizedAgent.Client\bin\Release" "AuthorizedAgentClient"
xcopy /r/y/s "Accela.AuthorizedAgent.Client\bin\Release\AccelaDocProxySetting.xml" "AuthorizedAgentClient\AccelaDocProxySetting.xml"

if %errorlevel% EQU 0 echo deploy successfully 
if %errorlevel% GTR 0 echo deploy failed, please check destination directory

goto end

:end
if [%1] NEQ [nopause] pause