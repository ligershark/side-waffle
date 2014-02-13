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

            var curProjObj = ProjectRootElement.Open(currentProject.FullName);
            curProjObj.AddItem("TemplateReference", relativePath, GetTemplateReferenceMetadata());
            curProjObj.Save();

            // TODO: We need a way in VS for the user to remove these TemplateReference items
        }

        private IEnumerable<KeyValuePair<string, string>> GetTemplateReferenceMetadata() {
            var result = new List<KeyValuePair<string, string>>();
            result.Add(new KeyValuePair<string, string>("Visible", "False"));
            return result;
        }
    }
}
