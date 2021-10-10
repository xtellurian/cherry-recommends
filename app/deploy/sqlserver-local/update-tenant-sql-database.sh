set -e

cd ../../web

CONTEXT="MultiTenantDbContext"
DATABASE=tenants
CS="Server=127.0.0.1,1433;Database=$DATABASE;User Id=SA;Password=YourStrong@Passw0rd"

dotnet ef database update --context $CONTEXT --connection "$CS" --project ../migrations/sqlserver -- --Provider sqlserver --Hosting:Multitenant "true"
