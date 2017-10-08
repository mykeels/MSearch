using System;
using System.Collections.Generic;
using System.Linq;

namespace MSearch.Extensions
{
    public static class ListExtensions
    {
        public static Random r = new Random(Convert.ToInt32(DateTime.Now.Millisecond));

        public static T Random<T>(this IEnumerable<T> l)
        {
            //Random r = new Random(Convert.ToInt32(DateTime.Now.Millisecond));
            if (l.Count() == 0)
            {
                return default(T);
            }
            double rr = r.NextDouble() * l.Count();
            return l.ElementAt(Convert.ToInt32(Math.Floor(Convert.ToDouble(rr))));
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> l, int count)
        {
            l = l.ToList();
            Random r = new Random(Convert.ToInt32((new DateTime()).Ticks));
            if (l.Count() == 0 || count < 1)
            {
                return new List<T>();
            }
            List<T> ret = new List<T>();
            for (int i = 0; i < Math.Min(count, l.Count()); i++)
            {
                double rr = r.NextDouble() * l.Count();
                int index = Convert.ToInt32(Math.Floor(Convert.ToDouble(rr)));
                ret.Add(l.ElementAt(index));
                l = l.Except(ret);
            }
            return ret;
        }

        public static List<T> Shuffle<T>(this List<T> arr)
        {
            List<T> ret = new List<T>();
            var count = arr.Count();
            for (int i = 0; i < count; i++)
            {
                T x = arr.Random();
                ret.Add(x);
                arr.Remove(x);
            }
            return ret;
        }
    }
}
