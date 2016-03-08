#!/usr/bin/env bash

# Setup dnx
curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
dnvm upgrade -r mono

# Restore dependencies
dnu restore --quiet

# Build
dnu build ./HandyHelpers/project.json ./HandyHelpers.Tests/project.json --quiet

# Test
# Appending "-parallel none" to workaround a known bug that causes dnx to hang when using mono
# See http://stackoverflow.com/questions/33581380/dnx-test-on-travis-with-mono-hangs
dnx -p HandyHelpers.Tests test -parallel none

# All clear
echo "All clear!"
