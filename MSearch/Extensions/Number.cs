using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Extensions
{
    public class Number
    {
        static Random rand = new Random(DateTime.Now.Millisecond);

        public static double Rnd(double multiplicand = 1)
        {
            return rand.NextDouble() * multiplicand;
        }
    }
}
