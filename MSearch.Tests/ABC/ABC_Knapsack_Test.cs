using System;
using System.Collections.Generic;
using MSearch.Tests.Problems.Knapsacks;
using MSearch.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSearch.ABC;

namespace MSearch.Tests.ABC
{
    [TestClass]
    public class ABC_Knapsack_Test: Knapsack
    {
        [TestMethod]
        public void Test_That_Knapsack_ABC_Works(Knapsack k)
        {
            this.Load(Constants.SAMPLE_MKNAPCB4_DATASET);
            Console.WriteLine($"Goal:\t{this.goal}");
            Hive<List<int>, Bee<List<int>>> hive = new Hive<List<int>, Bee<List<int>>>();
            hive.create(k.getConfiguration());
            List<int> finalResult =  hive.fullIteration();
        }
    }
}
