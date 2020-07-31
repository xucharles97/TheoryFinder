[CmdletBinding(PositionalBinding=$true)]
Param([string] $rootDir)

 

$fullPath = Resolve-Path -Path $rootDir
Write-Host $("Debug "+$fullPath)
# % is alias for ForEach-Object

# One way of writig it:
#Get-ChildItem $fullPath -recurse | where {$_.Extension -eq ".sln"}|%{
#    Write-Host $_.FullName
#}

#another way of writing it
$slnFiles = Get-ChildItem $fullPath -recurse | where {$_.Extension -eq ".sln"}

foreach($sln in $slnFiles){
    #Get solution content useful for checking which year(format solution is).
    #$slnContent = get-content  $sln.FullName -Delimiter "\r\n"
    #Write-Host $slnContent
    #exit 0
    Write-Host $sln.FullName
}

Write-Host $("Total solutions: "+$slnFiles.Length.ToString())

