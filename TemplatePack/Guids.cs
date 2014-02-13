// Guids.cs
// MUST match guids.h
using System;

namespace TemplatePack
{
    static class GuidList
    {
        public const string guidTemplatePackPkgString = "e6e2a48e-387d-4af2-9072-86a5276da6d4";
        public const string guidTemplatePackCmdSetString = "a94bef1a-053e-4066-a851-16e5f6c915f1";

        public static readonly Guid guidTemplatePackCmdSet = new Guid(guidTemplatePackCmdSetString);
    };
}