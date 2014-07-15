<#
.SYNOPSIS 
    You can use this script if you are modifying the TemplateBuilder targets
    file ligershark.templates.targets and want to try out changes when 
    building SideWaffle. To use this script you will need to do the following:
        1. Create an alias to msbulid.exe
        2. Define the environment variable TemplateBuilderDevRoot which should
           point to the root template-builder folder, this should end with a \
           as well. For example 'C:\Data\personal\mycode\template-builder\'.

.PARAMETER extraBuildArgs
    You can now pass in additional arguments for msbuild.exe
    Let's say you want to execute a specific target and don't want to hack this file
    you can invoke it in the following way
                .\build-debug.ps1 -extraBuildArgs '/t:Demo'

.PARAMETER preventOverridingTargetsPath
    If this is set the build will still be invoked but the path to the .targets & .tasks file will not be modified. This is useful if you just want to build with the sources in place and use the Debug build configuration
#>
[cmdletbinding(DefaultParameterSetName ='build')]
param(
    [Parameter(ParameterSetName='build')]
    $configuration = 'Release',

    [Parameter(ParameterSetName='build')]
    $extraBuildArgs,

    [Parameter(ParameterSetName='build')]
    $restoreNugetPackages = $true,

    [Parameter(ParameterSetName='build')]
    [switch]$preventOverridingTargetsPath,

    [Parameter(ParameterSetName='build')]
    [switch]$UseLocalTemplateBuilderSrc,

    [Parameter(ParameterSetName='build')]
    [switch]$UseLocalSlowCheetahXdtSrc,

    [Parameter(ParameterSetName='build')]
    [switch]$UpdateNugetExe,

    [Parameter(ParameterSetName='optimizeImages')]
    [switch]$optimizeImages
    )

