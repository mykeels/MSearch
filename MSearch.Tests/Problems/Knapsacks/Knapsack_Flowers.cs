using System;
using System.Collections.Generic;
using MSearch.Flowers;
using MSearch.Tests.Common;
using MSearch.Extensions;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSearch.Tests.Problems.Knapsacks
{
    //[TestClass]
    public class Knapsack_Flowers
    {
        //[TestMethod]
        public void Test_That_Flower_Pollination_Works()
        {
            Knapsack knapsack = new Knapsack();
            knapsack.Load(Constants.SAMPLE_MKNAPCB4_DATASET);
            Pollination<List<int>> garden = new Pollination<List<int>>();
            garden.create(knapsack.getConfiguration());
            List<int> finalSolution = garden.fullIteration();
        }
    }
}
