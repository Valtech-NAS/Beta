param(
	[string]$subscriptionName,
	[string]$storageName,
	[string]$storageKey,
	[string]$storageContainer,
	[string]$buildConfiguration,
	[string]$projectName,
	[string]$buildNumber
)

$srcPath = "src\$projectName\bin\$buildConfiguration\app.publish"
$version = (Get-Content src\version.txt) + $buildNumber

Write-Output "Storing package at: $srcPath for version: $version"

Write-Output "Running Azure Imports"
Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\ServiceManagement\Azure\*.psd1"
Import-AzurePublishSettingsFile "G:\Azure\Pre-Production-9-12-2014-credentials.publishsettings"
Set-AzureSubscription -CurrentStorageAccount $storageName -SubscriptionName $subscriptionName

Write-Host "Files will be uploaded to $storageAccountName\$storage$storageContainer"
$context = New-AzureStorageContext -StorageAccountName $storageName -StorageAccountKey $storageKey

Write-Host "Copying files to Azure Storage" 

#DON'T NEED THIS FILE AS THIS IS IN THE SECURE REPO.
#$fqName = "$srcPath\ServiceConfiguration.Cloud.cscfg"
#Set-AzureStorageBlobContent -Blob "$storeageFolder\$buildnumber\ServiceConfiguration.Cloud.cscfg" -Container "$storage" -File "$fqName" -Context $context -Force

$fqName = "$srcPath\$projectName.cspkg"
Write-Host "Copying file: $fqName to $storageContainer\$projectName-$version.cspkg"
Set-AzureStorageBlobContent -Blob "$storageContainer\$projectName-$version.cspkg" -Container $storageName -File $fqName -Context $context -Force
Write-Host "Copied file: $fqName to $storageContainer\$projectName-$version.cspkg"