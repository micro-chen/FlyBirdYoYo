#!/bin/bash

############this section is auto check dotnet sdk##################
#echo ""
#echo "Installing dotnet cli..."
#echo ""
#
#export DOTNET_INSTALL_DIR="./.dotnet/"
#
#tools/install.sh
#
#origPath=$PATH
#export PATH="./dotnet/bin/:$PATH"
#
#if [ $? -ne 0 ]; then
#  echo >&2 ".NET Execution Environment installation has failed."
#  exit 1
#fi
#
#export DOTNET_HOME="$DOTNET_INSTALL_DIR/cli"
#export PATH="$DOTNET_HOME/bin:$PATH"
#
############this section is auto check dotnet sdk##################

echo "begin pull and build !"
#  git clone https://smartdevframework.visualstudio.com/FlyBirdPrint/_git/FlyBirdPrint

# 停止掉nginx 对此访问重定向
# 停止后台承载dotnet 此站点进程
git checkout dev
git pull  origin  dev 

dotnet build 
dotnet publish  --configuration Release ./FlyBirdPrint.Web/FlyBirdPrint.Web.csproj
#启动后台承载dotnet 此站点进程
#启动掉nginx 对此访问重定向

