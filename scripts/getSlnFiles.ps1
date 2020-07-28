[CmdletBinding(PositionalBinding=$true)]
Param([string] $rootDir)

Write-Host $rootDir

$fullPath = Resolve-Path -Path $rootDir
# % is alias for ForEach-Object

# One way of writtig it:

#Get-ChildItem $fullPath -recurse | where {$_.Extension -eq ".sln"}|%{
#    Write-Host $_.FullName
#}

#another way of writing it
$slnFiles = Get-ChildItem $fullPath -recurse | where {$_.Extension -eq ".sln"}

foreach($sln in $slnFiles){
    Write-Host $sln.FullName
}
