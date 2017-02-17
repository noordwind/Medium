#!/usr/bin/env bash
dotnet restore --source "https://api.nuget.org/v3/index.json" --no-cache
dotnet build **/project.json
