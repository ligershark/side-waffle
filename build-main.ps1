[cmdletbinding(DefaultParameterSetName ='build')]
param(
    [Parameter(ParameterSetName='build')]
    $configuration = 'Release',

    [Parameter(ParameterSetName='build')]
    $extraBuildArgs,

    [Parameter(ParameterSetName='build')]
    $restoreNugetPackages = $true,

    [Parameter(ParameterSetName='build')]
    [switch]$UseLocalTemplateBuilderSrc,

    [Parameter(ParameterSetName='build')]
    [switch]$UseLocalSlowCheetahXdtSrc,

    [Parameter(ParameterSetName='build')]
    [switch]$UpdateNugetExe,

    [Parameter(ParameterSetName='build')]
    [switch]$publish,

    [Parameter(ParameterSetName='build')]
    [switch]$disableTemplateValidation,

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
            (new-object Net.WebClient).DownloadString("https://raw.github.com/ligershark/psbuild/master/src/GetPSBuild.ps1") | iex
        }

        if(!(Get-Module -listAvailable 'psbuild')){
            $msg = ('psbuild is required for this script and was unable to be installed automatically. Get psbuild from here: https://github.com/ligershark/psbuild/')
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
            &(Get-Nuget) $cmdArgs | Out-Null
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
        $cmdArgs = @('--dir', $folderToOptimize,'--force')

        'Calling img optimizer with the following args [{0}]' -f ($cmdArgs -join ' ') | Write-Host
        &$imgOptExe $cmdArgs

        'Images optimized' | Write-Host
    }
}


