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
            return string.Join(string.Empty, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList().Shuffle().Take(length));
        }

        public static string Random(int length)
        {
            return string.Join(string.Empty, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToList().Shuffle().Take(length));
        }
    }
}
