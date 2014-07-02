namespace SideWaffle.Common {
    using Microsoft.VisualStudio.TemplateWizard;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Html5UpProjectWizard : IWizard {

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams) {
            // here we need to show the UI for which file to download
            var form = new DownloadZipWindow();
            var result = form.ShowDialog();

            if (result.HasValue && result.Value) {
                // download the file
                string file = form.DownloadedFile;

                string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                if (!Directory.Exists(tempDir)) {
                    Directory.CreateDirectory(tempDir);
                }
                
                // unpack the file in temp
                ZipFile.ExtractToDirectory(file, tempDir);

                // copy the files to the project directory
            }

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
