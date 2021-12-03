#!/bin/bash

git pull origin
/home/pi/.dotnet/dotnet publish -c Release
/home/pi/.dotnet/dotnet ./bin/Release/net6.0/publish/GMABot.dll