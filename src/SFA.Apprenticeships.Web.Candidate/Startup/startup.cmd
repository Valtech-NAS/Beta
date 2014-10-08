@echo off
@echo Installing “IPv4 Address and Domain Restrictions” feature
%windir%\System32\ServerManagerCmd.exe -install Web-IP-Security
@echo Unlocking configuration for “IPv4 Address and Domain Restrictions” feature
%windir%\system32\inetsrv\AppCmd.exe unlock config -section:system.webServer/security/ipSecurity