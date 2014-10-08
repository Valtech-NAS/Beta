param(
	[string]$buildConfiguration,
    [string]$subscription,
	[string]$projectname,
	[string]$service,
	[string]$storage,
	[string]$slot,
	[string]$buildNumber
)

#$subscription = "Visual Studio Ultimate with MSDN"
#$projectname = "$project.Azure"
#$service = "web-candidatedev"
#$storage = "webcandidatedev"
#$slot = "staging" #staging or production
$package = "src\$projectname\bin\$buildConfiguration\app.publish\$projectname.cspkg"
$configuration = "src\$projectname\bin\$buildConfiguration\app.publish\ServiceConfiguration.$projectConfiguration.cscfg"
$timeStampFormat = "g"
$deploymentLabel = "ContinuousDeploy to $service v$buildNumber"

Write-Output "Using $configuration"
 
Write-Output "Running Azure Imports"
Import-Module "C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\ServiceManagement\Azure\*.psd1"
Import-AzurePublishSettingsFile "C:\TeamCity\Visual Studio Ultimate with MSDN-6-13-2014-credentials.publishsettings"
Set-AzureSubscription -CurrentStorageAccount $storage -SubscriptionName $subscription
 
function Publish(){
 $deployment = Get-AzureDeployment -ServiceName $service -Slot $slot -ErrorVariable a -ErrorAction silentlycontinue 
 
 if ($a[0] -ne $null) {
    Write-Output "$(Get-Date -f $timeStampFormat) - No deployment is detected. Creating a new deployment. "
 }
 
 if ($deployment.Name -ne $null) {
    #Update deployment inplace (usually faster, cheaper, won't destroy VIP)
    Write-Output "$(Get-Date -f $timeStampFormat) - Deployment exists in $service.  Upgrading deployment."
    UpgradeDeployment
 } else {
    CreateNewDeployment
 }
}
 
function CreateNewDeployment()
{
    write-progress -id 3 -activity "Creating New Deployment" -Status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Creating New Deployment: In progress"
 
    $opstat = New-AzureDeployment -Slot $slot -Package $package -Configuration $configuration -label $deploymentLabel -ServiceName $service
 
    $completeDeployment = Get-AzureDeployment -ServiceName $service -Slot $slot
    $completeDeploymentID = $completeDeployment.deploymentid
 
    write-progress -id 3 -activity "Creating New Deployment" -completed -Status "Complete"
    Write-Output "$(Get-Date -f $timeStampFormat) - Creating New Deployment: Complete, Deployment ID: $completeDeploymentID"
}
 
function UpgradeDeployment()
{
    write-progress -id 3 -activity "Upgrading Deployment" -Status "In progress"
    Write-Output "$(Get-Date -f $timeStampFormat) - Upgrading Deployment: In progress"
 
    # perform Update-Deployment
    $setdeployment = Set-AzureDeployment -Upgrade -Slot $slot -Package $package -Configuration $configuration -label $deploymentLabel -ServiceName $service -Force
 
    $completeDeployment = Get-AzureDeployment -ServiceName $service -Slot $slot
    $completeDeploymentID = $completeDeployment.deploymentid
 
    write-progress -id 3 -activity "Upgrading Deployment" -completed -Status "Complete"
    Write-Output "$(Get-Date -f $timeStampFormat) - Upgrading Deployment: Complete, Deployment ID: $completeDeploymentID"
}
 
Write-Output "Create Azure Deployment"
Publish