using EnvDTE;
using EnvDTE80;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;


namespace TemplatePack {
    class TemplateReferenceCreator {

        public void AddTemplateReference(EnvDTE.Project currentProject, EnvDTE.Project selectedProject) {           
            // we need to compute a relative path for the template project
            Uri currentProjUri = new Uri(currentProject.FullName);
            Uri selectedProjUri = new Uri(selectedProject.FullName);
            string relativePath = currentProjUri.MakeRelativeUri(selectedProjUri).ToString();

            FileInfo selectedProjFile = new FileInfo(selectedProject.FullName);

            var curProjObj = ProjectRootElement.Open(currentProject.FullName);
            var item = curProjObj.AddItem("TemplateReference", selectedProjFile.Name, GetTemplateReferenceMetadata(relativePath));

            // Install the TemplateBuilder NuGet pkg into the target project
            
            InstallTemplateBuilderPackage(currentProject);

            // Add the SideWaffle Project Template files into the target project
            var dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
            Solution2 solution = (Solution2)dte2.Solution;
            string itemPath = solution.GetProjectItemTemplate("SW-ProjectVSTemplateFile.csharp.zip", "CSharp");
            selectedProject.ProjectItems.AddFromTemplate(itemPath, "_project1.vstemplate");

            curProjObj.Save();

            // Reload the project
            ReloadProject(dte2, currentProject);
        }

        private IEnumerable<KeyValuePair<string, string>> GetTemplateReferenceMetadata(string relativePath) {
            var result = new List<KeyValuePair<string, string>>();
            result.Add(new KeyValuePair<string, string>("PathToProject", relativePath));
            // result.Add(new KeyValuePair<string, string>("Visible", "False"));
            return result;
        }

        private bool InstallTemplateBuilderPackage(Project project)
        {
            bool installedPkg = true;
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE;

            try
            {
                var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
                IVsPackageInstallerServices installerServices = componentModel.GetService<IVsPackageInstallerServices>();

                if (!installerServices.IsPackageInstalled(project, "TemplateBuilder"))
                {
                    dte.StatusBar.Text = @"Installing TemplateBuilder NuGet package, this may take a minute...";

                    var installer = componentModel.GetService<IVsPackageInstaller>();
                    installer.InstallPackage(null, project, "TemplateBuilder", (System.Version)null, false);

                    dte.StatusBar.Text = @"Finished installing the TemplateBuilder NuGet package";
                }
            }

            catch (Exception ex)
            {
                installedPkg = false;

                dte.StatusBar.Text = @"Unable to install the TemplateBuilder NuGet package";

                // Log the failure to the ActivityLog
            }

            return installedPkg;
        }

        private void ReloadProject(DTE2 dte2, Project currentProject)
        {
            // we need to unload and reload the project
            dte2.ExecuteCommand("File.SaveAll");

            string solutionName = System.IO.Path.GetFileNameWithoutExtension(dte2.Solution.FullName);
            string projectName = currentProject.Name;

            dte2.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Activate();
            ((DTE2)dte2).ToolWindows.SolutionExplorer.GetItem(solutionName + @"\" + projectName).Select(vsUISelectionType.vsUISelectionTypeSelect);

            dte2.ExecuteCommand("Project.UnloadProject");
            System.Threading.Thread.Sleep(500);
            dte2.ExecuteCommand("Project.ReloadProject");
        }
    }
}
