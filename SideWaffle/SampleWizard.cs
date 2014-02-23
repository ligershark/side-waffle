using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideWaffle {
    public class SampleWizard : IWizard {
        // Use to communicate $saferootprojectname$ to ChildWizard     
        public static Dictionary<string, string> GlobalDictionary = new Dictionary<string, string>();
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams) {
            // Place "$saferootprojectname$ in the global dictionary. 
            // Copy from $safeprojectname$ passed in my root vstemplate           
            replacementsDictionary.Add("$datetime$", DateTime.Now.ToShortDateString());
        }

        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem) {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project) {
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem) {
        }

        public void RunFinished() {
        }

        

        public bool ShouldAddProjectItem(string filePath) {
            return true;
        }
    }
}
