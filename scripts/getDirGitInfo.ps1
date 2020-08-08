#
# This scripts assumes following directory structure
# at least one subdirectory
# TopStarred/Trending
#    - dir1
#    - dir2
#

[CmdletBinding(PositionalBinding=$true)]
Param([string] $categoryDir)
#function assumes it is called in git repo dir such that `git log` will work properly
function Get-LatestCommitHash(){
    
    $outGitLog = git log
    #Write-Host $outGitLog.GetType()
    # assume git commit hash in first line of git log output
    #Write-Host $outGitLog[0]
    # $outGitLog == 'commit hash' 
    $hash = $outGitLog[0].Split()[1]
    return $hash

}

function Get-SlnFiles([string]$fullPathToDir){
    #another way of writing it
    # if return only one .sln file then returning type is FileInfo ortherwise return type is object object[]
    $slnFiles = Get-ChildItem $fullPathToDir -recurse | where {$_.Extension -eq ".sln"} 
    
    #Write-Host $slnFiles.GetType()

    # foreach($sln in $slnFiles){
    #     #Get solution content useful for checking which year(format solution is).
    #     #$slnContent = get-content  $sln.FullName -Delimiter "\r\n"
    #     #Write-Host $slnContent
    #     #exit 0
    #     #write-host $sln.FullName
    # }
    #Write-Host $("Total solutions: "+$slnFiles.Count.ToString())
    
    return $slnFiles
}


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

        $slnFiles = Get-SlnFiles($fullPathToRepo)
        If ( $slnFiles -is [array] ){
            
            foreach($sln in $slnFiles){
                #unlike write-host, the output from write-output can be redirected using ">" operators at the command line 
                Write-Output $($repo.Name+", "+$sln.FullName+", "+$fullPathToRepo+", "+ $hash)
    
            }
        }
        else {
            
            write-output $($repo.Name+", "+$slnFiles.FullName +", "+$fullPathToRepo+", "+ $hash)
        }

        pop-location
}


