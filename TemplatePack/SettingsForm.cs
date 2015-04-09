using EnvDTE;
using EnvDTE80;
using LigerShark.Templates.DynamicBuilder;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TemplatePack.Tooling;
using System.Linq;

namespace TemplatePack
{
    public partial class SettingsForm : Form
    {
        DynamicTemplateBuilder templateBuilder;
        RemoteTemplateSettings templateSettings;
        private bool templateSourcesChanged = false;
        private bool newSourceAdded = false;
        private int _numSources;

        public SettingsForm()
        {
            InitializeComponent();
            templateBuilder = new DynamicTemplateBuilder(
                                    GetService(typeof(DTE)) as DTE2,
                                    new ActivityLogger(GetService(typeof(SVsActivityLog)) as IVsActivityLog));
            templateSettings = new RemoteTemplateSettings();
            NumberOfSources = 0;

            // Load the list of sources
            var templateList = templateBuilder.GetTemplateSettingsFromJson();
            UpdateInterval = templateList.UpdateInterval.ToString();
            LoadSourcesListView(templateList);

            // Check the box for the user's configuration schedule (default: Once A Week)
            SetupRadioButtons(UpdateInterval);

            alwaysRadioBtn.Tag = UpdateFrequency.Always;
            onceADayRadioBtn.Tag = UpdateFrequency.OnceADay;
            onceAWeekRadioBtn.Tag = UpdateFrequency.OnceAWeek;
            onceAMonthRadioBtn.Tag = UpdateFrequency.OnceAMonth;
            neverRadioBtn.Tag = UpdateFrequency.Never;
        }

        private void editBtn_click(object sender, EventArgs e)
        {
            // Get the selected row
            ListView.SelectedListViewItemCollection selectedRows = remoteSourceListView.SelectedItems;

            // Load the selected row into the textboxes
            if (remoteSourceListView.SelectedItems.Count > 0)
            {
                CurrentItemSelected = remoteSourceListView.SelectedItems[0];
                sourceNameTextBox.Text = CurrentItemSelected.SubItems[1].Text;
                sourceUrlTextBox.Text = CurrentItemSelected.SubItems[2].Text;

                if (CurrentItemSelected.SubItems[3].Text != "")
                {
                    sourceBranchTextBox.Enabled = true;
                    sourceBranchTextBox.Text = CurrentItemSelected.SubItems[3].Text;
                }
            }

            applyBtn.Visible = true;
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            if (CurrentItemSelected != null)
            {
                // Update the name of the selected item
                CurrentItemSelected.SubItems[1].Text = sourceNameTextBox.Text;
                CurrentItemSelected.SubItems[2].Text = sourceUrlTextBox.Text;

                if (sourceBranchTextBox.Enabled == true)
                {
                    // Update the selected item's branch if and only if the branch textbox is enabled
                    CurrentItemSelected.SubItems[3].Text = sourceBranchTextBox.Text;
                }
            }

            // Reset the textboxes and buttons
            sourceNameTextBox.Clear();
            sourceBranchTextBox.Text = "origin/master";
            sourceUrlTextBox.Clear();
            applyBtn.Visible = false;
        }

        private void newSourceBtn_Click(object sender, EventArgs e)
        {
            // Add new row to the ListView
            ListViewItem row = new ListViewItem();
            row.SubItems.Add(String.Empty);
            row.SubItems.Add(String.Empty);
            row.SubItems.Add(String.Empty);

            remoteSourceListView.Items.Add(row);

            // Select the row that was just added
            CurrentItemSelected = row;
            sourceNameTextBox.Text = "new";
            sourceUrlTextBox.Clear();

            if (sourceBranchTextBox.Enabled == true)
            {
                // Reset the branch textbox
                sourceBranchTextBox.Enabled = false;
                sourceBranchTextBox.Text = "origin/master";
            }

            applyBtn.Visible = true;
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            // Delete the selected item
            CurrentItemSelected = remoteSourceListView.SelectedItems[0];

            try
            {
                remoteSourceListView.Items.RemoveAt(CurrentItemSelected.Index);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error Removing List Item: " + ex.Message);
            }
            
        }

        private void BranchTextbox_Validated(object sender, EventArgs e)
        {
            var branch = sourceBranchTextBox.Text;

            Regex regex = new Regex(".{1,}/.{1,}");
            Match match = regex.Match(branch);
            
            if (!match.Success)
            {
                MessageBox.Show("Sorry, the branch you entered is not in the correct format.");
            }
        }

