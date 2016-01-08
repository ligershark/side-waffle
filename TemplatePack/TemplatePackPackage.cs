using EnvDTE;
using EnvDTE80;
using LigerShark.Templates.DynamicBuilder;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using TemplatePack.Tooling;

namespace TemplatePack
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidTemplatePackPkgString)]
    [ProvideOptionPage(typeof(OptionPageGrid), "SideWaffle", "General", 0, 0, true)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class TemplatePackPackage : Package
    {
        private DTE2 _dte;
        private ActivityLogger _activityLogger;

        protected override void Initialize()
        {
            base.Initialize();
            _dte = GetService(typeof(DTE)) as DTE2;
            _activityLogger = new ActivityLogger(GetService(typeof(SVsActivityLog)) as IVsActivityLog);

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

            System.Threading.Tasks.Task.Run(async () => {
                await System.Threading.Tasks.Task.Delay(100);
                
                try {
                    new DynamicTemplateBuilder(_dte, _activityLogger).ProcessTemplates();
                }
                catch (Exception ex) {
                    _activityLogger.Error(ex.ToString());
                    _dte.StatusBar.Text = @"An error occured while updating templates, check the activity log";

                    // Leave this for now until we are sure activity logger above works well
                    System.Windows.MessageBox.Show(ex.ToString());
                }
            });
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

        public bool SendTelemetry
        {
            get
            {
                OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
                return page.SendAnonymousData;
            }
        }
    }

    public class OptionPageGrid : DialogPage
    {
        [Category("Telemetry")]
        [DisplayName("Send Anonymous Data")]
        [Description("By selecting true, you agree to send anonymous data to Google Analytics. This data will be used by the SideWaffle team to see how templates are being used.")]
        [DefaultValue(true)]
        public bool SendAnonymousData { get; set; }

        // When the user clicks the OK button in the Options window
        // save the settings in the JSON file.
        protected override void OnApply(PageApplyEventArgs e)
        {
            if (e.ApplyBehavior == ApplyKind.Apply)
            {
                // TODO: Add save settings feature here
                SettingsStore userSettings = new SettingsStore { SendTelemetry = SendAnonymousData };
                string json = JsonConvert.SerializeObject(userSettings, Formatting.Indented);

                // Get the file path where the settings will be stored.
                var rootDir = Environment.ExpandEnvironmentVariables(@"%localappdata%\LigerShark\SideWaffle\");
                var filePath = Path.Combine(rootDir, "SideWaffle-Settings.json");

                // Save the settings to the JSON file
                userSettings.WriteJsonFile(rootDir, filePath, json);
            }
            base.OnApply(e);
        }
    }
}
