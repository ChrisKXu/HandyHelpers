#!/usr/bin/env bash

# Setup dnx
curl -sSL https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.sh | DNX_BRANCH=dev sh && source ~/.dnx/dnvm/dnvm.sh
dnvm install latest

# Restore dependencies
dnu restore
