# Setup dnx

$Branch='dev'
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))
dnvm install latest -r clr -arch x86 -alias default

# Restore dependencies
dnu restore

# Build
dnu build ./HandyHelpers/project.json --quiet
dnu build ./HandyHelpers.Tests/project.json --quiet

# Test
dnx -p HandyHelpers.Tests test

# Pack
dnu pack ./HandyHelpers/project.json --quiet --out ./artifacts --configuration Release

# All clear
Write-Host "All clear!"
