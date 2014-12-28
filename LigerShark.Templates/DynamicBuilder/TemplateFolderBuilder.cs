namespace LigerShark.Templates.DynamicBuilder {
    using Microsoft.Build.BuildEngine;
    using Microsoft.Build.Evaluation;
    using Microsoft.Build.Execution;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TemplateFolderBuilder {
        private string TemplateBuilderInstallFolder { get; set; }
        private string TemplateSourceRoot { get; set; }
        private string ProjectTemplateRoot { get; set; }
        private string TemplateReferenceSourceRoot { get; set; }
        private string ItemTemplateSourceRoot { get; set; }
        private string BaseIntermediateOutputPath { get; set; }
        private string OutputPath { get; set; }

        public TemplateFolderBuilder(
            string templateBuilderInstallFolder,
            string templateSourceRoot,
            string templateReferenceSourceRoot,
            string projectTemplateRoot, 
            string itemTemplateSourceRoot,
            string baseIntermediateOutputPath,
            string outputPath) {
            if (string.IsNullOrEmpty(templateBuilderInstallFolder)) { throw new ArgumentNullException("templateBuilderInstallFolder"); }
            if (string.IsNullOrEmpty(templateSourceRoot)) { throw new ArgumentNullException("templateSourceRoot"); }            
            if (string.IsNullOrEmpty(baseIntermediateOutputPath)) { throw new ArgumentNullException("baseIntermediateOutputPath"); }
            if (string.IsNullOrEmpty(outputPath)) { throw new ArgumentNullException("outputPath"); }

            this.TemplateBuilderInstallFolder = templateBuilderInstallFolder;
            this.TemplateSourceRoot = templateSourceRoot;
            this.TemplateReferenceSourceRoot = templateReferenceSourceRoot;
            this.ProjectTemplateRoot = projectTemplateRoot;
            this.ItemTemplateSourceRoot = itemTemplateSourceRoot;
            this.BaseIntermediateOutputPath = baseIntermediateOutputPath;
            this.OutputPath = outputPath;
        }

        public bool BuildTemplates() {          
            Dictionary<string, string> buildProperties = new Dictionary<string, string>();
            buildProperties.Add("ls-TemplateSubFolder", "SideWaffle");
            buildProperties.Add("TemplateSourceRoot", TemplateSourceRoot);
            buildProperties.Add("ls-TemplateReferenceRoot", TemplateReferenceSourceRoot);
            buildProperties.Add("ls-ProjectTemplateRoot", ProjectTemplateRoot);
            buildProperties.Add("ls-ItemTemplateRoot", ItemTemplateSourceRoot);
            buildProperties.Add("BaseIntermediateOutputPath", BaseIntermediateOutputPath);            
            buildProperties.Add("OutputPath", OutputPath);

            string pathToBuildScript = Path.Combine(this.TemplateBuilderInstallFolder, @"tools\build-templates.proj");
            if (!File.Exists(pathToBuildScript)) {
                throw new FileNotFoundException(string.Format("Unable to file templatebuilder folder build script at [{0}]",pathToBuildScript));
            }

            return BuildProjectWithProperties(pathToBuildScript, buildProperties);
        }
        protected bool BuildProjectWithProperties(string projectFilepath,Dictionary<string,string>globalProperties) {        
            try {
                var pc = new ProjectCollection(globalProperties);

                var logger = new InmemoryMsbuildLogger();
                logger.Verbosity = Microsoft.Build.Framework.LoggerVerbosity.Detailed;

                pc.RegisterLogger(logger);

                var project = pc.LoadProject(projectFilepath);
                var projectInstance = project.CreateProjectInstance();

                string[] targets = new string[] { "BuildStandAlone" };
                var buildReqData = new BuildRequestData(projectInstance, targets, null, BuildRequestDataFlags.ProvideProjectStateAfterBuild);
                var buildResult = BuildManager.DefaultBuildManager.Build(
                    new BuildParameters(pc),
                    buildReqData);

                // TODO: the log does not have much data
                string log = logger.GetLog();

                return (buildResult.OverallResult == BuildResultCode.Success);
            }
            catch (Exception ex) {
                string message = string.Format("Unable to build templates from folder", ex.ToString());
                System.Windows.MessageBox.Show(message);

                throw ex;
            }             
        }
    }
}
