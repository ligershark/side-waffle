namespace TemplatePack.Tooling {
    using EnvDTE80;
    using LibGit2Sharp;
    using LigerShark.Templates;
    using LigerShark.Templates.DynamicBuilder;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Threading;

    public class DynamicTemplateBuilder {
        public string BaseIntermediateOutputPath { get; set; }
        public string OutputPath { get; set; }
        public string RootDirectory { get; set; }
        public string SourceRoot { get; set; }
        private int UpdatePeriod { get; set; }
        private DTE2 Dte { get; set; }
        private ActivityLogger ActivityLog { get; set; }
        
        /// <summary>
        /// If dte is null it will be ignored
        /// If activityLogger is null it will be ignored.
        /// </summary>
        /// <param name="dte"></param>
        public DynamicTemplateBuilder(DTE2 dte, ActivityLogger activityLogger) {
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
            this.Dte = dte;
            this.ActivityLog = activityLogger;
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

        protected string UpdateLogFilePath {
            get {
                return Path.Combine(SideWaffleInstallDir, "UpdateLog.txt");
            }
        }

        protected string TemplateInstallLogFilePath {
            get {
                return Path.Combine(SideWaffleInstallDir, "TemplateInstallLog.txt");
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
                        "Unsupported scheme [{0}] in template source Uri [{1}]",
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
                    ResetDirectoryAttributes(destDirInfo);
                    // TODO: if the folder exists and there is a .git folder then we should do a fetch/merge or pull
                    destDirInfo.Delete(true);
                }

                // clone it
                var repoPath = Repository.Clone(source.Location.AbsoluteUri, destFolder);
                var repo = new Repository(repoPath);
                Branch branch = repo.Checkout(source.Branch);
                branch.Checkout();
            }
            catch (Exception ex) {
                UpdateStatusBar("There was an error check the activity log");
                // TODO: we should log this error
                string msg = ex.ToString();
                System.Windows.Forms.MessageBox.Show(msg);
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

            if (CheckIfAlreadyBuildingSources())
            {
                UpdateStatusBar("Updating project and item templates");
                CreateTemplateBuilderBinIfNotExists();

                var settings = GetTemplateSettingsFromJson();
                var templateLocalInfo = GetLocalInfoFor(settings.Sources);

                // see if the source exists locally, if not then get it
                foreach (var template in templateLocalInfo)
                {
                    if (!Directory.Exists(template.TemplateSourceRoot) && template.Source.Enabled)
                    {
                        FetchSourceLocally(template.Source, template.TemplateSourceRoot);
                        BuildTemplate(template);
                        CopyTemplatesToExtensionsFolder(template);
                        TouchUpgradeLog();
                        TouchInstallLog();
                    }
                    else if (Directory.Exists(template.TemplateSourceRoot) && template.Source.Enabled)
                    {
                        if (CheckIfTimeToUpdateSources())
                        {
                            FetchSourceLocally(template.Source, template.TemplateSourceRoot);
                            BuildTemplate(template);
                            CopyTemplatesToExtensionsFolder(template);
                            TouchUpgradeLog();
                            TouchInstallLog();
                        }
                    }
                }

                UpdateStatusBar("Finished updating project and item templates");
            }
        }

        public bool CheckIfTimeToUpdateSources()
        {
            if (File.Exists(TemplateInstallLogFilePath))
            {
                // Get the amount of time that has passed since we last updated
                DateTime lastWrite = File.GetLastWriteTimeUtc(TemplateInstallLogFilePath);
                DateTime today = DateTime.UtcNow;
                double elapsedTime = (double)today.Subtract(lastWrite).TotalDays;
                string updateFrequency = GetTemplateSettingsFromJson().UpdateInterval.ToString();

                switch (updateFrequency)
                {
                    case "Always":
                        UpdatePeriod = 0;
                        break;
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
            else
            {
                // create the file and return true
                using (File.Create(this.UpdateLogFilePath)) {
                    // nothing to write
                }
                return true;
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
            var results = RemoteTemplateSettings.ReadFromJson(Path.Combine(this.RootDirectory, "templatesources.json"));

            if (results == null || results.Sources == null || results.Sources.Count <= 0) {
                results = new RemoteTemplateSettings {
                    UpdateInterval = UpdateFrequency.OnceAWeek,
                    Sources = new List<TemplateSource>{
                        new TemplateSource{
                            Name="sidewaffleremote",
                            Location = new Uri(@"https://github.com/ligershark/side-waffle.git"),
                            Branch="origin/autoupdate",
                            Enabled=false }}
                };
            }

            return results;
        }

        public RemoteTemplateSettings GetDefaultJsonSettings()
        {
            var results = new RemoteTemplateSettings
            {
                UpdateInterval = UpdateFrequency.OnceAWeek,
                Sources = new List<TemplateSource>{
                    new TemplateSource{
                        Name="sidewaffleremote",
                        Location = new Uri(@"https://github.com/ligershark/side-waffle.git"),
                        Branch="origin/autoupdate",
                        Enabled = false },
                    new TemplateSource{
                        Name="contoso",
                        Location = new Uri(@"https://github.com/sayedihashimi/contoso-templatepack.git"),
                        Branch="origin/autoupdate",
                        Enabled = false}}
            };

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
        private void TouchUpgradeLog() {
            if (!File.Exists(this.UpdateLogFilePath)) {
                using (File.Create(this.UpdateLogFilePath)) {
                    // nothing to write
                }
            }

            try {
                System.IO.File.SetLastWriteTimeUtc(this.UpdateLogFilePath, DateTime.UtcNow);
            }
            catch (IOException iex) {
                // ignore since this is not critical
                LogError(iex.ToString());
            }
        }

        private void TouchInstallLog()
        {
            if (!File.Exists(this.TemplateInstallLogFilePath))
            {
                using (File.Create(this.TemplateInstallLogFilePath))
                {
                    // nothing to write
                }
            }

            try
            {
                System.IO.File.SetLastWriteTimeUtc(this.TemplateInstallLogFilePath, DateTime.UtcNow);
            }
            catch (IOException iex)
            {
                // ignore since this is not critical
                LogError(iex.ToString());
            }
        }
        public void RebuildAllTemplates()
        {
            if (!CheckIfAlreadyBuildingSources())
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
        }

        public bool CheckIfAlreadyBuildingSources()
        {
            // Get the amount of time that has passed since we last updated
            if (File.Exists(UpdateLogFilePath))
            {
                DateTime lastWrite = File.GetLastWriteTimeUtc(UpdateLogFilePath);
                DateTime now = DateTime.UtcNow;
                double elapsedTime = (double)now.Subtract(lastWrite).TotalHours;

                if (elapsedTime <= 1)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
            else
            {
                // Shouldn't ever happen since we are creating the file when VS is launched
                return false;
            }
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
        private void LogError(string message) {
            if (ActivityLog != null) {
                ActivityLog.Error(message);
            }
            else {
                UpdateStatusBar(message);
            }
        }
        private void UpdateStatusBar(string message) {
            if (Dte != null) {
                Dte.StatusBar.Text = message;
            }
            else {
                // not sure what else to do here
                Console.WriteLine(message);
            }
        }
    }
}
