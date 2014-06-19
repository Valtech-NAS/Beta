param(
	[string]$buildConfiguration,
    [string]$StorageAccountName,
	[string]$projectname,
	[string]$sitename,
	[string]$storage,
	[string]$storagekey,
	[string]$buildnumber
)

#$projectname = "SFA.Apprenticeships.Web.Candidate.Azure"
#$sitename = "Candidate"
#$buildConfiguration = "Release"
#$StorageAccountName = "nasbuilds"
#$storage = "archive"
#$storageAccountKey = "P1wxhz2gYA9dPs26J4oJc2vTidBt0aV6oHgi7KR6QGJnpMHU+gE6h5jyK8sB7eQ3Sdr8LrVe/4yIVAvAeQhAQw=="
$srcpath = "src\$projectname\bin\$buildConfiguration\app.publish"
#$buildnumber = "%dep.SfaApprenticeships_Ci_CompileAndUnitTest.system.BuildNumber%"
 
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