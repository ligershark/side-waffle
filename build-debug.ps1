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
    Configuration = 'Debug'
    VisualStudioVersion = '12.0'
    ToolsDirectory = '.\.tools'
}

function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$script:scriptDir = ((Get-ScriptDirectory) + "\")
$script:toolsRoot = (Join-Path -Path (Get-ScriptDirectory) -ChildPath Tools\)
$script:nugetExePath = ('{0}nuget.exe' -f $script:toolsRoot)
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
function EnsureImageOptimizerAvailable(){
    [cmdletbinding()]
    param()
    process{
        $toolsDir = (Join-Path -Path $script:scriptDir -ChildPath $global:SideWaffleBuildSettings.ToolsDirectory)
        if(!(Test-Path $toolsDir)){
            New-Item $toolsDir -ItemType Directory
        }

        $imgOptimizer = (Get-ChildItem -Path $toolsDir -Include 'ImageCompressor.Job.exe' -Recurse)

        if(!$imgOptimizer){
            'Downloading image optimizer to the .tools folder' | Write-Output
            # nuget install AzureImageOptimizer -Prerelease -OutputDirectory C:\temp\nuget\out\
            $cmdArgs = @()
            $cmdArgs += 'install'
            $cmdArgs += 'AzureImageOptimizer'
            $cmdArgs += '-Prerelease'
            $cmdArgs += '-OutputDirectory'
            $cmdArgs += (Resolve-Path $toolsDir).ToString()

            'Calling nuget to install image optimzer with the following args. [{0}]' -f ($cmdArgs -join ' ') | Write-Verbose
            &$script:nugetExePath $cmdArgs
        }
        else{
            'Image optimizer found'
        }
    }
}

function OptimizeImages(){
    [cmdletbinding()]
    param(
         $root = (Join-Path -Path $script:scrDir -ChildPath TemplatePack\),

        [switch]
        $Recurse
    )
    process{
        'Optimizing images in directory [{0}]' -f $root | Write-Verbose

        if($Recurse){
            $pngsToOptimize = (Get-ChildItem $root *.png -Recurse)
        }
        else{
            $pngsToOptimize = (Get-ChildItem $root *.png)
        }

        
        if($pngsToOptimize){
            $pngsToOptimize = ($pngsToOptimize |
                Where-Object { !$_.FullName.Contains('\obj\')} | 
                Where-Object { !$_.FullName.Contains('\bin\')})
        }
        
        $beforeOptimizing = ($pngsToOptimize.FullName | Get-Item | Select-Object FullName, Length)

        'Number of .pngs to optimize: [{0}]' -f $pngsToOptimize.Length | Write-Verbose

        $pngsToOptimize.FullName | OptimizePng
        $afterOptimizing = ($pngsToOptimize.FullName | Get-Item | Select-Object FullName, Length)
        
        $delta = @()
        foreach($item in $beforeOptimizing){
            $beforeItem = $item
            $afterItem = ($afterOptimizing | Where-Object {$_.FullName -eq $item.FullName})

            $deltaObj = New-Object PSObject -Property @{
                    FullName = $beforeItem.FullName
                    LengthBefore = $beforeItem.Length
                    LengthAfter = [int]::MinValue
                    LengthDiff = [int]::MinValue
                }
            
            if(!$afterItem){
                'Unable to find matching file in after collection for file [{0}]' -f $_.FullName | Write-Error
            }
            else{
                $deltaObj.LengthAfter = $afterItem.Length
                $deltaObj.LengthDiff = ($beforeItem.Length - $afterItem.Length)
            }

            $delta += $deltaObj
        }

        $imgReportPath = (Join-Path -Path $global:SideWaffleBuildSettings.TempFolder -ChildPath 'imagereport.csv')
        if(!(Test-Path $global:SideWaffleBuildSettings.TempFolder)){
            mkdir $global:SideWaffleBuildSettings.TempFolder
        }
        $delta | Select-Object FullName, LengthBefore, LengthAfter,LengthDiff | Export-Csv -Path $imgReportPath -NoTypeInformation
        'Saved image report to [{0}]' -f $imgReportPath | Write-Verbose

        return $delta
    }
}

function OptimizePng(){
    [cmdletbinding()]
    param(
        [Parameter(
            Mandatory=$true,
            ValueFromPipeline=$true)]
        $image
        )
    begin{
        $pngout = Join-Path $script:toolsRoot pngout.exe
        $pngoptim = Join-Path $script:toolsRoot PNGOptim.1.2.2.exe
    }
    process{
        foreach($img in $image){
            $fullPath = Resolve-Path $img
            $pngoutArgs = @()
            $pngoutArgs += $fullPath
            $pngoutArgs += '/q'
            
            'Calling pngout [{0} {1}]' -f $pngout, ($pngoutArgs -join ' ') | Write-Verbose
            &$pngout $pngoutArgs
        }
    }
}

function UpdateNuGetExe(){
    [cmdletbinding()]
    param()
    process{
        $cmdArgs = @()
        $cmdArgs += 'update'
        $cmdArgs += '-self'

        'Updating nuget.exe. Calling nuget.exe with the args: [{0}]' -f ($cmdArgs -join ' ') | Write-Verbose
        & $script:nugetExePath $cmdArgs
    }
}

function RestoreNugetPackages(){
    [cmdletbinding()]
    param()
    process{
        $cmdArgs = @()
        $cmdArgs += 'restore'
        $cmdArgs += (Resolve-Path $script:slnFilePath).ToString()

        'Restoring nuget packages. Calling nuget.exe with the args: [{0}]' -f ($cmdArgs -join ' ') | Write-Verbose

        & $nugetExePath $cmdArgs
    }
}

function Build-TemplateBuilder(){
    [cmdletbinding()]
    param(
        [Parameter(Mandatory=$true)]
        $tbSourceRoot
    )
    process{
        'Building templatebuilder' | Write-Output
        $tbSlnFile = (Join-Path $tbSourceRoot -ChildPath 'src\LigerShark.TemplateBuilder.sln')

        Invoke-MSBuild $tbSlnFile -configuration 'Debug'
    }
}

function Build-SlowCheetahXdt(){
    [cmdletbinding()]
    param(
        [Parameter(Mandatory=$true)]
        $scSourceRoot
    )
    process{
        'Building SlowCheetah.Xdt' | Write-Output
        $scSlnFile = (Join-Path $scSourceRoot -ChildPath 'SlowCheetah.Xdt\SlowCheetah.Xdt.sln')

        Invoke-MSBuild $scSlnFile -configuration 'Debug'
    }
}

###########################################################
# Begin script
###########################################################


'Begin started. This script uses psbuild which is available at https://github.com/ligershark/psbuild' | Write-Output

EnsurePsbuildInstalled

if($optimizeImages){
    # EnsureImageOptimizerAvailable
    'optimizing images' | Write-Verbose

    $imgResult = OptimizeImages -root $script:scrDir -Recurse
    
    $imgResult | 
        Select-Object @{Name='Name';Expression={(get-item $_.FullName).Name}},LengthBefore,LengthAfter,LengthDiff | 
        Sort-Object LengthDiff -Descending |
        Write-Output

    'Total diff: {0} KB' -f (($imgResult | Measure-Object -Property LengthDiff -Sum | Select-Object Sum).Sum/1KB) | Write-Output
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

        Build-TemplateBuilder -tbSourceRoot $expTempBuilderSrcPath
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

        Build-SlowCheetahXdt -scSourceRoot $expScXdtPath
        $slowcheetahxdtTaskRoot = ('{0}SlowCheetah.Xdt\bin\Debug\' -f $expScXdtPath)
        $buildProperties += @{'ls-SlowCheetahXdtTaskRoot'=$slowcheetahxdtTaskRoot}
    }

    $extraArgs += '/clp:v=m;ShowCommandLine'

    if($extraBuildArgs){
        $extraArgs += $extraBuildArgs
    }

    Invoke-MSBuild  -projectsToBuild $projToBuild `
                    -configuration $global:SideWaffleBuildSettings.Configuration `
                    -visualStudioVersion $global:SideWaffleBuildSettings.VisualStudioVersion `
                    -nologo `
                    -properties $buildProperties `
}