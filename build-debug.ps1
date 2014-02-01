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
param(
    $extraBuildArgs,   

    [switch]$preventOverridingTargetsPath
    )
function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}


# There are a few things which this script requires
#     msbuild alias
#     TemplateBuilderDevRoot : Environment variable

# When called it will return true if all dependencies are found, and false if not
function CheckForDependencies{
    $depFound = $true
    # check for the msbuild alias othewise set it to use
    
    if(!(Get-Alias msbuild)){
        $msbuildDefaultPath = ("{0}\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" -f $env:windir);

        # check to see if it is in the default location
        if(Test-Path $msbuildDefaultPath){
            Set-Alias msbuild $msbuildDefaultPath -Scope Global
        }
        else{
            "Unable to locate msbuild.exe. Set the alias 'msbuild' and try again" | Write-Error
            $depFound = $false
        }
    }

    if(!$env:TemplateBuilderDevRoot){
        "Missing required environment variable TemplateBuilderDevRoot. Please define it and try again" | Write-Error
        $depFound = $false
    }
    elseif(!(Test-Path $env:TemplateBuilderDevRoot)){
        "TemplateBuilderDevRoot not found at [{0}]" -f $env:TemplateBuilderDevRoot | Write-Error
        $depFound = $false
    }
    else{
        # check to see if $env:TemplateBuilderDevRoot does not end with a '\' and warn the user if not
        if(!(($env:TemplateBuilderDevRoot).EndsWith('\'))){
            '$env:TemplateBuilderDevRoot does not end with a \ which is expected. Things may not go as planned.' | Write-Warning
        }
    }

    return $depFound
}

$scriptDir = ((Get-ScriptDirectory) + "\")

$templateBuilderTargetsPath = ("{0}tools\ligershark.templates.targets" -f $env:TemplateBuilderDevRoot)
$templateTaskRoot = ("{0}src\LigerShark.TemplateBuilder.Tasks\bin\Debug\" -f $env:TemplateBuilderDevRoot)

if(CheckForDependencies){
    "Dep check passed" | Write-Host
    # msbuild ".\TemplatePack\TemplatePack.csproj" 
    #    /p:VisualStudioVersion=11.0 
    #    /p:TemplateBuilderTargets="($env:TemplateBuilderDevRoot)tools\ligershark.templates.targets" 
    #    /p:ls-TasksRoot="($env:TemplateBuilderDevRoot)\src\LigerShark.TemplateBuilder.Tasks\bin\Debug\" /fl

    # /flp1:v=d;logfile=build.d.log /flp2:v=diag;logfile=build.diag.log

    $msbuildArgs = @()
    $msbuildArgs += ('{0}TemplatePack\TemplatePack.csproj' -f $scriptDir)
    $msbuildArgs += '/m'
    $msbuildArgs += '/nologo'
    $msbuildArgs += ("/p:Configuration=Debug")
    $msbuildArgs += ("/p:VisualStudioVersion=11.0")
    if(!$preventOverridingTargetsPath){
        $msbuildArgs += ("/p:TemplateBuilderTargets={0}" -f $templateBuilderTargetsPath)
        $msbuildArgs += ("/p:ls-TasksRoot={0}" -f $templateTaskRoot)
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
