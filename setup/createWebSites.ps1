
$file = "C:\Windows\System32\drivers\etc\hosts"

function add-host([string]$filename, [string]$ip, [string]$hostname) {
	remove-host $filename $hostname
	$ip + "`t`t" + $hostname | Out-File -encoding ASCII -append $filename
}

function remove-host([string]$filename, [string]$hostname) {
	$c = Get-Content $filename
	$newLines = @()
	
	foreach ($line in $c) {
		$bits = [regex]::Split($line, "\t+")
		if ($bits.count -eq 2) {
			if ($bits[1] -ne $hostname) {
				$newLines += $line
			}
		} else {
			$newLines += $line
		}
	}
	
	# Write file
	Clear-Content $filename
	foreach ($line in $newLines) {
		$line | Out-File -encoding ASCII -append $filename
	}
}

function print-hosts([string]$filename) {
	$c = Get-Content $filename
	
	foreach ($line in $c) {
		$bits = [regex]::Split($line, "\t+")
		if ($bits.count -eq 2) {
			Write-Host $bits[0] `t`t $bits[1]
		}
	}
}

print-hosts $file
Write-Host "Adding candidate and employer websites"
add-host $file "127.0.0.1" "local.candidates.gov.uk"
add-host $file "127.0.0.1" "local.employers.gov.uk"
Write-Host "Added candidate and employer websites"
print-hosts $file

C:\Windows\System32\inetsrv\appcmd delete site local.candidates.gov.uk
C:\Windows\System32\inetsrv\appcmd delete site local.employers.gov.uk

C:\Windows\System32\inetsrv\appcmd delete apppool SFACandidates
C:\Windows\System32\inetsrv\appcmd delete apppool SFAEmployers

C:\Windows\System32\inetsrv\appcmd add apppool /name:SFACandidates /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
C:\Windows\System32\inetsrv\appcmd add apppool /name:SFAEmployers /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated

C:\Windows\System32\inetsrv\appcmd add site /name:local.candidates.gov.uk /id:100 /physicalPath:C:\Projects\SFA\Beta\src\SFA.Apprenticeships.Web.Candidate\ /bindings:http/*:80:local.candidates.gov.uk
C:\Windows\System32\inetsrv\appcmd add site /name:local.employers.gov.uk /id:200 /physicalPath:C:\Projects\SFA\Beta\src\SFA.Apprenticeships.Web.Employer\ /bindings:http/*:80:local.employers.gov.uk

C:\Windows\System32\inetsrv\appcmd set app "local.candidates.gov.uk/" /applicationPool:SFACandidates
C:\Windows\System32\inetsrv\appcmd set app "local.employers.gov.uk/" /applicationPool:SFAEmployers