cd ../../../azure

$DATABASE=$args[0]

if([string]::IsNullOrEmpty($DATABASE)){
   echo "Usage: $0 <database-name>"
   exit 1
}

$STACK=$(pulumi stack --show-name)
echo "Using Pulumi Stack $STACK"

$SERVER=$(pulumi stack output SqlServerName)
$USER=$(pulumi stack output SqlServerUsername)
$PW=$(pulumi stack output SqlServerPassword --show-secrets)

$CS="Server=tcp:$SERVER.database.windows.net,1433;Initial Catalog=$DATABASE;User ID=$USER;Password=$PW;Min Pool Size=0;Max Pool Size=30;Persist Security Info=False;";

cd ../web

$CONTEXT="SignalBoxDbContext"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --Provider sqlserver --Hosting:SingleTenantDatabaseName $DATABASE
