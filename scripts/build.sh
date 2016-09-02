#!/usr/bin/env bash

# Restore dependencies
dotnet restore --quiet

# Build
dotnet build ./HandyHelpers/project.json ./HandyHelpers.Tests/project.json --quiet

# Test
# Appending "-parallel none" to workaround a known bug that causes dnx to hang when using mono
# See http://stackoverflow.com/questions/33581380/dnx-test-on-travis-with-mono-hangs
dotnet -p HandyHelpers.Tests test -parallel none

# All clear
echo "All clear!"
