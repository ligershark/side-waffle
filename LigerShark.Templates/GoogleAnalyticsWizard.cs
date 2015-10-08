using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;

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
            TrackTemplate(TemplateID, TemplateName);
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
            GoogleAnalyticsApi tracker = new GoogleAnalyticsApi("UA-62483606-3", "1092810121650");
            //tracker.TrackEvent("template", "add", templateName);
            tracker.TrackEvent("Project Template", "Test", templateName);
        }
    }
}