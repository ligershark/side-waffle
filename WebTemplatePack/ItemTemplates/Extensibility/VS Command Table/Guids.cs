// Guids.cs
// MUST match guids.h
using System;

namespace $rootnamespace$
{
    static class GuidList
    {
        public const string guidMyPackagePkgString = "$guid1$";
        public const string guidMyPackageCmdSetString = "$guid2$";

        public static readonly Guid guidMyPackageCmdSet = new Guid(guidMyPackageCmdSetString);
    };
}