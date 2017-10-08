using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MSearch.Extensions;

namespace MSearch.Tests.Helpers.IO
{
    public class File
    {
        public static string GetFileName(string path)
        {
            char splitter = '/';
            if (path.Contains(@"\")) splitter = '\\';
            string[] parts = path.Split(splitter);
            return parts.LastOrDefault() ?? string.Empty;
        }

        public static string GetExtension(string path)
        {
            return path.Split('.').LastOrDefault() ?? "file";
        }

        public static string PreventNameClash(string fullPath)
        {
            fullPath = Site.MapPath(fullPath);
            if (System.IO.File.Exists(fullPath))
            {
                string folderPart = Directory.GetFolderPart(fullPath);
                string filename = File.GetFileName(fullPath);
                string newFileName = string.Join(".", filename.Split('.').Take(filename.Split('.').Count() - 1)) +
                    "-" + Convert.ToInt32(System.IO.Directory.GetFiles(folderPart).Count((file) =>
                    {
                        file = GetFileName(file);
                        return file.StartsWith(string.Join(".", filename.Split('.').Take(filename.Split('.').Count() - 1)).Split('-').FirstOrDefault());
                    }) + 1) + "." +
                    GetExtension(filename);
                return folderPart.Trim('/') + "/" + newFileName;
            }
            return fullPath;
        }

        public static string CreateFolder(string path)
        {
            path = Site.MapPath(path);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string GetFolderPath(string path)
        {
            char ch = '/';
            if (path.Contains(@"\"))
            {
                ch = '\\';
            }
            char[] separator = new char[] { ch };
            string[] myarray = path.Split(separator);
            if (System.IO.File.Exists(path))
            {
                return string.Join("/", myarray.Take((myarray.Count<string>() - 1)));
            }
            if (System.IO.Directory.Exists(path))
            {
                return path;
            }
            return path;
        }
    }
}
