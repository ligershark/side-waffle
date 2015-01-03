// Guids.cs
// MUST match guids.h
using System;

namespace LigerShark.SideWaffleOptions
{
    static class GuidList
    {
        public const string guidSideWaffleOptionsPkgString = "0c4fc195-8bcd-4f56-a064-7210d9979e54";
        public const string guidSideWaffleOptionsCmdSetString = "50fe588c-aa9a-48a5-9859-2f9507b2ebd4";

        public static readonly Guid guidSideWaffleOptionsCmdSet = new Guid(guidSideWaffleOptionsCmdSetString);
    };
}