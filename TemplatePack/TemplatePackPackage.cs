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
using LigerShark.Templates.DynamicBuilder;
using TemplatePack.Tooling;

namespace TemplatePack
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidTemplatePackPkgString)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
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

                CommandID menuCommandID = new CommandID(GuidList.guidMenuOptionsCmdSet, (int)PkgCmdIDList.SWMenuGroup);
                OleMenuCommand menuItem = new OleMenuCommand(OpenSettings, menuCommandID);
                mcs.AddCommand(menuItem);
            }

            // TODO: we should build the templates in the background if possible, it's blocking the UI now
            _dte.StatusBar.Text = @"Updating project and item templates";
            try
            {
                System.Threading.ThreadPool.QueueUserWorkItem(x => { new DynamicTemplateBuilder().ProcessTemplates(); }, new Object());
            }
            catch (Exception ex)
            {
                // todo: replace with logging or something
                System.Windows.MessageBox.Show(ex.ToString());
            }
            _dte.StatusBar.Text = @"Template update complete";
        }

        private void OpenSettings(object sender, EventArgs e)
        {
            // Here is where our UI (i.e. user control) will go to do all the settings.
            var window = new SettingsForm();
            window.Show();
        }

        void button_BeforeQueryStatus(object sender, EventArgs e)
        {
            var button = (OleMenuCommand)sender;
            var project = GetSelectedProjects().ElementAt(0);

            // TODO: We should only show this if the target project has the TemplateBuilder NuGet pkg installed
            //       or something similar to that.
            button.Visible = true;
            // button.Visible = project.IsWebProject();
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            Project currentProject = GetSelectedProjects().ElementAt(0);
            var projects = _dte.Solution.GetAllProjects();
            var names = from p in projects
                        where p != currentProject
                        select p.Name;

            ProjectSelector selector = new ProjectSelector(names);
            bool? isSelected = selector.ShowDialog();

            if (isSelected.HasValue && isSelected.Value)
            {
                // need to save everything because we will directly write to the project file in the creator
                _dte.ExecuteCommand("File.SaveAll");

                TemplateReferenceCreator creator = new TemplateReferenceCreator();
                var selectedProject = projects.First(p => p.Name == selector.SelectedProjectName);
                creator.AddTemplateReference(currentProject, selectedProject);
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
