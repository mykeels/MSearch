using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Extensions
{
    public class StringX
    {
        public static string RandomLetters(int length)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Shuffle().First(length);
        }

        public static string Random(int length)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Shuffle().First(length);
        }
    }
}
