Write-Host "Starting to create the performance counters."

$categoryName = "SFA.Apprenticeships.Web.Candidate"
$categoryHelp = "Performance counters related to SFA.Apprenticeships.Web.Candidate"
$categoryType = [System.Diagnostics.PerformanceCounterCategoryType]::SingleInstance

Function CreatePerformanceCounterCategory($candidateRegistrationCount, $applicationSubmissionCount, $vacancySearchCount){
    $objCCDC = New-Object System.Diagnostics.CounterCreationDataCollection
  
    $objCCD1 = CreatePerformanceCounter "CandidateRegistration" "Number of candidate registrations"
    $objCCDC.Add($objCCD1) | Out-Null
  
    $objCCD2 = CreatePerformanceCounter "ApplicationSubmission" "Number of applications submitted"
    $objCCDC.Add($objCCD2) | Out-Null

	$objCCD2 = CreatePerformanceCounter "VacancySearch" "Number of searches performed"
    $objCCDC.Add($objCCD2) | Out-Null
	
  
    [System.Diagnostics.PerformanceCounterCategory]::Create($categoryName, $categoryHelp, $categoryType, $objCCDC) | Out-Null

    CreatePerformanceCounterInstance "CandidateRegistration" $candidateRegistrationCount
    CreatePerformanceCounterInstance "ApplicationSubmission" $applicationSubmissionCount
	CreatePerformanceCounterInstance "VacancySearch" $vacancySearchCount
}

Function CreatePerformanceCounter($counterName, $counterHelp)
{
    $objCCD1 = New-Object System.Diagnostics.CounterCreationData
    $objCCD1.CounterName = $counterName
    $objCCD1.CounterType = "NumberOfItems64"
    $objCCD1.CounterHelp = $counterHelp
    
    $objCCD1
}

Function CreatePerformanceCounterInstance($counterName, $rawValue)
{
    $counterInstance = New-Object System.Diagnostics.PerformanceCounter($categoryName, $counterName, $false)
    $counterInstance.RawValue = $rawValue
}

Function GetCounterValue($counterName)
{
    $counterValue = 0
    $counterName = ("\"+$categoryName+"\"+$counterName)
    $counter = Get-Counter $counterName
    if ( $counter -eq $null )
    {
        $counterValue = 0
    }
    else
    {
        $counterValue = $counter.counterSamples[0].CookedValue
    }

    $counterValue
}

$categoryExists = [System.Diagnostics.PerformanceCounterCategory]::Exists($categoryName)

If (-Not $categoryExists)
{
  Write-Host "The category doesn't exist. Let's create it."

  CreatePerformanceCounterCategory 0 0
}
Else
{
	Write-Host "The category already exists. Let's recreate it."
    $candidateRegistrationCount = GetCounterValue "CandidateRegistration"
    $applicationSubmissionCount = GetCounterValue "ApplicationSubmission"
	$vacancySearchCount = GetCounterValue "VacancySearch"        
    
    [Diagnostics.PerformanceCounterCategory]::Delete($categoryName)    
    CreatePerformanceCounterCategory $candidateRegistrationCount $applicationSubmissionCount $vacancySearchCount
}