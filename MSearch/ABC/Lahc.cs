using System;
using System.Collections.Generic;
using MSearch.Extensions;
using MSearch.Common;

namespace MSearch.ABC
{
    public class LAHC
    {
        public List<double> Table { get; set; }
        public int I { get; set; }
        public Search.Direction Movement { get; set; }
        private double defaultTableValue { get; set; }

        public LAHC()
        {
            this.Table = new List<double>();
        }

        public LAHC(int _tableSize = 2, double defaultTableValue = 0, Search.Direction movement = Search.Direction.Optimization)
        {
            this.Table = (new List<double>()).Fill(_tableSize, defaultTableValue);
            if (movement == Search.Direction.Optimization) defaultTableValue = Double.MaxValue;
            else defaultTableValue = Double.MinValue;
        }

        public bool Update(double fitness)
        {
            int v = I % Table.Count;
            bool isBetter = false;
            if (Movement == Search.Direction.Optimization) isBetter = (fitness < Table[v]);
            else isBetter = (fitness > Table[v]);
            if (isBetter)
            {
                Table[v] = fitness;
                I++;
            }
            return isBetter;
        }
    }
}
