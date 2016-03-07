#!/usr/bin/env bash

# Setup dnx
curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
dnvm install latest -r clr -arch x86 -alias default

# Restore dependencies
dnu restore
