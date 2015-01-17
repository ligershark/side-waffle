using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            OriginalUpdateInterval = templateSettings.UpdateInterval.ToString();

            // Load the list of sources
            var templateList = templateBuilder.GetTemplateSettingsFromJson();

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
            switch (UpdateInterval)
            {
                case "OnceADay":
                    onceADayCheckbox.Checked = true;
                    UpdateInterval = "OnceADay";
                    break;
                case "OnceAWeek":
                    onceAWeekCheckbox.Checked = true;
                    UpdateInterval = "OnceAWeek";
                    break;
                case "OnceAMonth":
                    onceAMonthCheckbox.Checked = true;
                    UpdateInterval = "OnceAMonth";
                    break;
                case "Never":
                    neverCheckBox.Checked = true;
                    UpdateInterval = "Never";
                    break;
                default:
                    onceAWeekCheckbox.Checked = true;
                    UpdateInterval = "OnceAWeek";
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

            newSourceAdded = true;
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
            templateBuilder.RebuildAllTemplates();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            /*
             *  Save the new or updated sources - It might be easier just to save whatever is in
             *  the ListView when the Ok button is clicked. Reason being by doing it this way we don't
             *  have to worry about keeping up with whether or not the list changed...one thing we would
             *  have to worry about though is whether or not the user finished adding a new source
             */

            // Save the configuration schedule if changed
            if (OriginalUpdateInterval != UpdateInterval)
            {
                // UpdateInterval is what you want to save
            }

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

        private string UpdateInterval { get; set; }

        private string OriginalUpdateInterval { get; set; }
    }
}
