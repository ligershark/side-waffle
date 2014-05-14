using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;

namespace TemplatePack
{
    static class SolutionHelper
    {
        public static bool IsWebProject(this Project project)
        {
            if (project.Object is VsWebSite.VSWebSite)
                return true;

            try
            {
                var extenderNames = (object[])project.ExtenderNames;
                return extenderNames.Any(extenderNameObject => extenderNameObject.ToString() == "WebApplication");
            }
            catch
            {
                // Ignore 
            }

            return false;
        }

        public static IEnumerable<Project> GetAllProjects(this Solution solution)
        {
            Projects projects = solution.Projects;
            List<Project> list = new List<Project>();
            var item = projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                    continue;

                if (project.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                    list.AddRange(GetSolutionFolderProjects(project));
                else
                    list.Add(project);
            }

            return list;
        }

        public static IEnumerable<Project> GetAllNonWebProjects(this Solution solution) {
            return GetAllProjects(solution).Where(p => !p.IsWebProject());
        }

        private static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            List<Project> list = new List<Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                    continue;

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == ProjectKinds.vsProjectKindSolutionFolder)
                    list.AddRange(GetSolutionFolderProjects(subProject));
                else
                    list.Add(subProject);
            }

            return list;
        }
    }
}
