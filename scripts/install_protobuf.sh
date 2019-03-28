#!/bin/bash


d="Darwin"
l="Linux"
if [ "$(uname -s | grep ${d})" != "" ]; then
  osn="macosx"
elif [ "$(uname -s | grep ${l})" != "" ]; then
  osn="linux"
else
  osn="windows"
fi

if [ $osn == "macosx" ]; then
    brew install protobuf
elif [ $osn == "linux" ]; then
    # Make sure you grab the latest version
    curl -OL https://github.com/google/protobuf/releases/download/v3.7.0/protoc-3.7.0-linux-x86_64.zip
    
    # Unzip
    unzip protoc-3.7.0-linux-x86_64.zip -d protoc3
    
    # Move protoc to /usr/local/bin/
    sudo mv protoc3/bin/* /usr/local/bin/
    
    # Move protoc3/include to /usr/local/include/
    sudo mv protoc3/include/* /usr/local/include/
    
    # Optional: change owner
    sudo chown $USER /usr/local/bin/protoc
    sudo chown -R $USER /usr/local/include/google
fi