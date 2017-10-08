using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSearch.Extensions;

namespace MSearch.Tests.Helpers.IO
{
    public class Directory
    {
        public static DirectoryInfo Create(string path)
        {
            path = Site.MapPath(path);
            if (!System.IO.Directory.Exists(path)) return System.IO.Directory.CreateDirectory(path);
            return new System.IO.DirectoryInfo(path);
        }

        public static string GetFolderPart(string path)
        {
            char splitter = '/';
            if (path.Contains(@"\")) splitter = '\\';
            string[] parts = path.Split(splitter);
            if (System.IO.File.Exists(path)) return string.Join("/", parts.Take(parts.Count() - 1));
            else if (System.IO.Directory.Exists(path)) return path;
            else return string.Join("/", parts.Take(parts.Count() - 1));
        }
    }
}
