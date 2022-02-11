set -e

echo "> docker ps"
docker ps

THIS_DIR=$(pwd)
DATA_DIR="$THIS_DIR/.persist"

echo "SQL Data will be persisted in $DATA_DIR"

echo "Starting SQL Server Docker Container..."


docker start sql1 || \
   sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" \
   -p 1433:1433 --name sql1 -h sql1 \
   -v $DATA_DIR/data:/var/opt/mssql/data \
   -v $DATA_DIR/log:/var/opt/mssql/log \
   -v $DATA_DIR/secrets:/var/opt/mssql/secrets \
   -d mcr.microsoft.com/mssql/server:2019-latest