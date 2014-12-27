using Microsoft.Html.Core;
using Microsoft.Html.Editor.SmartTags;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.Web.Editor;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace $rootnamespace$
{
    [Export(typeof(IHtmlSmartTagProvider))]
    [ContentType(HtmlContentTypeDefinition.HtmlContentType)]
    [Order(Before = "Default")]
    [Name("AddClassSmartTagProvider")]
    internal class $safeitemname$SmartTagProvider : IHtmlSmartTagProvider
    {
        public IHtmlSmartTag TryCreateSmartTag(ITextView textView, ITextBuffer textBuffer, ElementNode element, AttributeNode attribute, int caretPosition, HtmlPositionType positionType)
        {
            if (element.GetAttribute("class") == null)
            {
                return new $safeitemname$SmartTag(textView, textBuffer, element);
            }

            return null;
        }
    }

    internal class $safeitemname$SmartTag : HtmlSmartTag
    {
        public $safeitemname$SmartTag(ITextView textView, ITextBuffer textBuffer, ElementNode element)
            : base(textView, textBuffer, element, HtmlSmartTagPosition.ElementName)
        { }

        protected override IEnumerable<ISmartTagAction> GetSmartTagActions(ITrackingSpan span)
        {
            yield return new $safeitemname$TagAction(this);
        }

        class $safeitemname$TagAction : HtmlSmartTagAction
        {
            public $safeitemname$TagAction(HtmlSmartTag htmlSmartTag) :
                base(htmlSmartTag, "Add class")
            { }

            public override void Invoke()
            {
                var element = this.HtmlSmartTag.Element;
                var textBuffer = this.HtmlSmartTag.TextBuffer;

                textBuffer.Insert(element.NameRange.End, " class=\"foo\"");
            }
        }
    }
}