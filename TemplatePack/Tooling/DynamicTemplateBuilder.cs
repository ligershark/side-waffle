namespace TemplatePack.Tooling {
    using LibGit2Sharp;
    using LigerShark.Templates;
    using LigerShark.Templates.DynamicBuilder;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;

    public class DynamicTemplateBuilder {
        public string BaseIntermediateOutputPath { get; set; }
        public string OutputPath { get; set; }
        public string RootDirectory { get; set; }
        public string SourceRoot { get; set; }
        private int UpdatePeriod { get; set; }
        
        public DynamicTemplateBuilder() {
            // Note: using extensions install dir causes max path issues
            //this.SourceRoot = Path.Combine(SideWaffleInstallDir, @"DynamicTemplates\sources\");
            //this.BaseIntermediateOutputPath = Path.Combine(SideWaffleInstallDir, @"DynamicTemplates\baseintout\");
            //this.OutputPath = Path.Combine(SideWaffleInstallDir, @"DynamicTemplates\output\");

            var verstr = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version.ToString();
            var rootDir = Environment.ExpandEnvironmentVariables(
                            string.Format(@"%localappdata%\LigerShark\SideWaffle\DynamicTemplates\{0}\", verstr));

            this.RootDirectory = Path.GetFullPath(rootDir);
            this.SourceRoot = Path.Combine(rootDir, @"sources\");
            this.BaseIntermediateOutputPath = Path.Combine(rootDir, @"baseintout\");
            this.OutputPath = Path.Combine(rootDir, @"output\");
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
                new DirectoryHelper().DirectoryCopy(source.Location.LocalPath, destFolder, true, true);
            }
            else if(
                string.Compare("git", source.Location.Scheme, StringComparison.InvariantCultureIgnoreCase)==0 ||
                string.Compare("http", source.Location.Scheme, StringComparison.InvariantCultureIgnoreCase)==0 ||
                string.Compare("https", source.Location.Scheme, StringComparison.InvariantCultureIgnoreCase) == 0) {
                    FetchGitSourceLocally(source, destFolder);
            }
            else {
                throw new ApplicationException(
                    string.Format(
                        "Unsupported scheme [{0}] in template source uri [{1}]",
                        source.Location.Scheme,
                        source.Location.AbsoluteUri));
            }
        }
        protected void FetchGitSourceLocally(TemplateSource source, string destFolder) {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (string.IsNullOrEmpty(destFolder)) { throw new ArgumentNullException("destFolder"); }

            // TODO: these methods should be renamed since fetch means something in git

            try {
                var destDirInfo = new DirectoryInfo(destFolder);
                if (destDirInfo.Exists) {
                    // TODO: if the folder exists and there is a .git folder then we should do a fetch/merge or pull
                    destDirInfo.Delete(true);
                }

                // clone it
                var repoPath = Repository.Clone(source.Location.AbsoluteUri, destFolder);
                var repo = new Repository(repoPath);
                Branch branch = repo.Checkout(source.Branch);
                branch.Checkout();
                // Repository.Clone(source.Location.AbsoluteUri,destFolder,new CloneOptions().br)
            }
            catch (Exception ex) {
                string msg = ex.ToString();
            }
        }
        protected void BuildTemplate(TemplateLocalInfo template) {
            if (template == null) { throw new ArgumentNullException("template"); }

            var builder = new TemplateFolderBuilder(
                TemplateBuilderBinPath,
                template.TemplateSourceRoot,
                template.TemplateReferenceSourceRoot,
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

            var outputPath = Path.Combine(OutputPath, template.Source.Name);
            if (Directory.Exists(outputPath)) {
                new DirectoryHelper().DirectoryCopy(System.IO.Path.Combine(OutputPath, template.Source.Name), SideWaffleInstallDir, true, true);
            }
            else {
                throw new ApplicationException(string.Format(@"Template output not found in [{0}]", outputPath));
            }
        }
        public void ProcessTemplates() {
            CreateTemplateBuilderBinIfNotExists();
     
            var settings = GetTemplateSettingsFromJson();
            var templateLocalInfo = GetLocalInfoFor(settings.Sources);

            // see if the source exists locally, if not then get it
            foreach (var template in templateLocalInfo) {
                if (!Directory.Exists(template.TemplateSourceRoot) && template.Source.Enabled)
                {
                    FetchSourceLocally(template.Source, template.TemplateSourceRoot);
                    BuildTemplate(template);
                    CopyTemplatesToExtensionsFolder(template);

                    // Write to the log file "UpdateLog.txt"
                }

                else if (Directory.Exists(template.TemplateSourceRoot) && template.Source.Enabled)
                {
                    if (CheckIfTimeToUpdateSources())
                    {
                        FetchSourceLocally(template.Source, template.TemplateSourceRoot);
                        BuildTemplate(template);
                        CopyTemplatesToExtensionsFolder(template);

                        // Write to the log file "UpdateLog.txt"
                    }
                }
            }
        }

        public bool CheckIfTimeToUpdateSources()
        {
            String logPath = "UpdateLog.txt";
            if (File.Exists(logPath))
            {
                // Get the amount of time that has passed since we last updated
                DateTime lastWrite = File.GetLastWriteTime(logPath);
                DateTime today = DateTime.Now;
                double elapsedTime = (double)today.Subtract(lastWrite).TotalDays;
                string updateFrequency = GetTemplateSettingsFromJson().UpdateInterval.ToString();

                switch (updateFrequency)
                {
                    case "OnceADay":
                        UpdatePeriod = 1;
                        break;
                    case "OnceAWeek":
                        UpdatePeriod = 7;
                        break;
                    case "OnceAMonth":
                        UpdatePeriod = 30;
                        break;
                    case "Never":
                        UpdatePeriod = 0;
                        break;
                    default:
                        UpdatePeriod = 7;
                        break;
                }
                if (UpdatePeriod == 0)
                {
                    // Always return false because we never want to update
                    return false;
                }
                else
                {
                    // Otherwise we check to see if it is time to update 
                    if (UpdatePeriod == elapsedTime)
                    {
                        return true;
                    }

                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
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

        public RemoteTemplateSettings GetTemplateSettingsFromJson() {
            var results = RemoteTemplateSettings.ReadFromJson(Path.Combine(this.SideWaffleInstallDir, "templatesources.json"));

            if (results == null || results.Sources == null || results.Sources.Count <= 0) {
                results = new RemoteTemplateSettings {
                    UpdateInterval = UpdateFrequency.OnceAWeek,
                    Sources = new List<TemplateSource>{
                        new TemplateSource{
                            Name="sidewaffleremote",
                            Location = new Uri(@"https://github.com/ligershark/side-waffle.git"),
                            Branch="origin/autoupdate" }}
                };
            }

            return results;
        }

        public void WriteJsonTemplateSettings(RemoteTemplateSettings settings)
        {
            var filePath = Path.Combine(this.SideWaffleInstallDir, "templatesources.json");

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(filePath, json);
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

        public void RebuildAllTemplates()
        {
            // Delete the folder where the templates are built (i.e. C:\Users\<Username>\AppData\Local\LigerShark\SideWaffle\DynamicTemplates\<Version>
            if (Directory.Exists(RootDirectory))
            {
                // Reset the attributes for all subdirectories and their files
                DirectoryInfo parentDirectoryInfo = new DirectoryInfo(RootDirectory);
                ResetDirectoryAttributes(parentDirectoryInfo);

                Directory.Delete(RootDirectory, true);
            }

            // Delete the Output folder from the Extension directory
            if (Directory.Exists(OutputPath))
            {
                // Reset the attributes for all subdirectories and their files
                DirectoryInfo parentDirectoryInfo = new DirectoryInfo(OutputPath);
                ResetDirectoryAttributes(parentDirectoryInfo);

                Directory.Delete(OutputPath, true);
            }

            // Download and build the latest templates from their source
            ProcessTemplates();
        }

        private static void ResetDirectoryAttributes(DirectoryInfo parentDirectory)
        {
            if (parentDirectory != null)
            {
                parentDirectory.Attributes = FileAttributes.Normal;
                foreach (FileInfo fi in parentDirectory.GetFiles())
                {
                    fi.Attributes = FileAttributes.Normal;
                }
                foreach (DirectoryInfo di in parentDirectory.GetDirectories())
                {
                    ResetDirectoryAttributes(di);
                }
            }
        }
    }
}
