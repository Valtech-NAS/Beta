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

Remove-Item C:\Projects\SFA\Apprenticeships\Configuration\* -recurse

Write-Host "$checkoutRoot\Apprenticeships\Configuration\**"

Copy-Item "$checkoutRoot\Configuration\**" "C:\Projects\SFA\Apprenticeships\Configuration" -recurse