$resourceGroup = "MonadFormRecoRG"
$location = "australiaeast"
#$storageAccName = "monadformrecstrg"
#$storageSKU = "Standard_LRS"
$formRecoName = "MonadFormReco"
$forRecoSKU = "S0"

# Create resource group
az group create -n $resourceGroup -l $location

# az storage account create  --location $location  --name $storageAccName  --resource-group  $resourceGroup  --sku $storageSKU

az cognitiveservices account create  --name $formRecoName --resource-group $resourceGroup --kind FormRecognizer --sku $forRecoSKU  --location $location  --yes


#az cognitiveservices account show --name "InvoiceFormReco" --resource-group "InvoiceFormRG" --query "properties.endpoint"