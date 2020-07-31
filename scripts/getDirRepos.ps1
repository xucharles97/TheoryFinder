

[CmdletBinding(PositionalBinding=$true)]
Param([string] $categoryDir)
#function assumes it is called in repo dir such that `git log` will work properly
function Get-LatestCommitHash(){
    
    $outGitLog = git log
    #Write-Host $outGitLog.GetType()
    # assume git commit hash in first line of git log output
    #Write-Host $outGitLog[0]
    # $outGitLog == 'commit hash' 
    $hash = $outGitLog[0].Split()[1]
    return $hash

}
#
# This scripts assumes following directory structure
# at least one subdirectory
# TopStarred/Trending
#    - dir1
#    - dir2
#

$fullPath = Resolve-Path -Path $categoryDir

#Write-Host $fullPath

# Get directories only- return type array of object(i.e., object[]) or DirectoryInfo when only one subdirectory 
$repos = Get-ChildItem -Directory $fullPath

foreach($repo in $repos){
    #Get solution content useful for checking which year(format solution is).
    #$slnContent = get-content  $sln.FullName -Delimiter "\r\n"
    #Write-Host $slnContent
    #exit 0
    $fullPathToRepo = $repo.FullName
    #write-host = $fullPathToRepo
    push-location $fullPathToRepo
    #commit has string type
    $hash = Get-LatestCommitHash
    write-host $($repo.Name+","+ $fullPathToRepo+","+ $hash)
    
    pop-location
}


