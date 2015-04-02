using EnvDTE;
using EnvDTE80;
using Microsoft.Build.Construction;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;
using System;
using System.Collections.Generic;
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
            InstallTemplateBuilderPackage(selectedProject);

            // Add the SideWaffle Project Template files into the target project
            SVsServiceProvider ServiceProvider = null;
            DTE dte = (DTE)ServiceProvider.GetService(typeof(DTE));

            Solution2 solution = (Solution2)dte.Solution;
            String itemPath = solution.GetProjectItemTemplate("", "CSharp");
            selectedProject.ProjectItems.AddFromTemplate(itemPath, "");

            curProjObj.Save();
        }

        private IEnumerable<KeyValuePair<string, string>> GetTemplateReferenceMetadata(string relativePath) {
            var result = new List<KeyValuePair<string, string>>();
            result.Add(new KeyValuePair<string, string>("PathToProject", relativePath));
            // result.Add(new KeyValuePair<string, string>("Visible", "False"));
            return result;
        }

        private bool InstallTemplateBuilderPackage(EnvDTE.Project project)
        {
            bool installedPkg = true;
            try
            {
                var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
                IVsPackageInstallerServices installerServices = componentModel.GetService<IVsPackageInstallerServices>();

                if (!installerServices.IsPackageInstalled(project, "TemplateBuilder"))
                {
                    var installer = componentModel.GetService<IVsPackageInstaller>();
                    installer.InstallPackage("All", project, "TemplateBuilder", (System.Version)null, false);
                }
            }

            catch (Exception ex)
            {
                installedPkg = false;

                // Log the failure
            }

            return installedPkg;
        }
    }
}
