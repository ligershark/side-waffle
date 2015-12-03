using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;

namespace LigerShark.Templates
{
    public class GoogleAnalyticsWizard : Component, IWizard
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
                    LogError(ex.ToString());
                }
            });
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try {
                TemplateName = replacementsDictionary["$TemplateName$"];
                TemplateID = replacementsDictionary["$TemplateID$"];
            }
            catch(Exception ex) {
                LogError(ex.ToString());
            }
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

        public string GetHashString(string text)
        {
            var hashType = new MD5CryptoServiceProvider();
            var hashBytes = Encoding.UTF8.GetBytes(text);
            var hash = hashType.ComputeHash(hashBytes);

            return BitConverter.ToString(hash).Replace("-", "");
        }

        private void LogError(string message)
        {            
            IVsActivityLog _log = GetService(typeof(SVsActivityLog)) as IVsActivityLog;

            _log.LogEntry(
            (UInt32)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR,
            this.ToString(),
            string.Format(CultureInfo.CurrentCulture, "{0}", message));
        }
    }
}