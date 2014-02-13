using System;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;

namespace TemplatePack
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidTemplatePackPkgString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class TemplatePackPackage : Package
    {
        private DTE2 _dte;

        protected override void Initialize()
        {
            base.Initialize();
            _dte = GetService(typeof(DTE)) as DTE2;

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                CommandID cmdId = new CommandID(GuidList.guidTemplatePackCmdSet, (int)PkgCmdIDList.cmdidMyCommand);
                OleMenuCommand button = new OleMenuCommand(ButtonClicked, cmdId);
                button.BeforeQueryStatus += button_BeforeQueryStatus;
                mcs.AddCommand(button);
            }
        }

        void button_BeforeQueryStatus(object sender, EventArgs e)
        {
            var button = (OleMenuCommand)sender;
            var project = GetSelectedProjects().ElementAt(0);

            button.Visible = project.IsWebProject();
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            Project currentProject = GetSelectedProjects().ElementAt(0);
            var projects = _dte.Solution.GetAllNonWebProjects();
            var names = from p in projects
                        where p != currentProject
                        select p.Name;

            ProjectSelector selector = new ProjectSelector(names);
            bool? isSelected = selector.ShowDialog();

            if (isSelected.HasValue && isSelected.Value)
            {
                WebJobCreator creator = new WebJobCreator();
                var selectedProject = projects.First(p => p.Name == selector.SelectedProjectName);
                creator.AddReference(currentProject, selectedProject);
                creator.CreateFolders(currentProject, selector.SelectedProjectName);

                // ensure that the NuGet package is installed in the project as well
                //InstallWebJobsNuGetPackage(currentProject);
            }
        }

        public IEnumerable<Project> GetSelectedProjects()
        {
            var items = (Array)_dte.ToolWindows.SolutionExplorer.SelectedItems;
            foreach (UIHierarchyItem selItem in items)
            {
                var item = selItem.Object as Project;
                if (item != null)
                {
                    yield return item;
                }
            }
        }

    }
}
