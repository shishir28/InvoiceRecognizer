$resourceGroup = "MonadFormRecoRG"
$location = "australiaeast"
$storageAccName = "monadformrecstrg"
$storageSKU = "Standard_LRS"

# Create resource group
az group create -n $resourceGroup -l $location

az storage account create  --location $location  --name $storageAccName  --resource-group  $resourceGroup  --sku $storageSKU