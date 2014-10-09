param(
	[string]$buildConfiguration,
    [string]$StorageAccountName,
	[string]$projectname,
	[string]$sitename,
	[string]$storage,
	[string]$storagekey,
	[string]$buildnumber,
	[string]$environment,
	[string]$projectConfiguration
)

$srcpath = "src\$projectname\bin\$buildConfiguration\app.publish"
 
Write-Output "Running Azure Imports"
Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\ServiceManagement\Azure\*.psd1"
Import-AzurePublishSettingsFile "C:\TeamCity\Visual Studio Ultimate with MSDN-6-13-2014-credentials.publishsettings"

Write-Host "Files will be uploaded to $storageAccountName\$storage"
$context = New-AzureStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $storagekey

Write-Host "Writing files" 
$fqName = "$srcpath\ServiceConfiguration.$projectConfiguration.cscfg"
Set-AzureStorageBlobContent -Blob "$environment\$sitename\$buildnumber\ServiceConfiguration.$projectConfiguration.cscfg" -Container "$storage" -File "$fqName" -Context $context -Force

$fqName = "$srcpath\$projectname.cspkg"
Set-AzureStorageBlobContent -Blob "$environment\$sitename\$buildnumber\$projectname.cspkg" -Container "$storage" -File "$fqName" -Context $context -Force