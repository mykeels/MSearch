using System;
using System.Collections.Generic;
using System.Linq;
using MSearch.Extensions;
using MSearch.Common;

namespace MSearch.GA
{
    public class CrossOver
    {
        public static IEnumerable<T> OnePoint<T>(IEnumerable<T> sol1, IEnumerable<T> sol2)
        {
            if (sol1 == null || sol2 == null) throw new Exception("None of the Arguments can be null");
            if (sol1.Count() != sol2.Count()) throw new Exception("Element Count in both Arguments must be the same");
            if (sol1.Count() <= 2) throw new Exception("For One Point CrossOver, Enumerable Length must be more than 2");
            int point = Convert.ToInt32(Math.Floor(Number.Rnd() * sol1.Count()));
            List<T> ret = new List<T>();
            for (int i = 0; i < point; i++)
            {
                ret.Add(sol1.ElementAt(i));
            }
            for (int i = point; i < sol2.Count(); i++)
            {
                ret.Add(sol2.ElementAt(i));
            }
            return ret.AsEnumerable();
        }

        public static IEnumerable<T> Uniform<T>(IEnumerable<T> sol1, IEnumerable<T> sol2)
        {
            if (sol1 == null || sol2 == null) throw new Exception("None of the Arguments can be null");
            if (sol1.Count() != sol2.Count()) throw new Exception("Element Count in both Arguments must be the same");
            List<T> ret = new List<T>();
            for (int i = 0; i < sol1.Count(); i++)
            {
                if (Number.Rnd() < 0.5) ret.Add(sol1.ElementAt(i));
                else ret.Add(sol2.ElementAt(i));
            }
            return ret.AsEnumerable();
        }

        public static IEnumerable<T>[] TwoPoint<T>(IEnumerable<T> l1, IEnumerable<T> l2, int cross1, int cross2)
        {
            List<List<T>> ret = new List<List<T>>();
            ret.Add(new List<T>());
            ret.Add(new List<T>());
            if (l1.Count() != l2.Count())
            {
                throw new Exception("Arrays should be of same length");
            }
            if (cross2 > cross1 & cross1 >= 1 & cross2 < l1.Count())
            {
                for (int i = 0; i < cross1; i++)
                {
                    ret[0].Add(l1.ElementAt(i));
                    ret[1].Add(l2.ElementAt(i));
                }
                for (int index = cross1; index < cross2; index++)
                {
                    ret[0].Add(l2.ElementAt(index));
                    ret[1].Add(l1.ElementAt(index));
                }
                for (int i = cross2; i < l2.Count(); i++)
                {
                    ret[0].Add(l1.ElementAt(i));
                    ret[1].Add(l2.ElementAt(i));
                }
            }
            else
            {
                throw new Exception("1st index should be less than 2nd index, 1st index should be greater than 1, 2nd index should be less than array_length - 1");
            }
            return ret.ToArray();
        }

        public static IEnumerable<T>[] AutoTwoPoint<T>(IEnumerable<T> l1, IEnumerable<T> l2)
        {
            List<T> ret = new List<T>();
            if (l1.Count() != l2.Count())
            {
                throw new Exception("Arrays should be of same length");
            }
            line1:

            int a = Convert.ToInt32(Math.Floor(Number.Rnd() * l1.Count() - 1) + 1);
            int b = Convert.ToInt32(a + Math.Floor(Number.Rnd() * (l1.Count() - a)));

            if (a == b || a == 0)
            {
                goto line1;
            }

            return TwoPoint(l1, l2, a, b);
        }

        public static IEnumerable<T>[] CutAndSplice<T>(IEnumerable<T> l1, IEnumerable<T> l2)
        {
            List<List<T>> ret = new List<List<T>>();
            ret.Add(new List<T>());
            ret.Add(new List<T>());
            int cross1 = (int)Math.Floor(Number.Rnd() * l1.Count());
            int cross2 = (int)Math.Floor(Number.Rnd() * l2.Count());

            for (int i = 0; i < cross1; i++)
            {
                ret[0].Add(l1.ElementAt(i));
            }
            for (int i = 0; i < cross2; i++)
            {
                ret[1].Add(l2.ElementAt(i));
            }
            for (int i = cross2; i < l2.Count(); i++)
            {
                ret[0].Add(l2.ElementAt(i));
            }
            for (int i = cross1; i < l1.Count(); i++)
            {
                ret[1].Add(l1.ElementAt(i));
            }
            return ret.ToArray();
        }

    }
}
