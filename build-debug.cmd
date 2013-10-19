%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe %~dp0build.proj /p:Configuration=Release /p:VisualStudioVersion=11.0 /flp1:v=d;logfile=build.d.log /flp2:v=diag;logfile=build.diag.log /p:TemplateBuilderTargets=C:\Data\personal\mycode\template-builder\tools\ligershark.templates.targets


