using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Tests.Helpers
{
    public class Site
    {
        public static string MapPath(string path = "")
        {
            if (!path.StartsWith("~/")) return path;
            return path.Replace("~", System.Environment.CurrentDirectory.Replace(@"\", "/"));
        }
    }
}
