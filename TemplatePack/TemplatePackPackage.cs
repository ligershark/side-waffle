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

            /*if(Environment.GetEnvironmentVariable("SideWaffleEnableDynamicTemplates") != null)*/{
                try {
                    new DynamicTemplateBuilder().ProcessTemplates();
                }
                catch (Exception ex) {
                    // todo: replace with logging or something
                    System.Windows.MessageBox.Show(ex.ToString());
                }
            }
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
     
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidMenuOptionsPkgString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class MenuOptionsPackage : Package
    {     
        // Overridden Package Implementation
        #region Package Members
     
        protected override void Initialize()
        {
            base.Initialize();
     
            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidMenuOptionsCmdSet, (int)PkgCmdIDList.SWMenuGroup);
                OleMenuCommand menuItem = new OleMenuCommand(MenuItemCallback, menuCommandID );
                mcs.AddCommand( menuItem );
            }
        }
        #endregion

        private void MenuItemCallback(object sender, EventArgs e)
        {
            // Here is where our UI (i.e. user control) will go to do all the settings.
            var window = new SettingsForm();
            window.Show();
        }
    }
}
