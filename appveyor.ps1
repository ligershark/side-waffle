# install vsixgallery script
(new-object Net.WebClient).DownloadString("https://raw.github.com/madskristensen/ExtensionScripts/master/AppVeyor/vsix.ps1") | iex

Vsix-IncrementVsixVersion | Vsix-UpdateBuildVersion

nuget restore sidewaffle.sln
# install psbuild
(new-object Net.WebClient).DownloadString("https://raw.github.com/ligershark/psbuild/master/src/GetPSBuild.ps1") | iex
.\build-main.ps1

Vsix-PushArtifacts | Vsix-PublishToGallery