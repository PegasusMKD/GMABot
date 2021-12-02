#!/bin/bash

git pull origin
dotnet publish -c Release
dotnet ./bin/Release/net6.0/publish/GMABot.dll