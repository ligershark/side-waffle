using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;

namespace LigerShark.Templates
{
    public class AngularDirectiveUsageWizard : Component, IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            try
            {
                var safeItemName = replacementsDictionary["$safeitemname$"];
                var directiveUsage = ToDirectiveUsage(safeItemName);

                replacementsDictionary.Add("$directiveUsage$", directiveUsage);
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        private string ToDirectiveUsage(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var originalValueArrray = value.ToCharArray();

            // Initializes the list with the first character in the original value
            var directiveUsage = new List<char>
            {
                 char.ToLower(originalValueArrray[0])
            };

            // Loops through the original value array and finds any upper case character
            // then adds a hyphen and converts the original character to lower case
            for (var i = 1; i < originalValueArrray.Length; i++)
            {
                if (char.IsUpper(originalValueArrray[i]))
                {
                    directiveUsage.Add('-');
                    directiveUsage.Add(char.ToLower(originalValueArrray[i]));
                }
                else
                {
                    directiveUsage.Add(originalValueArrray[i]);
                }
            }

            return new string(directiveUsage.ToArray());
        }


        private void LogError(string message)
        {
            try
            {
                var log = GetService(typeof(SVsActivityLog)) as IVsActivityLog;

                log.LogEntry(
                    (uint)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR,
                    ToString(),
                    string.Format(CultureInfo.CurrentCulture, "{0}", message));
            }
            catch (Exception)
            {
                // there was likely an error getting the activity service, ignore it so it won't throw
            }
        }
    }
}