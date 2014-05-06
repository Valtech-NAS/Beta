if(!(Test-Path -Path "c:\WebSites" )){
    New-Item -ItemType directory -Path "c:\WebSites"
}

if(!(Test-Path -Path "c:\WebSites\Local" )){
    New-Item -ItemType directory -Path "c:\WebSites\Local"
}

if(!(Test-Path -Path "c:\WebSites\Local\Employers" )){
    New-Item -ItemType directory -Path "c:\WebSites\Local\Employers"
}

if(!(Test-Path -Path "c:\WebSites\Local\Candidates" )){
    New-Item -ItemType directory -Path "c:\WebSites\Local\Candidates"
}

C:\Windows\System32\inetsrv\appcmd delete site local.candidates.gov.uk
C:\Windows\System32\inetsrv\appcmd delete site local.employers.gov.uk

C:\Windows\System32\inetsrv\appcmd delete apppool SFACandidates
C:\Windows\System32\inetsrv\appcmd delete apppool SFAEmployers

C:\Windows\System32\inetsrv\appcmd add apppool /name:SFACandidates /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
C:\Windows\System32\inetsrv\appcmd add apppool /name:SFAEmployers /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated

C:\Windows\System32\inetsrv\appcmd add site /name:local.candidates.gov.uk /id:100 /physicalPath:c:\WebSites\Local\Candidates\ /bindings:http/*:80:local.candidates.gov.uk
C:\Windows\System32\inetsrv\appcmd add site /name:local.employers.gov.uk /id:200 /physicalPath:c:\WebSites\Local\Candidates\ /bindings:http/*:80:local.employers.gov.uk

C:\Windows\System32\inetsrv\appcmd set app "local.candidates.gov.uk/" /applicationPool:SFACandidates
C:\Windows\System32\inetsrv\appcmd set app "local.employers.gov.uk/" /applicationPool:SFAEmployers