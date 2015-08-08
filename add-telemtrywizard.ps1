<#
#   Name:           add-telmetrywizard.ps1
#   Author:         Tyler Hughes (RandomlyKnighted)
#   Purpose:        This script recursively goes through all VSTemplate files located in the SideWaffle project adding a Wizard
#                   to each template. After implementation the wizard when used will use Google Analytics to help keep track of
#                   what templates are most commonly used.
#   Last Updated:   August 6, 2015
#>

$docsFolder = [environment]::getfolderpath("mydocuments") + "\Visual Studio 2015\Projects\side-waffle"
$files = Get-ChildItem $docsFolder *.vstemplate -Recurse | % { $_.FullName }

foreach($filePath in $files)
{
    [xml]$templateXml = Get-Content $filePath
    $updatedFile = $false
    'Processing [{0}]' -f $filePath | Write-Host

    '    Adding Telemetry Wizard' | Write-Host


    # Create the CustomParameters element
    $customParameters = $templateXml.CreateElement($null,'CustomParameters','http://schemas.microsoft.com/developer/vstemplate/2005')

    # Create the parameter elements that will pass the data to the wizard
    $nameParameter = $templateXml.CreateElement($null, 'CustomParameter', 'http://schemas.microsoft.com/developer/vstemplate/2005')
    $guidParameter = $templateXml.CreateElement($null, 'CustomParameter', 'http://schemas.microsoft.com/developer/vstemplate/2005')

    # Get the name of the template from the VSTemplate file then create new GUID for the TemplateID
    $templateName = $templateXml.VSTemplate.TemplateData.Name
    $guid = [guid]::NewGuid()

    # Set the values to their respective paramter elements
	$nameParameter.SetAttribute("Name", "`$TemplateName`$")
    $nameParameter.SetAttribute("Value", $templateName)
	$guidParameter.SetAttribute("Name", "`$TemplateID`$")
    $guidParameter.SetAttribute("Value", $guid)

    $customParameters.AppendChild($nameParameter)
    $customParameters.AppendChild($guidParameter)

    # Create the WizardExtension element
    $wizardExtension = $templateXml.CreateElement($null,'WizardExtension','http://schemas.microsoft.com/developer/vstemplate/2005')
    $assembly = $templateXml.CreateElement($null,'Assembly','http://schemas.microsoft.com/developer/vstemplate/2005')
    $fullclassname = $templateXml.CreateElement($null,'FullClassName','http://schemas.microsoft.com/developer/vstemplate/2005')
    $assembly.InnerText = 'LigerShark.Templates, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
    $fullclassname.InnerText = 'LigerShark.Templates.GoogleAnalyticsWizard'
    $wizardExtension.AppendChild($assembly)
    $wizardExtension.AppendChild($fullclassname)

    # Add both elements to the VSTemplate file
    $templateXml.VSTemplate.TemplateContent.AppendChild($customParameters) | Out-Null
    $templateXml.VSTemplate.AppendChild($wizardExtension) | Out-Null
    $updatedFile = $true

    if($updatedFile)
    {
        $templateXml.Save($filePath)
        $updatedFile = $false
    }
}