$global:SideWaffleBuildSettings = New-Object PSObject -Property @{
    TempFolder = ("$env:LOCALAPPDATA\SideWaffle\BuildOutput\")
    MaxNumThreads = ((Get-WmiObject Win32_Processor).NumberOfCores)
    VisualStudioVersion = '12.0'
    ToolsDirectory = '.\.tools'
}

function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$script:scriptDir = ((Get-ScriptDirectory) + "\")
$script:slnFilePath = ('{0}SideWaffle.sln' -f $scriptDir)

# There are a few things which this script requires
#     msbuild alias
#     $global:codeHome

<#
.SYNOPSIS 
    This will throw an error if the psbuild module is not installed and available.
#>
function EnsurePsbuildInstalled(){
    [cmdletbinding()]
    param()
    process{

        if(!(Get-Module -listAvailable 'psbuild')){
            $msg = ('psbuild is required for this script, but it does not look to be installed. Get psbuild from here: https://github.com/ligershark/psbuild')
            throw $msg
        }

        if(!(Get-Module 'psbuild')){
            # add psbuild to the currently loaded session modules
            import-module psbuild -Global;
        }
    }
}

<#
.SYNOPSIS
    If the image optimizer is not installed in the .tools\
    folder then it will be downloaded there.
#>
function GetImageOptimizer(){
    [cmdletbinding()]
    param(
        $toolsDir = (Join-Path -Path $script:scriptDir -ChildPath $global:SideWaffleBuildSettings.ToolsDirectory)
    )
    process{
        
        if(!(Test-Path $toolsDir)){
            New-Item $toolsDir -ItemType Directory | Out-Null
        }

        $imgOptimizer = (Get-ChildItem -Path $toolsDir -Include 'ImageCompressor.Job.exe' -Recurse)

        if(!$imgOptimizer){
            'Downloading image optimizer to the .tools folder' | Write-Verbose
            # nuget install AzureImageOptimizer -Prerelease -OutputDirectory C:\temp\nuget\out\
            $cmdArgs = @('install','AzureImageOptimizer','-Prerelease','-OutputDirectory',(Resolve-Path $toolsDir).ToString())

            'Calling nuget to install image optimzer with the following args. [{0}]' -f ($cmdArgs -join ' ') | Write-Verbose
            &(GetNuGet) $cmdArgs | Out-Null
        }

        $imgOptimizer = Get-ChildItem -Path $toolsDir -Include 'ImageCompressor.Job.exe' -Recurse | select -first 1
        # imgOptimizer is now a single fileinfo reference, or $null

        if(!$imgOptimizer){ throw 'Image optimizer not found' }
        $imgOptimizer
    }
}

function OptimizeImages(){
    [cmdletbinding()]
    param(
        $folder = (Join-Path $script:scriptDir 'TemplatePack')
    )
    process{        
        [string]$imgOptExe = GetImageOptimizer

        [string]$folderToOptimize = (Resolve-path $folder)

        'Starting image optimizer on folder [{0}]' -f $foldersToDelete | Write-Host
        # .\.tools\AzureImageOptimizer.0.0.10-beta\tools\ImageCompressor.Job.exe --folder M:\temp\images\opt\to-optimize
        $cmdArgs = @('--folder', $folderToOptimize)

        'Calling img optimizer with the following args [{0}]' -f ($cmdArgs -join ' ') | Write-Host
        &$imgOptExe $cmdArgs

        'Images optimized' | Write-Host
    }
}


<#
.SYNOPSIS
    If nuget is not in the tools
    folder then it will be downloaded there.
#>
function GetNuget(){
    [cmdletbinding()]
    param(
        $toolsDir = ("$env:LOCALAPPDATA\LigerShark\AzureJobs\tools\"),

        $nugetDownloadUrl = 'http://nuget.org/nuget.exe'
    )
    process{
        $nugetDestPath = Join-Path -Path $toolsDir -ChildPath nuget.exe

        if(!(Test-Path $toolsDir)){
            New-Item -Path $toolsDir -ItemType Directory
        }
        
        if(!(Test-Path $nugetDestPath)){
            'Downloading nuget.exe' | Write-Verbose
            # download nuget
            $webclient = New-Object System.Net.WebClient
            $webclient.DownloadFile($nugetDownloadUrl, $nugetDestPath)

            # double check that is was written to disk
            if(!(Test-Path $nugetDestPath)){
                throw 'unable to download nuget'
            }
        }

        # return the path of the file
        $nugetDestPath
    }
}

function UpdateNuGetExe(){
    [cmdletbinding()]
    param(
        $nugetExePath = (GetNuget)
    )
    process{
        $cmdArgs = @('update','-self')

        'Updating nuget.exe. Calling nuget.exe with the args: [{0}]' -f ($cmdArgs -join ' ') | Write-Verbose
        & $nugetExePath $cmdArgs
    }
}

function RestoreNugetPackages(){
    [cmdletbinding()]
    param(
        $nugetExePath = (GetNuget)
    )
    process{
        $cmdArgs = @('restore', (Resolve-Path $script:slnFilePath).ToString())

        'Restoring nuget packages. Calling nuget.exe with the args: [{0}]' -f ($cmdArgs -join ' ') | Write-Verbose

        & $nugetExePath $cmdArgs
    }
}

function Build-TemplateBuilder(){
    [cmdletbinding()]
    param(
        [Parameter(Mandatory=$true)]
        $tbSourceRoot,

        $configuration = 'Release'
    )
    process{
        'Building templatebuilder' | Write-Host
        $tbSlnFile = (Join-Path $tbSourceRoot -ChildPath 'src\LigerShark.TemplateBuilder.sln')

        Invoke-MSBuild $tbSlnFile -configuration $configuration
    }
}

function Build-SlowCheetahXdt(){
    [cmdletbinding()]
    param(
        [Parameter(Mandatory=$true)]
        $scSourceRoot,

        $configuration = 'Release'
    )
    process{
        'Building SlowCheetah.Xdt' | Write-Host
        $scSlnFile = (Join-Path $scSourceRoot -ChildPath 'SlowCheetah.Xdt\SlowCheetah.Xdt.sln')

        Invoke-MSBuild $scSlnFile -configuration $configuration
    }
}

###########################################################
# Begin script
###########################################################


'Begin started. This script uses psbuild which is available at https://github.com/ligershark/psbuild' | Write-Host

EnsurePsbuildInstalled

if($optimizeImages){
    # delete the bin & obj folder before starting
    $foldersToDelete = @( (Join-Path $script:scriptDir 'TemplatePack\bin\'), (Join-Path $script:scriptDir 'TemplatePack\obj'))

    foreach($folder in $foldersToDelete){
        if(Test-Path $folder){
            'Deleting build folder: [{0}]' -f $folder | Write-Verbose
            Remove-Item $folder -Recurse
        }
    }

    OptimizeImages
}
else {

    if($restoreNugetPackages){
        if($UpdateNugetExe){
            UpdateNuGetExe
        }

        RestoreNugetPackages
    }

    if(-not $preventOverridingMsbuildPath){
        Set-MSBuild "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
    }

    # msbuild ".\TemplatePack\TemplatePack.csproj" 
    #    /p:VisualStudioVersion=11.0 
    #    /p:TemplateBuilderTargets="($env:TemplateBuilderDevRoot)tools\ligershark.templates.targets" 
    #    /p:ls-TasksRoot="($env:TemplateBuilderDevRoot)\src\LigerShark.TemplateBuilder.Tasks\bin\Debug\" /fl

    # /flp1:v=d;logfile=build.d.log /flp2:v=diag;logfile=build.diag.log

    $projToBuild = ('{0}TemplatePack\TemplatePack.csproj' -f $scriptDir)
    
    $buildProperties = @{}
    $extraArgs = @()   

    if($UseLocalTemplateBuilderSrc){
        $expTempBuilderSrcPath = $env:TemplateBuilderDevRoot

        if(!($expTempBuilderSrcPath)){
            $expTempBuilderSrcPath = ('{0}template-builder\' -f $global:codeHome)
        }

        if(!(Test-Path $expTempBuilderSrcPath)){
            $msg = ('template builder folder not found at [{0}]' -f $expTempBuilderSrcPath)
            throw $msg
        }

        $templateBuilderTargetsPath = ("{0}tools\ligershark.templates.targets" -f $expTempBuilderSrcPath)
        $templateTaskRoot = ("{0}src\LigerShark.TemplateBuilder.Tasks\bin\Debug\" -f $expTempBuilderSrcPath)

        Build-TemplateBuilder -tbSourceRoot $expTempBuilderSrcPath -configuration $configuration
        $buildProperties += @{'TemplateBuilderTargets'=$templateBuilderTargetsPath}
        $buildProperties += @{'ls-TasksRoot'=$templateTaskRoot}
    }

    if($UseLocalSlowCheetahXdtSrc){
        $expScXdtPath = $env:SlowCheetahXdtDevRoot

        if(!($expScXdtPath)){
            $expScXdtPath = ('{0}slow-cheetah.xdt\' -f $global:codeHome)
        }

        if(!(Test-Path $expScXdtPath)){
            $msg = ('SlowCheetah.Xdt folder not found at [{0}]' -f $expScXdtPath)
            throw $msg
        }

        Build-SlowCheetahXdt -scSourceRoot $expScXdtPath -configuration $configuration
        $slowcheetahxdtTaskRoot = ('{0}SlowCheetah.Xdt\bin\Debug\' -f $expScXdtPath)
        $buildProperties += @{'ls-SlowCheetahXdtTaskRoot'=$slowcheetahxdtTaskRoot}
    }

    $extraArgs += '/clp:v=m;ShowCommandLine'

    if($extraBuildArgs){
        $extraArgs += $extraBuildArgs
    }

    Invoke-MSBuild  -projectsToBuild $projToBuild `
                    -configuration $configuration `
                    -visualStudioVersion $global:SideWaffleBuildSettings.VisualStudioVersion `
                    -nologo `
                    -properties $buildProperties `
}
