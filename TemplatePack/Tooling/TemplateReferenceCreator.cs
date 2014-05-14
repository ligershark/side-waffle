using System.IO;
using EnvDTE;
using System;
using Microsoft.Build.Construction;
using System.Collections.Generic;


namespace TemplatePack {
    class TemplateReferenceCreator {
        public void AddTemplateReference(EnvDTE.Project currentProject, EnvDTE.Project selectedProject) {           
            // we need to compute a relative path for the template project
            Uri currentProjUri = new Uri(currentProject.FullName);
            Uri selectedProjUri = new Uri(selectedProject.FullName);
            string relativePath = currentProjUri.MakeRelativeUri(selectedProjUri).ToString();

            FileInfo selectedProjFile = new FileInfo(selectedProject.FullName);

            var curProjObj = ProjectRootElement.Open(currentProject.FullName);
            var item = curProjObj.AddItem("TemplateReference", selectedProjFile.Name, GetTemplateReferenceMetadata(relativePath));

            curProjObj.Save();
        }

        private IEnumerable<KeyValuePair<string, string>> GetTemplateReferenceMetadata(string relativePath) {
            var result = new List<KeyValuePair<string, string>>();
            result.Add(new KeyValuePair<string, string>("PathToProject", relativePath));
            // result.Add(new KeyValuePair<string, string>("Visible", "False"));
            return result;
        }
    }
}
