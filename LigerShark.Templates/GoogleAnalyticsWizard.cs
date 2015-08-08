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
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }
        // After the project template is created, send template ID to Google
        public void ProjectFinishedGenerating(Project project)
        {
            sendTrackingCode("$TemplateID", "$TemplateName");
        }
        // After the item template is created, send template ID to Google
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            sendTrackingCode("$TemplateID", "$TemplateName");
        }
        public void RunFinished()
        {
        }
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
        }
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
        private void sendTrackingCode(string templateID, string templateName)
        {
            Console.WriteLine("Template Name: " + templateName);
            Console.WriteLine("Template ID: " + templateID);
        }
    }
}