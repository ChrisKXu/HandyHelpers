#!/usr/bin/env bash

# Workarounds for dnx hang problem in docker
export LANG=en_US.UTF-8
export LANGUAGE=en_US.UTF-8
export LC_ALL=en_US.UTF-8

# Setup dnx
curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
dnvm upgrade -r mono

# Restore dependencies
dnu restore --quiet

# Build
dnu build ./HandyHelpers/project.json --quiet
dnu build ./HandyHelpers.Tests/project.json --quiet

# Test
dnx -p HandyHelpers.Tests test

# All clear
echo "All clear!"
