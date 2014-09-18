param(
	[string]$storageName,
	[string]$storageKey,
	[string]$storageContainer,
	[string]$buildConfiguration,
	[string]$projectName,
	[string]$buildNumber
)

$srcPath = "src\$projectName\bin\$buildConfiguration\app.publish"
$version = (Get-Content src\version.txt) + $buildNumber

Write-Output "Running Azure Imports"
Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\ServiceManagement\Azure\*.psd1"
#Import-AzurePublishSettingsFile "C:\TeamCity\Visual Studio Ultimate with MSDN-6-13-2014-credentials.publishsettings"

Write-Host "Files will be uploaded to $storageAccountName\$storage"
$context = New-AzureStorageContext -StorageAccountName $storageName -StorageAccountKey $storageKey

Write-Host "Writing files" 
#$fqName = "$srcPath\ServiceConfiguration.Cloud.cscfg"
#Set-AzureStorageBlobContent -Blob "$storeageFolder\$buildnumber\ServiceConfiguration.Cloud.cscfg" -Container "$storage" -File "$fqName" -Context $context -Force

$fqName = "$srcPath\$projectName.cspkg"
Set-AzureStorageBlobContent -Blob "$storageContainer\$projectName-$version.cspkg" -Container $storageName -File $fqName -Context $context -Force