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

                // copy the files to the project directory
                var foo = "bar";
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

            DirectoryCopy(sourceDir, destDir, true);
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem) {
        }

        public void RunFinished() {
        }

        public bool ShouldAddProjectItem(string filePath) {
            return true;
        }

        // taken from http://msdn.microsoft.com/en-us/library/bb762914.aspx
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists) {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName)) {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
