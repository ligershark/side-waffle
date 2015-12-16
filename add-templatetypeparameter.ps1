<#
#   Name:           add-templatetypeparameter.ps1
#   Author:         Tyler Hughes (RandomlyKnighted)
#   Purpose:        This script recursively goes through all VSTemplate files located in the SideWaffle project and adds a new
#                   Custom Parmeter to each template. This new Custom Paramter lets us know if the template being created is a
#                   project or item template.
#   Last Updated:   December 14, 2015
#>

$docsFolder = [environment]::getfolderpath("mydocuments") + "\Visual Studio 2015\Projects\side-waffle\Project Templates"
$files = Get-ChildItem $docsFolder *.vstemplate.xml -Recurse | % { $_.FullName }

foreach($filePath in $files)
{
    [xml]$templateXml = Get-Content $filePath
    $updatedFile = $false
    'Processing [{0}]' -f $filePath | Write-Host

    '    Adding Telemetry Wizard' | Write-Host

    # Create the new parameter element that will pass the template type to the wizard
    $typeParameter = $templateXml.CreateElement($null, 'CustomParameter', 'http://schemas.microsoft.com/developer/vstemplate/2005')

	$templateType = "Project"

    # Set the values to their respective paramter elements
	$typeParameter.SetAttribute("Name", "`$TemplateType`$")
    $typeParameter.SetAttribute("Value", $templateType)

    $templateXml.VSTemplate.TemplateContent.CustomParameters.AppendChild($typeParameter) | Out-Null
    Write-Host ($templateXml)

    $updatedFile = $true

    if($updatedFile)
    {
        $templateXml.Save($filePath)
        $updatedFile = $false
    }
}