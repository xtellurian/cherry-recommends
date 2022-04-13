#!/bin/bash

set -e
echo "Stopping SQL Server Docker Container..."

docker kill azurite1

echo "Stopped sql1"