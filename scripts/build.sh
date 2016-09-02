#!/usr/bin/env bash

# Restore dependencies
dotnet restore

# Build
dotnet build ./HandyHelpers/project.json ./HandyHelpers.Tests/project.json

# Test
dotnet test HandyHelpers.Tests

# All clear
echo "All clear!"
