param(
	[string]$buildConfiguration,
    [string]$StorageAccountName,
	[string]$projectname,
	[string]$sitename,
	[string]$storage,
	[string]$storagekey,
	[string]$buildnumber
)

$srcpath = "src\$projectname\bin\$buildConfiguration\app.publish"
 
Write-Output "Running Azure Imports"
Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\ServiceManagement\Azure\*.psd1"
Import-AzurePublishSettingsFile "C:\TeamCity\Visual Studio Ultimate with MSDN-6-13-2014-credentials.publishsettings"

Write-Host "Files will be uploaded to the storage account/Container"
#create a storage context 
$context = New-AzureStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $storagekey

$fqName = "$srcpath\ServiceConfiguration.Cloud.cscfg"
Write-Host "Writing file $fqName" 
Set-AzureStorageBlobContent -Blob "$sitename\$buildnumber\$fqName" -Container "$storage" -File "$fqName" -Context $context -Force

$fqName = "$srcpath\SFA.Apprenticeships.Web.Candidate.Azure.cspkg"
Write-Host "Writing file $fqName" 
Set-AzureStorageBlobContent -Blob "$sitename\$buildnumber\SFA.Apprenticeships.Web.Candidate.Azure.cspkg" -Container "$storage" -File "$fqName" -Context $context -Force