set -e
cd ../web

CONFIGURATION=$1


if [ -z "$CONFIGURATION" ]
then
      echo "Using Debug configuration"
      CONFIGURATION="Debug"
else
      echo "Using Debug $CONFIGURATION"
fi

echo "Running dotnet publish..."

dotnet publish -c $CONFIGURATION

echo "dotnet publish complete"
echo "stopping"