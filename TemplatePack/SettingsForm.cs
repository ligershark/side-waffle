using LigerShark.Templates.DynamicBuilder;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TemplatePack.Tooling;

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
            templateBuilder = new DynamicTemplateBuilder();
            templateSettings = new RemoteTemplateSettings();
            NumberOfSources = 0;

            // Load the list of sources
            var templateList = templateBuilder.GetTemplateSettingsFromJson();
            OriginalUpdateInterval = templateList.UpdateInterval.ToString();

            foreach (var template in templateList.Sources)
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

            // Check the box for the user's configuration schedule (default: Once A Week)
            switch (NewUpdateInterval)
            {
                case "OnceADay":
                    onceADayCheckbox.Checked = true;
                    NewUpdateInterval = "OnceADay";
                    break;
                case "OnceAWeek":
                    onceAWeekCheckbox.Checked = true;
                    NewUpdateInterval = "OnceAWeek";
                    break;
                case "OnceAMonth":
                    onceAMonthCheckbox.Checked = true;
                    NewUpdateInterval = "OnceAMonth";
                    break;
                case "Never":
                    neverCheckBox.Checked = true;
                    NewUpdateInterval = "Never";
                    break;
                default:
                    onceAWeekCheckbox.Checked = true;
                    NewUpdateInterval = "OnceAWeek";
                    break;
            }
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

        private void rebuildTemplatesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                LoadingImage.Visible = true;
                LoadingLabel.Visible = true;
                templateBuilder.RebuildAllTemplates();
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

            // Save the configuration schedule if changed
            if (OriginalUpdateInterval != NewUpdateInterval)
            {
                UpdateFrequency frequency = (UpdateFrequency) Enum.Parse(typeof(UpdateFrequency), NewUpdateInterval, true);
                templateSettings.UpdateInterval = frequency;
            }

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

        public void SourceName_TextChanged(object sender, EventArgs e)
        {
            if (CurrentItemSelected != null)
            {
                CurrentItemSelected.SubItems[1].Text = sourceNameTextBox.Text;
            }
        }

        public void SourceURL_TextChanged(object sender, EventArgs e)
        {
            if (CurrentItemSelected != null)
            {
                string url = sourceUrlTextBox.Text;
                if ((url.StartsWith("http")) || (url.StartsWith("https")) || (url.StartsWith("git")))
                {
                    sourceBranchTextBox.Enabled = true;
                    CurrentItemSelected.SubItems[3].Text = sourceBranchTextBox.Text;
                }

                else
                {
                    sourceBranchTextBox.Enabled = false;
                }

                CurrentItemSelected.SubItems[2].Text = url;
            }
        }

        public void SourceBranch_TextChanged(object sender, EventArgs e)
        {
            if (CurrentItemSelected != null && sourceBranchTextBox.Enabled == true)
            {
                CurrentItemSelected.SubItems[3].Text = sourceBranchTextBox.Text;
            }
        }

        public void SourcesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedRows = remoteSourceListView.SelectedItems;

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
        }

        public int NumberOfSources
        {
            get { return _numSources; }
            set { _numSources = value; }
        }

        private ListViewItem CurrentItemSelected { get; set; }

        private string NewUpdateInterval { get; set; }

        private string OriginalUpdateInterval { get; set; }
    }
}
