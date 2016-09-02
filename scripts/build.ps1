# Restore dependencies
dotnet restore

# Build
dotnet build ./HandyHelpers/project.json ./HandyHelpers.Tests/project.json

# Test
dotnet test HandyHelpers.Tests

# Pack
# dotnet pack ./HandyHelpers/project.json --out ./artifacts --configuration Release

# All clear
Write-Host "All clear!"