        private async void rebuildTemplatesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                LoadingImage.Visible = true;
                LoadingLabel.Visible = true;


                // Templates are rebuilt on another thread in order to avoid freezing the GIF image by locking the UI thread
                await Task.Run(() =>
                {
                    templateBuilder.RebuildAllTemplates();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Occurred:" + ex.Message);
            }
            finally
            {
                LoadingImage.Visible = false;
                LoadingLabel.Text = "All templates have been built";
            }
        }

        private void ResetDefaultsBtn_Clock(object sender, EventArgs e)
        {
            // Reset the templatesources.json file
            var settings = templateBuilder.GetDefaultJsonSettings();
            templateBuilder.WriteJsonTemplateSettings(settings);

            // Reset the ListView
            remoteSourceListView.Items.Clear();
            LoadSourcesListView(settings);

            // Reset the radio buttons so that is shows the default selection
            SetupRadioButtons(templateBuilder.GetTemplateSettingsFromJson().UpdateInterval.ToString());
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            List<TemplateSource> sources = new List<TemplateSource>();

            foreach (ListViewItem row in remoteSourceListView.Items)
            {
                TemplateSource source = new TemplateSource();
                if (row.Checked == true)
                {
                    source.Enabled = true;
                }

                else
                {
                    source.Enabled = false;
                }

                source.Name = row.SubItems[1].Text;
                
                Uri uri = new Uri(row.SubItems[2].Text);
                Uri url = uri;
                if (uri.IsFile)
                {
                    
                    url = new Uri(uri.AbsoluteUri);
                }

                source.Location = url;

                if (row.SubItems[3].Text != "")
                {
                    source.Branch = row.SubItems[3].Text;
                }

                sources.Add(source);
            }

            templateSettings.Sources = sources;

            // Save the configuration schedule
            var checkbox = scheduleGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            //UpdateFrequency frequency = (UpdateFrequency)Enum.Parse(typeof(UpdateFrequency), checkbox.Text, true);
            //MessageBox.Show("You changed the update frequency to " + checkbox.Tag.ToString());
            templateSettings.UpdateInterval = (UpdateFrequency)checkbox.Tag;

            // Here is where the .json file needs to be saved before calling ProcessTemplates
            templateBuilder.WriteJsonTemplateSettings(templateSettings);

            // If templatesources.json has changed then refresh the template building process
            if (templateSourcesChanged || newSourceAdded)
            {
                templateBuilder.ProcessTemplates();
            }

            templateSourcesChanged = false;
            newSourceAdded = false;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SourceURL_TextChanged(object sender, EventArgs e)
        {

            string url = sourceUrlTextBox.Text;
            if ((url.StartsWith("http")) || (url.StartsWith("https")) || (url.StartsWith("git")))
            {
                sourceBranchTextBox.Enabled = true;
            }

            else
            {
                sourceBranchTextBox.Enabled = false;
            }
        }

        public void LoadSourcesListView(RemoteTemplateSettings rts)
        {
            foreach (var template in rts.Sources)
            {
                ListViewItem row = new ListViewItem();

                if (template.Enabled)
                {
                    row.Checked = true;
                }

                else
                {
                    row.Checked = false;
                }

                row.SubItems.Add(template.Name);
                row.SubItems.Add(template.Location.ToString());
                row.SubItems.Add(template.Branch);

                remoteSourceListView.Items.Add(row);
                NumberOfSources += 1;
            }
        }

        public void SetupRadioButtons(string interval)
        {
            switch (interval)
            {
                case "Always":
                    alwaysRadioBtn.Checked = true;
                    break;
                case "OnceADay":
                    onceADayRadioBtn.Checked = true;
                    break;
                case "OnceAWeek":
                    onceAWeekRadioBtn.Checked = true;
                    break;
                case "OnceAMonth":
                    onceAMonthRadioBtn.Checked = true;
                    break;
                case "Never":
                    neverRadioBtn.Checked = true;
                    break;
                default:
                    onceAWeekRadioBtn.Checked = true;
                    break;
            }
        }

        public int NumberOfSources
        {
            get { return _numSources; }
            set { _numSources = value; }
        }

        private ListViewItem CurrentItemSelected { get; set; }

        private string UpdateInterval { get; set; }
    }
}
