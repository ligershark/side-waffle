// PkgCmdID.cs
// MUST match PkgCmdID.h
using System;

namespace TemplatePack
{
    static class GuidList
    {
        public const string guidTemplatePackPkgString = "e6e2a48e-387d-4af2-9072-86a5276da6d4";
        public const string guidTemplatePackCmdSetString = "a94bef1a-053e-4066-a851-16e5f6c915f1";

        public static readonly Guid guidTemplatePackCmdSet = new Guid(guidTemplatePackCmdSetString);

        // SideWaffle Remote Source Settings
        public const string guidMenuOptionsPkgString = "796B8CBC-3010-4A76-872B-56775129765F";
        public const string guidMenuOptionsCmdSetString = "13EE92AE-B8B5-4728-8AF6-F53D8DD9C391";

        public static readonly Guid guidMenuOptionsCmdSet = new Guid(guidMenuOptionsCmdSetString);
    }

    static class PkgCmdIDList
    {
        public const uint cmdidMyCommand = 0x100;
        public const uint SWMenuGroup = 0x100;
    };
}