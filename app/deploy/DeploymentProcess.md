# Deployment Process


Some steps must be repeated every time, others are only required on an initial deployment.

> These steps are run in `app/deploy/full-multi-deploy.sh`

To get started...

```
cd app/deploy
```

## Step 1 - Create or Update Infrastructure with Pulumi

```
pulumi up -y
```

## Step 1A - Setup Domain, DNS, and SSL

> This step is manual, and is only required the first time a new multitenant deployment is created.

### Create DNS records on cherry.ai DNS zone (or alternate if desired)

```
cd azure-scripts
./update-zone.sh <stack-name>
```

### Link App Service Certificate 

The `App Service Certificate` in the `application` resource group must be linked to the Key Vault in the same resource group.

### Verify the Certificate Domain

The `App Service Certificate`` in the `application` resource group must be verified. (via DNS records set above)

### Set the Custom Domain Name

The `App Service` needs to have it's custom domain name set to something like: `*.canary.cherry.ai`

### Connect Certificate

The Custom Domain in the `App Service` should be linked to the `App Service Certificate` configured above.

## Step 2 - Apply migrations to the Tenant database

```
Hosting__Multitenant="true" ./update-tenant-sql-database.sh
```

## Step 3 - Deploy the dotnet Azure Functions

```
./deploy-dotnet-functions.sh
```

## Step 4 - Deploy the python functions

```
./deploy-python-functions.sh
```

## Step 5 - Deploy the web application to Azure Web App

```
./deploy.sh
```

