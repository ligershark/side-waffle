$str = ""
$csStr = ""
$vbStr = ""

Get-ChildItem *.t4 -Recurse | 
    ForEach-Object { 
        $relPath = ((Resolve-Path $_.FullName -Relative).Substring(2))

        $isCsFile = ($relPath.EndsWith('.cs.t4'))
        $isVbFile = ($relPath.EndsWith('.vb.t4'))

        $strFragment = ("<ProjectItem TargetFileName=""CodeTemplates\$relPath"" ReplaceParameters=""true"">$relPath</ProjectItem>{1}" -f $relPath,[Environment]::NewLine)
        if($relPath.EndsWith('.cs.t4')){
            $csStr += $strFragment
        }
        if($relPath.EndsWith('.vb.t4')){
            $vbStr += $strFragment
        }
    }

    $str = ("******** CSharp ********{0}" -f [Environment]::NewLine)
    $str += $csStr
    
    $str += ("{0}{0}******** Visual Basic ********{0}" -f [Environment]::NewLine)
    $str += $vbStr

    $str | Write-Host

Set-Content C:\temp\scaffoldingtemp.txt -Value $str