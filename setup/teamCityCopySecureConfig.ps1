param(
    [string]$checkoutRoot
)

if(!(Test-Path -Path "c:\Projects" )){
    New-Item -ItemType directory -Path "C:\Projects"
}

if(!(Test-Path -Path "c:\Projects\SFA" )){
    New-Item -ItemType directory -Path "C:\Projects\SFA"
}

if(!(Test-Path -Path "c:\Projects\SFA\Apprenticeships" )){
    New-Item -ItemType directory -Path "C:\Projects\SFA\Apprenticeships"
}

if(!(Test-Path -Path "c:\Projects\SFA\Apprenticeships\Configuration" )){
    New-Item -ItemType directory -Path "C:\Projects\SFA\Apprenticeships\Configuration"
}

Write-Output "Removing old config items"

Remove-Item C:\Projects\SFA\Apprenticeships\Configuration\* -recurse

Write-Output "$checkoutRoot\Apprenticeships\Configuration\**"

if((Test-Path -Path "$checkoutRoot\Configuration")){
	Write-Output "Copying config on old build server set up"
	Copy-Item "$checkoutRoot\Configuration\**" "C:\Projects\SFA\Apprenticeships\Configuration" -recurse
}

if((Test-Path -Path "$checkoutRoot\Apprenticeships\Configuration" )){
	Write-Output "Copying config on new build server set up"
	Copy-Item "$checkoutRoot\Apprenticeships\Configuration\**" "C:\Projects\SFA\Apprenticeships\Configuration" -recurse
}
