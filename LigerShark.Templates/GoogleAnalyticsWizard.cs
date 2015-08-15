using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LigerShark.Templates
{
    public class GoogleAnalyticsWizard : IWizard
    {
        private object TemplateID { get; set; }
        private object TemplateName { get; set; }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {

        }

        // After the project template is created, send template ID to Google
        public void ProjectFinishedGenerating(Project project)
        {
            sendTrackingCode(TemplateID.ToString(), TemplateName.ToString());
        }

        // After the item template is created, send template ID to Google
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            sendTrackingCode(TemplateID.ToString(), TemplateName.ToString());
        }

        public void RunFinished()
        {

        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {

            TemplateName = customParams.GetValue(0);
            //TemplateID = customParams.GetValue(1);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        private void sendTrackingCode(string templateID, string templateName)
        {
            System.Windows.MessageBox.Show("Template Name: " + templateName);
            System.Windows.MessageBox.Show("Template ID: " + templateID);
        }
    }
}