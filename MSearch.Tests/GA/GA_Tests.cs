using System;
using System.Collections.Generic;
using System.Linq;
using MSearch.Tests.Problems.Knapsacks;
using MSearch.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSearch.GA;
using MSearch.Tests.Problems.Sphere;

namespace MSearch.Tests.GA
{
    [TestClass]
    public class GA_Tests: Knapsack
    {
        [TestMethod]
        public void Test_That_Knapsack_Genetic_Algorithm_Works()
        {
            this.Load(Constants.SAMPLE_MKNAPCB4_DATASET);
            Console.WriteLine($"Goal:\t{this.goal}");
            GeneticAlgorithm<List<int>> ga = new GeneticAlgorithm<List<int>>((List<int> sol1, List<int> sol2) => {
                return CrossOver.CutAndSplice<int>(sol1.AsEnumerable(), sol2.AsEnumerable())[0].ToList();
            });
            ga.create(this.getConfiguration());
            List<int> finalSolution = ga.fullIteration();
        }

        [TestMethod]
        public void Test_That_Genetic_Algorithm_On_Sphere_Works()
        {
            GeneticAlgorithm<double[]> garden = new GeneticAlgorithm<double[]>((double[] sol1, double[] sol2) => {
                return CrossOver.CutAndSplice<double>(sol1, sol2)[0].ToArray();
            });
            Sphere sphere = new Sphere();
            garden.create(sphere.getConfiguration());
            garden.fullIteration();
        }
    }
}
