#!/usr/bin/env bash

# Setup dnx
curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
dnvm upgrade -r mono

# Restore dependencies
dnu restore

# Build
cd HandyHelpers && dnu build && cd ..
cd HandyHelpers.Tests && dnu build && cd ..
dnx -p HandyHelpers.Tests test
