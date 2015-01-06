// Guids.cs
// MUST match guids.h
using System;

namespace LigerShark.MenuOptions
{
    static class GuidList
    {
        public const string guidMenuOptionsPkgString = "ee0cf212-810b-45a1-8c62-e10041913c94";
        public const string guidMenuOptionsCmdSetString = "c75eac28-63cd-4766-adb1-e655471525ea";

        public static readonly Guid guidMenuOptionsCmdSet = new Guid(guidMenuOptionsCmdSetString);
    };
}