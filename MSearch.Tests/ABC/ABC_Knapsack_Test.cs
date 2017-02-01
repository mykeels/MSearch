using System;
using System.Collections.Generic;
using MSearch.Tests.Problems;
using MSearch.ABC;

namespace MSearch.Tests.ABC
{
    public class ABC_Knapsack_Test: Knapsack
    {
        public static List<int> solveWithABC(Knapsack k)
        {
            Hive<List<int>, Bee<List<int>>> hive = new Hive<List<int>, Bee<List<int>>>();
            hive.create(k.getConfiguration());
            return hive.fullIteration();
        }
    }
}
