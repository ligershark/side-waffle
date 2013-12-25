// Guids.cs
// MUST match guids.h
using System;

namespace TemplatePack
{
    static class GuidList
    {
        public const string guidTemplatePackPkgString = "7e87d7d9-4413-418f-b9a4-c13da2e6d2e7";
        public const string guidTemplatePackCmdSetString = "2af7d08b-5c62-432c-89b0-c9fdb64f7c19";

        public static readonly Guid guidTemplatePackCmdSet = new Guid(guidTemplatePackCmdSetString);
    };
}