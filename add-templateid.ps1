[cmdletbinding()]
param(
    $rootDir
)

function InternalGet-ScriptDirectory{
    split-path (((Get-Variable MyInvocation -Scope 1).Value).MyCommand.Path)
}
$scriptDir = ((InternalGet-ScriptDirectory) + "\")

if([string]::IsNullOrEmpty($rootDir)){
    $rootDir = $scriptDir
}

if(-not (Test-Path $rootDir)){
    throw ('rootDir not found at [{0}]' -f $rootDir)
}

Get-ChildItem $rootDir -Include *.vstemplate,_project.vstemplate.xml -Recurse -File | ForEach-Object {
    $templatepath = $_
    'Inspecting [{0}] for TemplateID' -f $templatepath | Write-Verbose

    try{
        [xml]$template = Get-Content $_
        # see if the file has a TemplateId parameter, if so leave it alone
        if(-not $template.VSTemplate.TemplateData.TemplateID){
            'Adding TemplateId to [{0}]' -f $templatepath  | Write-Verbose
            $idelement = $template.CreateElement('TemplateID','http://schemas.microsoft.com/developer/vstemplate/2005')
            $idelement.InnerText = ([guid]::NewGuid())
            $template.VSTemplate.TemplateData.AppendChild($idelement) | Out-Null
            $template.Save($templatepath) | Out-Null
        }
    }
    catch{
        'Unable to read/write to the file [{0}]. Error: {1}' -f $templatepath, $_.Exception | Write-Error
        throw $_.Exception
    }
}

