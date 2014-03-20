using CustomScaffolder.UI;
using Microsoft.AspNet.Scaffolding;
using System.Collections.Generic;

namespace CustomScaffolder
{
    public class CustomCodeGenerator : CodeGenerator
    {
        CustomViewModel _viewModel;
 
        /// <summary>
        /// Constructor for the custom code generator
        /// </summary>
        /// <param name="context">Context of the current code generation operation.</param>
        /// <param name="information">Code generation information</param>
        public CustomCodeGenerator(
            CodeGenerationContext context, 
            CodeGeneratorInformation information)
                : base(context, information)
        {
            _viewModel = new CustomViewModel(Context);
        }

        /// <summary>
        /// This method does the code generation.
        /// </summary>
        public override void GenerateCode()
        {
            var codeType = _viewModel.SelectedModelType.CodeType;

            var parameters = new Dictionary<string, object>()
            {
                { 
                    /* This value should match the parameter in T4 */
                    "ModelType", 
                    
                    /* This is the value passed */ 
                    codeType
                }

                //You can pass more parameters after they are defined in the template
            };

            this.AddFileFromTemplate(Context.ActiveProject,
                "CustomCode",
                "CustomTextTemplate",
                parameters,
                skipIfExists: false);
        }

        /// <summary>
        /// Any UI for validation goes here.
        /// </summary>
        /// <returns></returns>
        public override bool ShowUIAndValidate()
        {
            SelectModelWindow window = new SelectModelWindow(_viewModel);
            bool? showDialog = window.ShowDialog();
            return showDialog ?? false;
        }
    }
}
