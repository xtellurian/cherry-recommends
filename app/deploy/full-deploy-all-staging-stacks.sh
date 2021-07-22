
az account show

start_dir=$(pwd)
echo $start_dir
set -e
for STACK in upstreet-dev vitable-dev
do
    echo $STACK
    cd $start_dir
	cd ../azure
    pulumi stack select $STACK
	cd $start_dir
    ./full-deploy.sh
done

echo "Completely done"