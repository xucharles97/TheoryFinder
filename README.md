# TheoryFinder

Program developed to extract PUT and Unit Test information from C# .sln files using the Roslyn Library. PUTs and Unit Tests are identified through various attributes listed in the code, like `[Theory]` for PUTs and `[Fact]` for unit tests. 

This program is capable of outputting various information about the tests in a .sln file, including:
* The amount of PUTs and Unit Tests
* The total lines of code within the PUTs and Unit Tests 
* The total amount of Asserts in the PUTs and Unit Tests

To get the hash and path of a git repo, run the `getDirGitInfo.ps1` file in the scripts directory.

# Running TheoryFinder

Use the .ps1 files in the ./scripts folder to run TheoryFinder. From the ./scripts folder, you can run either of the scripts as follows:

In general, you will need to format the command line commands as follows: 

```<script name> <path(full or relative) to root directory of project>```

1. To run the TheoryFinder program and get information on the PUTs and Unit Tests, update the filepath of the TheoryFinder executable in `getSlnFiles.ps1` to match the filepath of the executable on the local machine. Then run the above command on PowerShell, with `.\getSlnFiles.ps1` as the script name.

For example: 

```.\getSlnFiles.ps1 ..\..\..\subjects\trending\azure-powershell\```

2. To get the git hash information of a particular repo, run the `.\getDirGitInfo.ps1`script. You can also use `>` to redirect the output of the script. For example:
 
 ```.\getDirGitInfo.ps1 ..\..\..\subjects\topStarred > topStarred.csv```

