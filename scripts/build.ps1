# Restore dependencies
dotnet restore

# Build
dotnet build ./HandyHelpers/project.json ./HandyHelpers.Tests/project.json

# Test
dotnet HandyHelpers.Tests test

# Pack
dotnet pack ./HandyHelpers/project.json --out ./artifacts --configuration Release

# All clear
Write-Host "All clear!"
