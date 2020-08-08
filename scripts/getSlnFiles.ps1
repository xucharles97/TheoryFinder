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
# if return only one .sln file then returning type is FileInfo ortherwise return type is object object[]
$slnFiles = Get-ChildItem $fullPath -recurse | where {$_.Extension -eq ".sln"} 
Write-Host $slnFiles.GetType()

foreach($sln in $slnFiles){
    #Get solution content useful for checking which year(format solution is).
    #$slnContent = get-content  $sln.FullName -Delimiter "\r\n"
    #Write-Host $slnContent
    #exit 0
    Write-Host $sln.FullName
    C:\Users\Synthesis\Research\AnnaCharles\tools\TheoryFinder\TheoryFinder\bin\Debug\net472\TheoryFinder.exe $sln.FullName
}

Write-Host $("Total solutions: "+$slnFiles.Count.ToString())

