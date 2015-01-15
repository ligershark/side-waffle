using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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

        public SettingsForm()
        {
            InitializeComponent();
            templateBuilder = new DynamicTemplateBuilder();
            templateSettings = new RemoteTemplateSettings();

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
            }

            // Check the box for the user's configuration schedule (default: Once A Week)
            switch (templateList.UpdateInterval.ToString())
            {
                case "OnceADay":
                    onceADayCheckbox.Checked = true;
                    break;
                case "OnceAWeek":
                    onceAWeekCheckbox.Checked = true;
                    break;
                case "OnceAMonth":
                    onceAMonthCheckbox.Checked = true;
                    break;
                case "Never":
                    neverCheckBox.Checked = true;
                    break;
                default:
                    onceAWeekCheckbox.Checked = true;
                    break;
            }
        }

        private void newSourceBtn_Click(object sender, EventArgs e)
        {
            // Add new row to the ListView
            ListViewItem row = new ListViewItem();
            row.SubItems.Add("new");
            row.SubItems.Add(String.Empty);
            row.SubItems.Add(String.Empty);
            row.Selected = true;

            remoteSourceListView.Items.Add(row);

            newSourceAdded = true;
            sourceNameTextBox.Clear();
            sourceUrlTextBox.Clear();
        }

        private void rebuildTemplatesBtn_Click(object sender, EventArgs e)
        {
            templateBuilder.RebuildAllTemplates();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            // Save the new or updated source if textboxes are enabled

            // Save the configuration schedule if changed

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
            remoteSourceListView.Items[IndexSelected].SubItems[1].Text = sourceNameTextBox.Text;
        }

        public void SourceURL_TextChanged(object sender, EventArgs e)
        {
            string url = sourceUrlTextBox.Text;
            if ((url.StartsWith("http")) || (url.StartsWith("https")) || (url.StartsWith("git")))
            {
                sourceBranchTextBox.Enabled = true;
            }

            remoteSourceListView.Items[IndexSelected].SubItems[1].Text = url;
        }

        public void SourceBranch_TextChanged(object sender, EventArgs e)
        {

        }

        public void SourcesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            sourceNameTextBox.Text = e.Item.SubItems[1].Text;            
            sourceUrlTextBox.Text = e.Item.SubItems[2].Text;

            IndexSelected = e.ItemIndex;

            if (e.Item.SubItems[3].Text != "")
            {
                sourceBranchTextBox.Enabled = true;
                sourceBranchTextBox.Text = e.Item.SubItems[3].Text;
            }
        }

        public int IndexSelected { get; set; }
    }
}
