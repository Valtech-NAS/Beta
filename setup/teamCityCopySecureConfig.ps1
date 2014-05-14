param(
    [string]$checkoutRoot
)

if(!(Test-Path -Path "c:\Projects" )){
    New-Item -ItemType directory -Path "C:\Projects"
}

if(!(Test-Path -Path "c:\Projects\SFA" )){
    New-Item -ItemType directory -Path "C:\Projects\SFA"
}

if(!(Test-Path -Path "c:\Projects\SFA\Configuration" )){
    New-Item -ItemType directory -Path "C:\Projects\SFA\Configuration"
}

Remove-Item C:\Projects\SFA\Configuration\* -recurse

$secureSourcePath = "$checkoutRoot\Configuration\**"

Write-Host $secureSourcePath

Copy-Item $secureSourcePath "C:\Projects\SFA\Configuration"