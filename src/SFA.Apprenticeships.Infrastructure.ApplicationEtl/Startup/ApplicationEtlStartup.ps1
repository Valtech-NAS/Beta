Write-Host "Starting to create the performance counters."

$categoryName = "SFA.Apprenticeships.WorkerRoles.ApplicationEtl"
$categoryHelp = "Performance counters related to Application Etl Worker Role"
$categoryType = [System.Diagnostics.PerformanceCounterCategoryType]::SingleInstance

Function CreatePerformanceCounterCategory($applicationEtlCount){
    $objCCDC = New-Object System.Diagnostics.CounterCreationDataCollection
  
    $objCCD1 = CreatePerformanceCounter "ApplicationEtlExecutions" "Number of ApplicationEtl executions"
    $objCCDC.Add($objCCD1) | Out-Null
  
    [System.Diagnostics.PerformanceCounterCategory]::Create($categoryName, $categoryHelp, $categoryType, $objCCDC) | Out-Null

    CreatePerformanceCounterInstance "ApplicationEtlExecutions" $applicationEtlCount
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

  CreatePerformanceCounterCategory 0
}
Else
{
	Write-Host "The category already exists. Let's recreate it."
    $applicationEtlRunsCount = GetCounterValue "ApplicationEtlExecutions"
    
    [Diagnostics.PerformanceCounterCategory]::Delete($categoryName)    
    CreatePerformanceCounterCategory $applicationEtlRunsCount
}