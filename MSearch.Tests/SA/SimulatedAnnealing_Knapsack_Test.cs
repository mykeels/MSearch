using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Tests.Problems.Knapsacks;
using MSearch.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSearch.SA;

namespace MSearch.Tests.SA
{
    [TestClass]
    public class SimulatedAnnealing_Knapsack_Test: Knapsack
    {
        [TestMethod]
        public void Test_That_Knapsack_Simulated_Annealing_Works()
        {
            this.Load(Constants.SAMPLE_MKNAPCB4_DATASET);
            Console.WriteLine($"Goal:\t{this.goal}");
            SimulatedAnnealing<List<int>> sa = new SimulatedAnnealing<List<int>>();
            sa.create(this.getConfiguration());
            List<int> finalSolution = sa.fullIteration();
        }
    }
}
