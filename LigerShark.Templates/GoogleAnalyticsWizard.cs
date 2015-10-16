using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LigerShark.Templates
{
    public class GoogleAnalyticsWizard : IWizard
    {
        private string TemplateID { get; set; }
        private string TemplateName { get; set; }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {

        }

        // After the project template is created, send template ID to Google
        public void ProjectFinishedGenerating(Project project)
        {

        }

        // After the item template is created, send template ID to Google
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        public void RunFinished()
        {
            System.Threading.Tasks.Task.Run(async () => {
                await System.Threading.Tasks.Task.Delay(100);

                try
                {
                    TrackTemplate(TemplateID, TemplateName);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.ToString());
                }
            });
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            TemplateName = replacementsDictionary["$TemplateName$"];
            TemplateID = replacementsDictionary["$TemplateID$"];
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        private void TrackTemplate(string templateID, string templateName)
        {
            var result = GetHashString(Environment.UserDomainName + Environment.MachineName);

            GoogleAnalyticsApi tracker = new GoogleAnalyticsApi("UA-62483606-4", result);
            tracker.TrackEvent("template", "add", templateName);
        }

        public string GetHashString(string data)
        {
            var hashType = new MD5CryptoServiceProvider();
            var hashBytes = Encoding.UTF8.GetBytes(data);
            var hash = hashType.ComputeHash(hashBytes);
            
            return Convert.ToBase64String(hash);
        }
    }
}