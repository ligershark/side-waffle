using System.IO;
using EnvDTE;

namespace TemplatePack
{
    class WebJobCreator
    {
        public void AddReference(Project currentProject, Project selectedProject)
        {
            if (currentProject.Object is VSLangProj.VSProject)
            {
                var project = currentProject.Object as VSLangProj.VSProject;
                project.References.AddProject(selectedProject);
            }
            else if (currentProject.Object is VsWebSite.VSWebSite)
            {
                var project = currentProject.Object as VsWebSite.VSWebSite;
                try
                {
                    project.References.AddFromProject(selectedProject);
                }
                catch
                {
                    // Reference was already added. TODO: don't use try/catch for this.
                }
            }
        }

        public void CreateFolders(Project currentProject, string projectName)
        {
            string dir = GetProjectDirectory(currentProject);
            DirectoryInfo info = new DirectoryInfo(dir)
                .CreateSubdirectory("App_Data\\jobs")
                .CreateSubdirectory(projectName);

            string readmeFile = Path.Combine(info.FullName, "readme.txt");
            AddReadMe(readmeFile);
            currentProject.ProjectItems.AddFromFile(readmeFile);
        }

        private static string GetProjectDirectory(Project project)
        {
            if (project.Object is VSLangProj.VSProject)
                return Path.GetDirectoryName(project.FullName);

            return project.Properties.Item("fullPath").Value.ToString();
        }

        private static void AddReadMe(string destinationFileName)
        {
            string dir = Path.GetDirectoryName(typeof(WebJobCreator).Assembly.Location);
            string readme = Path.Combine(dir, "job", Path.GetFileName(destinationFileName));

            File.Copy(readme, destinationFileName, true);
        }
    }
}
