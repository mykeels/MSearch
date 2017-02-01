using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Extensions;
using MSearch.Tests.Common;
using MSearch.Tests.Helpers.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSearch.Tests.Problems
{
    [TestClass]
    public class Knapsack_Tests
    {
        private void saveKnapsacksToFile(List<Knapsack> knapsacks, string folderName)
        {
            Directory.Create("~/data");
            Directory.Create("~/data/knapsacks");
            Directory.Create("~/data/knapsacks/json");
            Directory.Create($"~/data/knapsacks/json/{folderName}");
            for (int i = 0; i < knapsacks.Count; i++)
            {
                var knapsack = knapsacks[i];
                knapsack.ToJson(true).SaveToFile($"data/knapsacks/json/{folderName}/{folderName}-{i + 1}.json");
            }
        }

        [TestMethod]
        public void Test_That_Read_Problem_2_ON_MKNAPCB1_DATASET_Works()
        {
            Knapsack_Problem knapsackProblem = new Knapsack_Problem();
            List<Knapsack> ret = knapsackProblem.readProblemTypeTwo(Constants.MKNAPCB1_DATASET_FILE);
            Console.WriteLine($"No of Knapsacks: {ret.Count}");
            Assert.AreEqual(ret.Count, 30, "No. of Knapsacks must equal 30");
            Console.WriteLine(ret.ToJson(true));
            //saveKnapsacksToFile(ret, Constants.MKNAPCB1_DATASET);
        }

        [TestMethod]
        public void Test_That_Read_Problem_2_ON_MKNAPCB4_DATASET_Works()
        {
            Knapsack_Problem knapsackProblem = new Knapsack_Problem();
            List<Knapsack> ret = knapsackProblem.readProblemTypeTwo(Constants.MKNAPCB4_DATASET_FILE);
            Console.WriteLine($"No of Knapsacks: {ret.Count}");
            Assert.AreEqual(ret.Count, 30, "No. of Knapsacks must equal 30");
            Console.WriteLine(ret.ToJson(true));
            //saveKnapsacksToFile(ret, Constants.MKNAPCB4_DATASET);
        }
    }
}
