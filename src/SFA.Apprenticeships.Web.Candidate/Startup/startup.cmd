﻿@echo off
@echo Installing “IPv4 Address and Domain Restrictions” feature
%windir%\system32\dism.exe /online /quiet /enable-feature /featurename:IIS-IPSecurity 
@echo Unlocking configuration for “IPv4 Address and Domain Restrictions” feature
%windir%\system32\inetsrv\AppCmd.exe unlock config -section:system.webServer/security/ipSecurity