function Get-Nuget{
    [cmdletbinding()]
    param(
        $toolsDir = ("$env:LOCALAPPDATA\LigerShark\tools\"),

        $nugetDownloadUrl = 'http://nuget.org/nuget.exe'
    )
    process{
        if(!(Test-Path $toolsDir)){
            New-Item -Path $toolsDir -ItemType Directory -Force | Out-Null
        }

        $nugetDestPath = Join-Path -Path $toolsDir -ChildPath nuget.exe
        
        if(!(Test-Path $nugetDestPath)){
            'Downloading nuget.exe' | Write-Verbose
            (New-Object System.Net.WebClient).DownloadFile($nugetDownloadUrl, $nugetDestPath)

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
        $nugetExePath = (Get-Nuget)
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
        $nugetExePath = (Get-Nuget)
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

        Invoke-MSBuild $tbSlnFile -configuration $configuration -properties @{'DeployExtension'='false'}
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

function Get-MSDeploy{
    [cmdletbinding()]
    param()
    process{
        $installPath = $env:msdeployinstallpath

        if(!$installPath){
            $keysToCheck = @('hklm:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy\3','hklm:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy\2','hklm:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy\1')

            foreach($keyToCheck in $keysToCheck){
                if(Test-Path $keyToCheck){
                    $installPath = (Get-itemproperty $keyToCheck -Name InstallPath | select -ExpandProperty InstallPath)
                }

                if($installPath){
                    break;
                }
            }
        }

        if(!$installPath){
            throw "Unable to find msdeploy.exe, please install it and try again"
        }

        [string]$msdInstallLoc = (join-path $installPath 'msdeploy.exe')

        "Found msdeploy.exe at [{0}]" -f $msdInstallLoc | Write-Verbose
        
        $msdInstallLoc
    }
}

function Filter-String{
[cmdletbinding()]
    param(
        [Parameter(Position=0,Mandatory=$true,ValueFromPipeline=$true)]
        [string[]]$message,
        [string[]]$textToRemove
    )
    process{
        foreach($msg in $message){
            $replacements = @()
            if(![string]::IsNullOrEmpty($env:deployUserName)){
                $replacements += $env:deployUserName
            }
            if(![string]::IsNullOrEmpty($env:deployPassword)){
                $replacements += $env:deployPassword
            }
            $textToRemove | % { $replacements += $_ }
            $replacements = ($replacements | Select-Object -Unique)
            
            $replacements | % {
                $msg = $msg.Replace($_,'REMOVED-FROM-LOG')
            }

            $msg
        }
    }
}

<#
.SYNOPSIS
    Used to execute a command line tool (i.e. nuget.exe) using cmd.exe. This is needed in
    some cases due to hanlding of special characters.

.EXAMPLE
    Execute-CommandString -command ('"{0}" {1}' -f (Get-NuGet),(@('install','psbuild') -join ' '))
    Calls nuget.exe install psbuild using cmd.exe

.EXAMPLE
    '"{0}" {1}' -f (Get-NuGet),(@('install','psbuild') -join ' ') | Execute-CommandString
    Calls nuget.exe install psbuild using cmd.exe

.EXAMPLE
    @('psbuild','packageweb') | % { """$(Get-NuGet)"" install $_ -prerelease"|Execute-CommandString}
    Calls 
        nuget.exe install psbuild -prerelease
        nuget.exe install packageweb -prerelease
#>
function Execute-CommandString{
    [cmdletbinding()]
    param(
        [Parameter(Mandatory=$true,Position=0,ValueFromPipeline=$true)]
        [string[]]$command,

        [switch]
        $ignoreExitCode
    )
    process{
        foreach($cmdToExec in $command){
            'Executing command [{0}]' -f $cmdToExec | Write-Verbose
            cmd.exe /D /C $cmdToExec

            if(-not $ignoreExitCode -and ($LASTEXITCODE -ne 0)){
                $msg = ('The command [{0}] exited with code [{1}]' -f $cmdToExec, $LASTEXITCODE)
                throw $msg
            }
        }
    }
}

<#
.SYNOPSIS
Publishes the latest to sidewaffle.com

.DESCRIPTION
This will publish the following files to sidewaffle.com
    TemplatePack.vsix -> sidewaffle.com/feed/web/TemplatePack.vsix 
    extension.vsixmanifest -> sidewaffle.com/feed/web/extension.vsixmanifest
    release-notes.html -> sidewaffle.com/release-notes.html
#>
function Publish{
    [cmdletbinding()]
    param(
        [string]$outputPath = (join-path $script:scriptDir 'TemplatePack\bin\Release\'),

        [ValidateNotNullOrEmpty()]
        [string]$deployUserName = ($env:deployUserName),
        
        [ValidateNotNullOrEmpty()]
        [string]$deployPassword = ($env:deployPassword)
    )
    process{        
        if(!(Test-Path $outputPath)){ throw ('OutputPath not found at {0}' -f $outputPath) }
        if([string]::IsNullOrEmpty($deployUserName)){ throw 'deployUserName is required but missing'}
        if([string]::IsNullOrEmpty($deployPassword)){ throw 'deployUserName is required but missing'}

        $filesToPublish = @()
        $filesToPublish += @{
            'path' = (join-path $outputpath 'TemplatePack.vsix')
            'dest' = 'feed/web/TemplatePack.vsix'
        }
        $filesToPublish += @{
            'path' = (join-path $outputpath 'extension.vsixmanifest')
            'dest' = 'feed/web/extension.vsixmanifest'
        }
        if(Test-Path (join-path $outputpath 'release-notes.html')){
            $filesToPublish += @{
                'path' = (join-path $outputpath 'release-notes.html')
                'dest' = 'release-notes.html'
            }
        }
        if(Test-Path (join-path $outputpath 'release-notes.xml')){
            $filesToPublish += @{
                'path' = (join-path $outputpath 'release-notes.xml')
                'dest' = 'release-notes.html'
            }
        }

        # call msdeploy once for each of these files
        # msdeploy.exe 
            #-source:contentPath='C:\Data\personal\mycode\side-waffle\release-notes.html' 
            #-dest:contentPath='"sidewaffle.com/release-notes.html"',UserName='<user-here>',Password='<pwd-here>',ComputerName='sidewaffle.com',IncludeAcls='False',AuthType='NTLM' 
            #-verb:sync 
            #-enableRule:DoNotDeleteRule
            #-whatif
    
        $commandFormatString = '"{0}" -verb:sync -disablerule:BackupRule -enableRule:DoNotDeleteRule -source:contentPath=''{1}'' -dest:contentPath=''sidewaffle.com/{2}'',UserName=''{3}'',Password=''{4}'',ComputerName=''sidewaffle.com'',IncludeAcls=''False'',AuthType=''NTLM''  '

        try{
            $filesToPublish | % {
                # build the cmd string
                ($commandFormatString -f (Get-MSDeploy), $_.path , $_.dest, $deployUserName,$deployPassword) | Execute-CommandString
            }
        }
        catch{
            # make sure to not show errors with secrets
            throw (Filter-String $_.ToString() -textToRemove @($deployUserName,$deployPassword))
        }
    }
}

function DoBuild{
    [cmdletbinding()]
    param()
    process{
        'Begin started. This script uses psbuild which is available at https://github.com/ligershark/psbuild' | Write-Host

        $env:DeployExtension=$false

        EnsurePsbuildInstalled

        if(-not ($disableTemplateValidation)){
            ValidateTemplates
        }
        else{
            'Not validating templates based on "disableTemplateValidation" switch' | Write-Warning
        }

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

            $msbArgs = @{
                'projectsToBuild'=$projToBuild
                'configuration' = $configuration
                'visualStudioVersion'=$global:SideWaffleBuildSettings.VisualStudioVersion
                'properties'=$buildProperties
                'nologo'=$true
            }

            Invoke-MSBuild @msbArgs

            if($publish){
                'Starting the publish process to sidewaffle.com' | Write-Output
                Publish
            }
        }
    }
}

function ValidateTemplates{
    [cmdletbinding()]
    param(
        [Parameter(Position=0,ValueFromPipeline=$true)]
        [array]$templateFiles = (GetTemplateFiles -templateRoot $script:scriptDir)
    )
    process{
        [array]$errors = @()
        $ns = 'http://schemas.microsoft.com/developer/vstemplate/2005'
        [array]$xpathtotest = @(
                '/dft:VSTemplate/dft:TemplateData/dft:Name'
                '/dft:VSTemplate/dft:TemplateData/dft:Description'
                '/dft:VSTemplate/dft:TemplateData/dft:SortOrder'
                '/dft:VSTemplate/dft:TemplateContent/dft:CustomParameters/dft:CustomParameter[1][@Name="$TemplateName$"]'
                '/dft:VSTemplate/dft:TemplateContent/dft:CustomParameters/dft:CustomParameter[@Name="$TemplateName$"]'
                '/dft:VSTemplate/dft:TemplateContent/dft:CustomParameters/dft:CustomParameter[@Name="$TemplateID$"]'
                '/dft:VSTemplate/dft:TemplateContent/dft:CustomParameters/dft:CustomParameter[@Name="$TemplateType$"]'
                '/dft:VSTemplate/dft:WizardExtension/dft:Assembly[text()="LigerShark.Templates, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"]'
                '/dft:VSTemplate/dft:WizardExtension/dft:FullClassName[text()="LigerShark.Templates.GoogleAnalyticsWizard"]'
            )

        foreach($template in $templateFiles){
            if(-not (Test-Path $template.FullName)){
                $errors += ('Template not found at [{0}]' -f $template.FullName)
                continue
            }

            foreach($xpath in $xpathtotest){
                if( (Select-Xml -Path $template.fullname -Namespace @{'dft'=$ns} -XPath $xpath) -eq $null){
                    $errors += ('Template file [{0}] is missing a requried xml value at [{1}]' -f $template.FullName,$xpath)
                }
            }
        }

        if($errors.Count -gt 0){
            Write-Error -Message ($errors -join [System.Environment]::NewLine)
            throw ('Missing required template values')
        }
    }
}

function GetTemplateFiles{
    [cmdletbinding()]
    param(
        [Parameter(Position=0,Mandatory=$true)]
        [ValidateScript({Test-Path $_})]
        [System.IO.DirectoryInfo]$templateRoot = $script:scriptDir
    )
    process{
        # find template files to process
        # [array]$templatefiles = Get-ChildItem $templateRoot
        [array]$folderswithtemplates = @( (Get-Item -Path (Join-Path $templateRoot 'TemplatePack')),(Get-Item -Path (Join-Path $templateRoot 'item-templates')), (Get-Item -Path (Join-Path $templateRoot 'project-templates')), (Get-Item -Path (Join-Path $templateRoot 'Project Templates')))

        [array]$templatefiles = @()

        foreach($f in $folderswithtemplates){
            if(-not (Test-Path $f.FullName)){
                throw ('Did not find templates folder at {0}' -f $f.FullName)
            }

            $templatefiles += Get-ChildItem -Path $f.FullName -Filter _project.vstemplate.xml -Recurse -File | Where-Object { $_.Directory.Name -ne 'SW-ProjectVSTemplateFile' }

            $templatefiles += (Get-ChildItem -Path $f.FullName -Filter *.vstemplate -Recurse -File)
        }

        $templatefiles
    }
}

try{
    DoBuild
}
catch{
    throw (Filter-String $_.Exception)
}