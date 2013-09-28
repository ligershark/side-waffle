// Guids.cs
// MUST match guids.h
using System;

namespace $rootnamespace$
{
    static class GuidList
    {
        public const string guid$rootnamespace$PkgString = "$guid1$";
        public const string guid$rootnamespace$CmdSetString = "$guid2$";

        public static readonly Guid guid$rootnamespace$CmdSet = new Guid(guid$rootnamespace$CmdSetString);
    };
}