# GitHub-Azure-Function-Proxy-for-ARM-Template
Allows you to use your private GitHub repo for Azure Linked ARM templates.

## Create the Azure Function
1. Create an Azure Function of type .NET named GitHubProxy
2. Paste the Azure-Function.cs into a new function
3. Login into GitHub
   - Click on your name (top right)
   - Select Settings
   - Go to Developer Settings
   - Go to Personal Access Token
   - Click Generate New Token
   - Give it name
   - Select repo scope access
4. Paste your Personal Access Token into the Azure Function 
   - string personalAcccessToken = "{REMOVED}"; 

## To call the Azuze Function
- Press the "Get function URL" in the Azure Portal
- e.g. https://{removed}.azurewebsites.net/api/{removed}?code={removed}
- Add the query string &location={raw url in GitHub}
- Sample: https://{removed}.azurewebsites.net/api/{removed}?code={removed}&location=https://raw.githubusercontent.com/AdamPaternostro/GitHub-Azure-Function-Proxy-for-ARM-Template/master/README.md
- You can test this in a browser window

## To Run via command line (Linux)
- You need to change the URL in the azuredeploy.json
```
# Login
az login

# Select Subscription
az account set -s REPLACE_ME

# Script parameters
resourceGroup="Azure-Function-Proxy"
location="eastus"
today=`date +%Y-%m-%d-%H-%M-%S`
deploymentName="MyDeployment-$today"

# Create resource group
az group create \
  --name        $resourceGroup \
  --location    $location

# Deploy the ARM template
az group deployment create \
  --name                 $deploymentName \
  --resource-group       $resourceGroup \
  --template-file        azuredeploy.json

# Clean up resource group
az group delete --name $resourceGroup
```
