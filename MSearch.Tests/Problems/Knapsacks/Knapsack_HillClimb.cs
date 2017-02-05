using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.HillClimb;
using MSearch.Tests.Common;
using MSearch.Extensions;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSearch.Tests.Problems.Knapsacks
{
    [TestClass]
    public class Knapsack_HillClimb
    {
        [TestMethod]
        public void Test_That_Knapsack_Hill_Climbing_Works()
        {
            Knapsack knapsack = new Knapsack();
            knapsack.Load(Constants.SAMPLE_MKNAPCB4_DATASET);
            HillClimb<List<int>> hillClimb = new HillClimb<List<int>>();
            hillClimb.create(knapsack.getConfiguration());
            List<int> finalSolution = hillClimb.fullIteration();
        }
    }
}
