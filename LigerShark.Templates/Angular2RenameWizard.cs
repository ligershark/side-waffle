using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;

namespace LigerShark.Templates
{
    /// <summary>
    /// Custom wizard used by the Angular2 item templates to produce proper names
    /// </summary>
    public class Angular2RenameWizard : Component, IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                var safeItemName = replacementsDictionary["$safeitemname$"];
                var properName = safeItemName;

                if (safeItemName.Contains("."))
                {
                    var indexOfPeriod = safeItemName.IndexOf(".", StringComparison.CurrentCulture);
                    properName = safeItemName.Remove(indexOfPeriod);
                }

                properName = FromCamelCase(properName);

                replacementsDictionary.Add("$properName$", properName);
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

        private string FromCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            if (char.IsUpper(value[0]))
            {
                return value;
            }

            var chArray = value.ToCharArray();

            if (value.Length < 3)
            {
                chArray[0] = char.ToUpper(chArray[0], CultureInfo.InvariantCulture);
            }
            else
            {

                if (char.IsUpper(chArray[2]))
                {
                    for (var i = 0; i < 2; i++)
                    {
                        chArray[i] = char.ToUpper(chArray[i], CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    chArray[0] = char.ToUpper(chArray[0], CultureInfo.InvariantCulture);
                }
            }
            return new string(chArray);
        }

        private void LogError(string message)
        {
            try
            {
                IVsActivityLog _log = GetService(typeof(SVsActivityLog)) as IVsActivityLog;

                _log.LogEntry(
                (UInt32)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR,
                this.ToString(),
                string.Format(CultureInfo.CurrentCulture, "{0}", message));
            }
            catch (Exception)
            {
                // there was likely an error getting the activity service, ignore it so it won't throw
            }
        }
    }
}
