#!/usr/bin/env bash

# Restore dependencies
dotnet restore

# Build
dotnet build ./HandyHelpers/project.json ./HandyHelpers.Tests/project.json

# Test
dotnet HandyHelpers.Tests test

# All clear
echo "All clear!"
