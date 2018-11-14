using System.IO;
using System.Collections.Generic;

namespace LiteGuiDemo
{
    internal static class FileHelper
    {
        private static readonly List<string> SearchPath_ = new List<string>();

        internal static string UnifyPathSeparator(string Path)
        {
            return Path.Replace('\\', '/');
        }

        internal static void AddSearchPath(string Path)
        {
            SearchPath_.Add(UnifyPathSeparator(Path));
        }

        internal static string Combine(string Path1, string Path2)
        {
            Path1 = UnifyPathSeparator(Path1);
            Path2 = UnifyPathSeparator(Path2);

            return Path1.EndsWith("/") ? $"{Path1}{Path2}" : $"{Path1}/{Path2}";
        }

        internal static string GetFileFullPath(string FileName)
        {
            foreach (var Path in SearchPath_)
            {
                var FullPath = Combine(Path, FileName);
                if (File.Exists(FullPath))
                {
                    return FullPath;
                }
            }

            return FileName;
        }
    }
}