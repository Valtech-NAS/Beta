%windir%\system32\certutil -addstore -enterprise -v -f Root "certificates\imsgatewayproxytestRoot.cer" >> "%TEMP%\StartupLog.txt" 2>&1
REM %windir%\system32\certutil -addstore -enterprise -v -f Root "certificates\imsnasproxytestClient.cer" >> "%TEMP%\StartupLog.txt" 2>&1
REM %windir%\system32\certutil -addstore -enterprise -v -f Root "certificates\test.pfx" >> "%TEMP%\StartupLog.txt" 2>&1
EXIT /B 0
