using Microsoft.AspNet.Scaffolding;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomScaffolder
{
    [Export(typeof(CodeGeneratorFactory))]
    public class CusomCodeGeneratorFactory : CodeGeneratorFactory 
    {
        /// <summary>
        ///  Information about the code generator goes here.
        /// </summary>
        private static CodeGeneratorInformation _info = new CodeGeneratorInformation(
            displayName: "Custom Scaffolder",
            description: "This is a custom scaffolder.",
            author: "Microsoft",
            version: new Version(1, 0, 0, 0),
            id: typeof(CustomCodeGenerator).Name,
            icon: ToImageSource(Resources._TemplateIconSample),
            gestures: new[] {"Controller", "View", "Area"},
            categories: new[] {Categories.Common, Categories.MvcController,Categories.Other});

        public CusomCodeGeneratorFactory()
            : base(_info)
        {
        }
        /// <summary>
        /// This method creates the code generator instance.
        /// </summary>
        /// <param name="context">The context has details on current active project, project item selected, Nuget packages that are applicable and service provider.</param>
        /// <returns>Instance of CodeGenerator.</returns>
        public override ICodeGenerator CreateInstance(CodeGenerationContext context)
        {
            return new CustomCodeGenerator(context, Information);
        }

       /// <summary>
       /// Any check required for your custom scaffolder goes here.
       /// </summary>
       /// <param name="codeGenerationContext"></param>
       /// <returns></returns>
        public override bool IsSupported(CodeGenerationContext codeGenerationContext)
        {
            if(codeGenerationContext.ActiveProject.CodeModel.Language != EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp)
            {
                return false;
            }
            FrameworkName targetFramework = codeGenerationContext.ActiveProject.GetTargetFramework();
            return (targetFramework != null) &&
                    String.Equals(".NetFramework", targetFramework.Identifier, StringComparison.OrdinalIgnoreCase) &&
                    targetFramework.Version >= new Version(4, 5);
        }
        /// <summary>
        /// Helper method to convert Icon to Imagesource.
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <returns>Imagesource</returns>
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
