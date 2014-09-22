param(
	[string]$subscriptionName,
	[string]$storageName,
	[string]$storageAccessKey,
	[string]$storageContainerName,
	[string]$buildConfiguration,
	[string]$projectName,
	[string]$buildNumber
)

$srcPath = "Beta\src\$projectName\bin\$buildConfiguration\app.publish"
$version = (Get-Content Beta\src\version.txt -ErrorAction Stop) + $buildNumber

Write-Output "Storing package at: $srcPath for version: $version"

Write-Output "Running Azure Imports"
#Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\ServiceManagement\Azure\*.psd1"
Import-AzurePublishSettingsFile "G:\Azure\Pre-Production-9-12-2014-credentials.publishsettings"
Set-AzureSubscription -CurrentStorageAccount $storageName -SubscriptionName $subscriptionName
Write-Host "Files will be uploaded to $storageAccountName\$storage$storageContainer"
$context = New-AzureStorageContext -StorageAccountName $storageName -StorageAccountKey $storageAccessKey

Write-Host "Copying files to Azure Storage" 

#DON'T NEED THIS FILE AS THIS IS IN THE SECURE REPO.
#$fqName = "$srcPath\ServiceConfiguration.Cloud.cscfg"
#Set-AzureStorageBlobContent -Blob "$storeageFolder\$buildnumber\ServiceConfiguration.Cloud.cscfg" -Container "$storage" -File "$fqName" -Context $context -Force

$fqName = "$srcPath\$projectName.cspkg"
Write-Host "Copying file: $fqName to $storageContainerName\$projectName-$version.cspkg"
Set-AzureStorageBlobContent -Blob "$storageContainerName\$projectName-$version.cspkg" -Container $storageName -File $fqName -Context $context -Force
Write-Host "Copied file: $fqName to $storageContainerName\$projectName-$version.cspkg"

Return $error.Count