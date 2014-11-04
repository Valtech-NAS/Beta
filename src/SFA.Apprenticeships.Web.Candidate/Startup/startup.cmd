@echo off
echo Installing “IPv4 Address and Domain Restrictions” feature
%windir%\system32\dism.exe /online /quiet /enable-feature /featurename:IIS-IPSecurity 
echo Unlocking configuration for “IPv4 Address and Domain Restrictions” feature
%windir%\system32\inetsrv\AppCmd.exe unlock config -section:system.webServer/security/ipSecurity
echo Create Performance Counters if necessary
REM PowerShell -Version 2.0 -ExecutionPolicy Unrestricted "%ROLEROOT%\approot\bin\startup\startup.ps1"

if "%EMULATED%"=="true" goto SKIP

cd %ROLEROOT%\approot\bin\startup

reg add HKLM\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell /v ExecutionPolicy /d Unrestricted /f
powershell .\startup.ps1 >> StartUp.log 2>> StartUpError.log

:SKIP
EXIT /B 0