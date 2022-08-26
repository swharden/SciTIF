rmdir /s /q ..\src\SciTIF\bin\Release
dotnet build ../src/ --configuration Release
explorer ..\src\SciTIF\bin\Release
pause