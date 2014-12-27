namespace LigerShark.Templates {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DirectoryHelper {
        
        public List<FileInfo> DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) {
            // taken from http://msdn.microsoft.com/en-us/library/bb762914.aspx
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

            var copiedFiles = new List<FileInfo>();
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string temppath = Path.Combine(destDirName, file.Name);
                copiedFiles.Add(file.CopyTo(temppath, false));
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    copiedFiles.AddRange(
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs));
                }
            }

            return copiedFiles;
        }
    }
}
