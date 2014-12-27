namespace LigerShark.Templates {
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.VisualStudio.TemplateWizard;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Html5UpProjectWizard : IWizard {
        private string TempDir { get; set; }
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams) {

            DTE2 dte = automationObject as DTE2;

            // here we need to show the UI for which file to download
            var form = new DownloadZipWindow();
            var result = form.ShowDialog();

            if (result.HasValue && result.Value) {
                // download the file
                string file = form.DownloadedFile;

                TempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                if (!Directory.Exists(TempDir)) {
                    Directory.CreateDirectory(TempDir);
                }

                // unpack the file in temp
                ZipFile.ExtractToDirectory(file, TempDir);
            }
        }
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem) {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project) {
            // standard vs project is now created, let's copy the extracted files
            string sourceDir = TempDir;
            string destDir = new FileInfo(project.FullName).Directory.FullName;

            if (string.IsNullOrEmpty(TempDir) || !Directory.Exists(TempDir) ||
                string.IsNullOrEmpty(destDir) || !Directory.Exists(destDir)) {
                // TODO: issue a warning or something here
                return;
            }

            List<FileInfo> copiedFiles = new DirectoryHelper().DirectoryCopy(sourceDir, destDir, true, false);
            copiedFiles.ForEach(file => {
                project.ProjectItems.AddFromFile(file.FullName);
            });
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
