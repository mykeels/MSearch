using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Extensions
{
    public static class NumberExtensions
    {

        private static int seed = DateTime.Now.Millisecond;
        static Random rand = new Random(DateTime.Now.Millisecond);

        private static double Rnd()
        {
            return rand.NextDouble();
        }

        public static string Pad(this int i, int count)
        {
            string ret = "";
            for (int index = 0; index <= count - i.ToString().Length - 1; index++)
            {
                ret += "0";
            }
            ret += i;
            return ret;
        }

        public static int ToInteger(this string s)
        {
            int res = 0;
            if (int.TryParse(s, out res))
            {
                return res;
            }
            else
            {
                return 0;
            }
        }

        public static bool IsNumber(this object value)
        {
            if (value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal)
                return true;
            else if (value is string)
            {
                int x = 0;
                return int.TryParse((string)value, out x);
            }
            return false;
        }

    }
}
