namespace TemplatePack.Tooling {
    using LigerShark.Templates.DynamicBuilder;
    using System;
    using System.IO;

    public class TemplateLocalInfo {
        public TemplateLocalInfo(TemplateSource source, string templateSourceRoot, string templateReferenceSourceRoot, string projectTemplateSourceRoot, string itemTemplateSourceRoot) {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (string.IsNullOrEmpty(templateSourceRoot)) { throw new ArgumentNullException("templateSourceRoot"); }

            this.Source = source;
            this.TemplateSourceRoot = templateSourceRoot;
            this.TemplateReferenceSourceRoot = templateReferenceSourceRoot;
            this.ProjectTemplateSourceRoot = projectTemplateSourceRoot;
            this.ItemTemplateSourceRoot = itemTemplateSourceRoot;
        }
        public TemplateLocalInfo(TemplateSource source, string templateSourceRoot)
            : this(
                source,
                templateSourceRoot,
                Path.Combine(templateSourceRoot, @"project-templates\"),
                Path.Combine(templateSourceRoot, @"project-templates-v0\"),
                Path.Combine(templateSourceRoot, @"item-Templates\")) {
        }
        public TemplateSource Source { get; set; }
        public string TemplateSourceRoot { get; set; }
        public string ProjectTemplateSourceRoot { get; set; }
        public string TemplateReferenceSourceRoot { get; set; }
        public string ItemTemplateSourceRoot { get; set; }
    }
}
