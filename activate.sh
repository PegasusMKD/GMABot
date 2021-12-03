#!/bin/bash

cd /home/pi/GMABot
sudo git fetch --all
sudo git reset --hard HEAD
sudo git pull origin
/home/pi/.dotnet/dotnet publish -c Release
/home/pi/.dotnet/dotnet ./bin/Release/net6.0/publish/GMABot.dll > /home/pi/logs/dotnet/dotnet.log 2>/home/pi/logs/dotnet/dotnet-errors.log
