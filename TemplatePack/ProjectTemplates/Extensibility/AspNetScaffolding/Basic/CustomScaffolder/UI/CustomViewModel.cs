using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace CustomScaffolder.UI
{
    public class CustomViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public CustomViewModel(CodeGenerationContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ModelType> ModelTypes
        {
            get
            {
                ICodeTypeService codeTypeService = (ICodeTypeService)Context
                    .ServiceProvider.GetService(typeof(ICodeTypeService));

                return codeTypeService
                    .GetAllCodeTypes(Context.ActiveProject)
                    .Where(codeType => codeType.IsValidWebProjectEntityType())
                    .Select(codeType => new ModelType(codeType));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ModelType SelectedModelType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CodeGenerationContext Context { get; private set; }
    }
}
