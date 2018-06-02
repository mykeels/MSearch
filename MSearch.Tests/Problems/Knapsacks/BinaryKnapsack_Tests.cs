using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Extensions;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSearch.Tests.Problems.Knapsacks
{
    [TestClass]
    public class BinaryKnapsack_Tests
    {
        [TestMethod]
        public void Test_That_Binary_Knapsack_GetInitial_Solution_Works()
        {
            BinaryKnapsack bKnapsack = new BinaryKnapsack();
            for (int i = 1; i <= 5; i++)
            {
                bKnapsack.Load($"Data/Knapsacks/json/mknapcb1/mknapcb1-{i}.json");
                double[] sol = bKnapsack.getInitialSolution();
                Console.WriteLine($"{i}\tSolution: " + JsonConvert.SerializeObject(sol));
                double fitness = bKnapsack.getFitness(sol);
                Console.WriteLine($"{i}\tFitness: " + fitness);
            }
        }

        [TestMethod]
        public void Test_That_Binary_Knapsack_ToKnapsackList_Works()
        {
            BinaryKnapsack bKnapsack = new BinaryKnapsack();
            for (int i = 1; i <= 5; i++)
            {
                bKnapsack.Load($"Data/Knapsacks/json/mknapcb1/mknapcb1-{i}.json");
                double[] sol = bKnapsack.getInitialSolution();
                Console.WriteLine($"{i}\tSolution: " + JsonConvert.SerializeObject(sol));
                Console.WriteLine($"{i}\tKnapsack_List: " + JsonConvert.SerializeObject(bKnapsack.toKnapsackList(sol)));
                Console.WriteLine($"{i}\tFitness: " + bKnapsack.getFitness(sol));
            }
        }
    }
}
