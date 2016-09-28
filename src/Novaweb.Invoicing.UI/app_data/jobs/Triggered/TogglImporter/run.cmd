@echo off

xcopy %WEBROOT_PATH%\*.dll .\ 
xcopy %WEBROOT_PATH%\*.json .\ 

dotnet .\TogglImporter.dll