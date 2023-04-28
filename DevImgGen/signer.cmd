@echo off
if not exist %6 goto:EOF
for /f "delims=*" %%g in ('type %6') do signtool sign /f certificates\OEM_Test_Cert_2017.pfx /fd SHA256 /t http://timestamp.digicert.com "%%g"
del %6