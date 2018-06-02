using System;
using System.Collections.Generic;
using MSearch.Tests.Problems.Knapsacks;
using MSearch.Tests.Common;
using Newtonsoft.Json;
using MSearch.ABC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSearch.Tests.ABC
{
    [TestClass]
    public class ABC_Tests: Knapsack
    {
        [TestMethod]
        public void Test_That_Knapsack_ABC_Works()
        {
            string filename = Constants.SAMPLE_MKNAPCB4_DATASET;
            this.Load(filename);
            Console.WriteLine($"Goal:\t{this.goal}");
            Hive<List<int>, Bee<List<int>>> hive = new Hive<List<int>, Bee<List<int>>>();
            hive.create(this.getConfiguration());
            List<int> finalResult =  hive.fullIteration();
        }

        [TestMethod]
        /**("Test That FailureLimit and AcceptProbability are defaults when instantiated with default constructor" */
        public void Test_That_FailureLimit_And_AcceptProbability_Are_Defaults()
        {
            Hive<int[], Bee<int[]>> hive = new Hive<int[], Bee<int[]>>();
            Assert.AreEqual(hive.getFailureLimit(), 20);
            Assert.AreEqual(hive.getAcceptanceProbability(), 0.4);
            Console.WriteLine("Hive:\n" + JsonConvert.SerializeObject(hive, Formatting.Indented));
            Console.WriteLine("Hive Failure Limit:\t" + hive.getFailureLimit());
            Console.WriteLine("Hive Acceptance Probability:\t" + hive.getAcceptanceProbability());
        }
    }
}
