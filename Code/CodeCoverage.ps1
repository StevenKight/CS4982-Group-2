# PURPOSE: Automates the running of Unit Tests and Code Coverage
# REF: https://stackoverflow.com/a/70321555/495455

# If running outside the test folder
#cd E:\Dev\XYZ\src\XYZTestProject

# This only needs to be installed once (globally), if installed it fails silently: 
dotnet tool install -g dotnet-reportgenerator-globaltool

# Delete previous test run results (there's a bunch of subfolders named with guids)
Remove-Item -Recurse -Force $PSScriptRoot/TestResults/

# Run the Coverlet.Collector - REPLACING YOUR SOLUTION NAME!!!
$output = [string] (& dotnet test $PSScriptRoot/CapstoneGroup2/CapstoneGroup2.sln --collect:"XPlat Code Coverage" 2>&1)
Write-Host "Last Exit Code: $lastexitcode"
Write-Host $output

# Delete previous test run reports - note if you're getting wrong results do a Solution Clean and Rebuild to remove stale DLLs in the bin folder
Remove-Item -Recurse -Force $PSScriptRoot/coveragereport/

# To keep a history of the Code Coverage we need to use the argument: -historydir:SOME_DIRECTORY 
if (!(Test-Path -path $PSScriptRoot/CoverageHistory)) {  
    New-Item -ItemType directory -Path $PSScriptRoot/CoverageHistory
}

# Generate the Code Coverage HTML Report
reportgenerator -reports:"$PSScriptRoot/**/coverage.cobertura.xml" -targetdir:"$PSScriptRoot/CoverageReport" -reporttypes:Html -historydir:$PSScriptRoot/CoverageHistory 

# Open the Code Coverage HTML Report (if running on a WorkStation)
$osInfo = Get-CimInstance -ClassName Win32_OperatingSystem
if ($osInfo.ProductType -eq 1) {
    (& "$PSScriptRoot/CoverageReport/index.html")
}