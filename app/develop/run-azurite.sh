

echo "> docker ps"
docker ps

THIS_DIR=$(pwd)
DATA_DIR="$THIS_DIR/.persist/azurite"

echo "SQL Data will be persisted in $DATA_DIR"

echo "Starting Azurite Docker Container..."

docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 -v $DATA_DIR:/data mcr.microsoft.com/azure-storage/azurite
