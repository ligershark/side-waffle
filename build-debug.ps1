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
    [switch]$preventOverridingMsbuildPath,

    [Parameter(ParameterSetName='build')]
    [switch]$UseLocalTemplateBuilderSrc,

    [Parameter(ParameterSetName='build')]
    [switch]$UseLocalSlowCheetahXdtSrc,

    [Parameter(ParameterSetName='optimizeImages')]
    [switch]$optimizeImages
    )

$global:SideWaffleBuildSettings = New-Object PSObject -Property @{
    TempFolder = ("$env:LOCALAPPDATA\SideWaffle\BuildOutput\")
    MaxNumThreads = ((Get-WmiObject Win32_Processor).NumberOfCores)
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

function Check-PathVariable(){
    [cmdletbinding()]
    param(
        [Parameter(Mandatory=$true)]
        $name,
        
        #[Parameter(Mandatory=$true)]
        $envValue,
        
        [switch]
        $checkEndsWithSlash
    )
    process{

        $private:succeeded = $true

        if(!$envValue){
            "Missing required environment variable {0}. Please define it and try again" -f $name | Write-Error
            $private:succeeded = $false
        }
        elseif(!(Test-Path $envValue)){
            "{0} not found at [{1}]" -f $name, $envValue | Write-Error
            $private:succeeded = $false
        }
        elseif($checkEndsWithSlash){
            # check to see if $envValue does not end with a '\' and warn the user if not
            if(!(($envValue).EndsWith('\'))){
                '$env:{0} does not end with a \ which is expected. Things may not go as planned.' -f $name | Write-Warning
            }
        }

        # return true / false
        $private:succeeded
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

if($optimizeImages){
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
        UpdateNuGetExe
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

    $msbuildArgs = @()
    $msbuildArgs += ('{0}TemplatePack\TemplatePack.csproj' -f $scriptDir)
    $msbuildArgs += '/m'
    $msbuildArgs += '/nologo'
    # https://github.com/ligershark/side-waffle/issues/108
    #     Cmd line build not working for Debug mode for some reason
    $msbuildArgs += ("/p:Configuration=Debug")
    $msbuildArgs += ("/p:VisualStudioVersion=12.0")

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

        $msbuildArgs += ("/p:TemplateBuilderTargets={0}" -f $templateBuilderTargetsPath)
        $msbuildArgs += ("/p:ls-TasksRoot={0}" -f $templateTaskRoot)                
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

        $slowcheetahxdtTaskRoot = ('{0}SlowCheetah.Xdt\bin\Debug\' -f $expScXdtPath)
        $msbuildArgs += ('/p:ls-SlowCheetahXdtTaskRoot={0}' -f $slowcheetahxdtTaskRoot)
    }

    $msbuildArgs += '/flp1:v=d;logfile=msbuild.d.log'
    $msbuildArgs += '/flp2:v=diag;logfile=msbuild.diag.log'
    $msbuildArgs += ('/clp:v=m;ShowCommandLine')
    if($extraBuildArgs){
        $msbuildArgs += $extraBuildArgs
    }

    "Calling msbuild.exe with the following args: {0}" -f ($msbuildArgs -join ' ') | Write-Host
    & msbuild $msbuildArgs

    "`r`n  MSBuild log files have been written to" | Write-Host -ForegroundColor Green
    "    msbuild.d.log" | Write-Host -ForegroundColor Green
    "    msbuild.diag.log" | Write-Host -ForegroundColor Green
    
}