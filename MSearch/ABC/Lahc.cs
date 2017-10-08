using System;
using System.Collections.Generic;
using MSearch.Extensions;
using MSearch.Common;
using System.Linq;

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
            this.Movement = movement;
            this.Table = (new List<double>());
            for (int i = 1; i <= _tableSize; i++)
            {
                this.Table.Add(defaultTableValue);
            }
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
