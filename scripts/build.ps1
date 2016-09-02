# Restore dependencies
dotnet restore

# Build
dotnet build ./HandyHelpers/project.json ./HandyHelpers.Tests/project.json

# Test
dotnet -p HandyHelpers.Tests test

# Pack
dotnet pack ./HandyHelpers/project.json --quiet --out ./artifacts --configuration Release

# All clear
Write-Host "All clear!"
