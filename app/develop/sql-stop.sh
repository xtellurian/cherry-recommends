#!/bin/bash

set -e
echo "Stopping SQL Server Docker Container..."

docker kill sql1

echo "Stopped sql1"