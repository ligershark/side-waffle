using Microsoft.Html.Core;
using Microsoft.Html.Editor.Validation.Validators;
using Microsoft.Html.Validation;
using Microsoft.VisualStudio.Utilities;
using Microsoft.Web.Editor;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace $rootnamespace$
{
    [Export(typeof(IHtmlElementValidatorProvider))]
    [ContentType(HtmlContentTypeDefinition.HtmlContentType)]
    public class $safeitemname$Provider : BaseHtmlElementValidatorProvider<$safeitemname$>
    { }

    public class $safeitemname$ : BaseValidator
    {
        public override IList<IHtmlValidationError> ValidateElement(ElementNode element)
        {
            var results = new ValidationErrorCollection();
            var classNames = element.GetAttribute("class");

            if (classNames != null && string.IsNullOrEmpty(classNames.Value))
            {
                int index = element.Attributes.IndexOf(classNames);
                string error = "The class attribute must have a value";

                results.AddAttributeError(element, error, HtmlValidationErrorLocation.AttributeValue, index);
            }

            return results;
        }
    }
}
