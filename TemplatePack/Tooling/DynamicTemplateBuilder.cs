namespace TemplatePack.Tooling {
    using LigerShark.Templates;
    using LigerShark.Templates.DynamicBuilder;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class DynamicTemplateBuilder {
        public string BaseIntermediateOutputPath { get; set; }
        public string OutputPath { get; set; }

        public string SourceRoot { get; set; }
        
        public DynamicTemplateBuilder() {
            this.SourceRoot = Environment.ExpandEnvironmentVariables(@"%localappdata%\LigerShark\SideWaffle\DynamicTemplates\sources\");
            this.BaseIntermediateOutputPath = Environment.ExpandEnvironmentVariables(@"%localappdata%\LigerShark\SideWaffle\DynamicTemplates\baseintout\");
            this.OutputPath = Environment.ExpandEnvironmentVariables(@"%localappdata%\LigerShark\SideWaffle\DynamicTemplates\output\");
        }

        protected string SideWaffleInstallDir {
            get {
                return (new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName);
            }
        }
        protected string TemplateBuilderBinPath {
            get {
                return Path.Combine(SideWaffleInstallDir, @"TemplateBuilderBin\");
            }
        }

        protected void FetchSourceLocally(TemplateSource source, string destFolder) {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (string.IsNullOrEmpty(destFolder)) { throw new ArgumentNullException("destFolder"); }

            if (string.Compare("file", source.Location.Scheme, StringComparison.InvariantCultureIgnoreCase)==0) {
                // do a recursive copy of the folder to the dest
                new DirectoryHelper().DirectoryCopy(source.Location.LocalPath, destFolder, true);
            }
            else {
                throw new ApplicationException(
                    string.Format(
                        "Unsupported scheme [{0}] in template source uri [{1}]",
                        source.Location.Scheme,
                        source.Location.AbsoluteUri));
            }
        }
        protected void BuildTemplates(TemplateLocalInfo template) {
            if (template == null) { throw new ArgumentNullException("template"); }

            var builder = new TemplateFolderBuilder(
                TemplateBuilderBinPath,
                template.TemplateSourceRoot,
                template.ProjectTemplateSourceRoot,
                template.ItemTemplateSourceRoot,
                Path.Combine(this.BaseIntermediateOutputPath, template.Source.Name) + @"\",
                Path.Combine(this.OutputPath, template.Source.Name) + @"\");

            var result = builder.BuildTemplates();
            if (!result) {
                System.Diagnostics.Trace.TraceError("Unable to build templates from source [{0}]. Unknown error", template.TemplateSourceRoot);
            }
        }

        protected void CopyTemplatesToExtensionsFolder(TemplateLocalInfo template) {
            if (template == null) { throw new ArgumentNullException("template"); }
            // copy the templates from the output directory and into the extensions folder
            string actualBaseIntOut = Path.Combine(this.BaseIntermediateOutputPath, template.Source.Name);
            var templatePath = new DirectoryInfo(Path.Combine(actualBaseIntOut, @"Debug\ls-Templates"));
            if(!templatePath.Exists){
                throw new ApplicationException(string.Format("Unable to find generated templates at [{0}]",templatePath.FullName));
            }

            var templatesToCopy = templatePath.GetFiles(@"*.zip", SearchOption.TopDirectoryOnly);

            // var destFolder = Path.Combine(SideWaffleInstallDir,"Output\")

            foreach (var tToCopy in templatesToCopy) {
                // TODO: Needs to be implemented. MSBuild should copy to OutputPath in the correct folder structure first
            }
        }
        public void ProcessTemplates() {
            CreateTemplateBuilderBinIfNotExists();
            var templateLocalInfo = GetLocalInfoFor(GetTemplateSources());

            // see if the source exists locally, if not then get it
            foreach (var template in templateLocalInfo) {
                if (!Directory.Exists(template.TemplateSourceRoot)) {
                    FetchSourceLocally(template.Source, template.TemplateSourceRoot);
                    BuildTemplates(template);
                    CopyTemplatesToExtensionsFolder(template);
                }
            }

            /*
            // get from GetTemplateBuilderInstallPath
            string templateBuilderInstallPath = @"C:\data\mycode\side-waffle\packages\TemplateBuilder.1.1.2-beta1\";
            // this is %localappdata%\LigerShark\SideWaffle\DynamicTemplates\sources\$sourcename$\
            string templateSourceRoot = @"C:\data\mycode\side-waffle\";
            // this is %localappdata%\LigerShark\SideWaffle\DynamicTemplates\sources\$sourcename$\Project Templates\
            string projectTemplateSourceRoot = @"C:\data\mycode\side-waffle\Project Templates\";
            // this is %localappdata%\LigerShark\SideWaffle\DynamicTemplates\sources\$sourcename$\Item Templates\
            string itemTemplateSourceRoot = @"C:\data\mycode\side-waffle\Item Templates\";
            // either passed in or %localappdata%\LigerShark\SideWaffle\DynamicTemplates\baseintout\$sourcename$\
            string baseIntermediateOutputPath = @"C:\temp\_net\bio\";
            // either passed in or %localappdata%\LigerShark\SideWaffle\DynamicTemplates\output\$sourcename$\
            string outputPath = @"C:\temp\_net\output\";
            var builder = new LigerShark.Templates.DynamicBuilder.TemplateFolderBuilder(
                templateBuilderInstallPath,
                templateSourceRoot,
                projectTemplateSourceRoot,
                itemTemplateSourceRoot,
                baseIntermediateOutputPath,
                outputPath);
            var result = builder.BuildTemplates();
            System.Windows.MessageBox.Show("Done building");
             * */
        }

        public void CreateTemplateBuilderBinIfNotExists() {
            // check to see if the directory is already there if not create it
            if (!Directory.Exists(TemplateBuilderBinPath)) {
                var swDir = new DirectoryInfo(SideWaffleInstallDir);
                var foundFiles = swDir.GetFiles("TemplateBuilder*.nupkg");
                if (foundFiles == null || foundFiles.Length <= 0) {
                    throw new FileNotFoundException(string.Format("Didn't find any files matching TemplateBuilder*.nupkg in [{0}]", swDir.FullName));
                }

                // there should only be 1 match here, but no need to enforce it
                var pkgToExtract = foundFiles[0];

                ZipFile.ExtractToDirectory(pkgToExtract.FullName, TemplateBuilderBinPath);
            }
        }

        private IList<TemplateSource> GetTemplateSources() {
            // TODO: get this from templates.json or from tools->option

            var sources = new List<TemplateSource> {
                //new TemplateSource{Name="sidewaffle",Location = new Uri(@"https://github.com/ligershark/side-waffle.git")}
                
                new TemplateSource{
                    Name="sidewafflelocal",
                    Location = new Uri(@"C:\data\mycode\side-waffle")}
            };
            return sources;
        }

        private IList<TemplateLocalInfo> GetLocalInfoFor(IList<TemplateSource> sources) {
            if (sources == null) { throw new ArgumentNullException("sources"); }

            var result = new List<TemplateLocalInfo>();

            foreach (var source in sources) {
                string srcRoot = Path.Combine(this.SourceRoot,source.Name);
                result.Add(new TemplateLocalInfo(source, Path.Combine(this.SourceRoot, source.Name)+@"\"));
            }

            return result;
        }
    }
    public class TemplateLocalInfo {
        public TemplateLocalInfo(TemplateSource source, string templateSourceRoot, string projectTemplateSourceRoot, string itemTemplateSourceRoot) {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (string.IsNullOrEmpty(templateSourceRoot)) { throw new ArgumentNullException("templateSourceRoot"); }

            this.Source = source;
            this.TemplateSourceRoot = templateSourceRoot;
            this.ProjectTemplateSourceRoot = projectTemplateSourceRoot;
            this.ItemTemplateSourceRoot = itemTemplateSourceRoot;
        }
        public TemplateLocalInfo(TemplateSource source, string templateSourceRoot)
            : this(
                source,
                templateSourceRoot,
                Path.Combine(templateSourceRoot, @"Project Templates\"),
                Path.Combine(templateSourceRoot, @"Item Templates\")) {
        }
        public TemplateSource Source { get; set; }
        public string TemplateSourceRoot { get; set; }
        public string ProjectTemplateSourceRoot { get; set; }
        public string ItemTemplateSourceRoot { get; set; }        
    }
}
