// Guids.cs
// MUST match guids.h
using System;

namespace $safeprojectname$
{
    static class GuidList
    {
        public const string guidBrowserLinkExtensionTemplatePkgString = "$guid1$";
        public const string guidBrowserLinkExtensionTemplateCmdSetString = "$guid2$";

        public static readonly Guid guidBrowserLinkExtensionTemplateCmdSet = new Guid(guidBrowserLinkExtensionTemplateCmdSetString);
    };
}