@echo off
REM if "%EMULATED%"=="true" goto SKIP

cd %ROLEROOT%\approot\bin\startup

reg add HKLM\Software\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell /v ExecutionPolicy /d Unrestricted /f
powershell .\VacancyEtlStartup.ps1 >> StartUp.log 2>> StartUpError.log

:SKIP
EXIT /B 0