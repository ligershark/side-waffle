
"`nThis script relies on PSBuild being installed, see https://github.com/ligershark/psbuild.`n" | Write-Host -ForegroundColor Magenta

# this is a work in progress

$customProps = @{}
$customProps.Add('DevEnvDir', "'${env:ProgramFiles(x86)}\Microsoft Visual Studio 12.0\Common7\IDE\'")
$customProps.Add('ExtensionSdkDir', "'${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v8.1\ExtensionSDKs'")
$customProps.Add('Framework40Version', 'v4.0')
$customProps.Add('FrameworkDir', "'C:\windows\Microsoft.NET\Framework\'")
$customProps.Add('FrameworkDIR32', "'C:\windows\Microsoft.NET\Framework\'")
$customProps.Add('FrameworkVersion', 'v4.0.30319')
$customProps.Add('FrameworkVersion32', 'v4.0.30319')
$customProps.Add('FSHARPINSTALLDIR', "'${env:ProgramFiles(x86)}\Microsoft SDKs\F#\3.1\Framework\v4.0\'")
$customProps.Add('MSBUILDLOGTASKINPUTS', '1')
$customProps.Add('PROCESSOR_ARCHITECTURE', 'x86')
$customProps.Add('PROCESSOR_ARCHITEW6432', 'AMD64')
$customProps.Add('VCINSTALLDIR', "'${env:ProgramFiles(x86)}\Microsoft Visual Studio 12.0\VC\'")
$customProps.Add('VisualStudioVersion', '12.0')
$customProps.Add('VSINSTALLDIR', "'${env:ProgramFiles(x86)}\Microsoft Visual Studio 12.0\'")
$customProps.Add('WindowsSdkDir', "'${env:ProgramFiles(x86)}\Windows Kits\8.1\'")
$customProps.Add('WindowsSDK_ExecutablePath_x64', "'${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\x64\'")
$customProps.Add('WindowsSDK_ExecutablePath_x86', "'${env:ProgramFiles(x86)}\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\'")

Set-MSBuild "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

Invoke-MSBuild .\SideWaffle.sln -visualStudioVersion 11.0 -properties $customProps -configuration Release
