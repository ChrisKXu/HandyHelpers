#!/usr/bin/env bash

# Setup dnx
curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
dnvm upgrade -r mono

# Restore dependencies
dnu restore --quiet

# Build
dnu build ./HandyHelpers/project.json ./HandyHelpers.Tests/project.json --quiet

# Test
dnx -p HandyHelpers.Tests test

# All clear
echo "All clear!"
