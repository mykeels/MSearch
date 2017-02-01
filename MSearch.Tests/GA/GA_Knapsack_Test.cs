using System;
using System.Collections.Generic;
using System.Linq;
using MSearch.Tests.Problems;
using MSearch.GA;

namespace MSearch.Tests.GA
{
    public class GA_Knapsack_Test: Knapsack
    {
        public List<int> solveWithGeneticAlgorithm()
        {
            GeneticAlgorithm<List<int>> ga = new GeneticAlgorithm<List<int>>((List<int> sol1, List<int> sol2) => {
                return CrossOver.CutAndSplice<int>(sol1.AsEnumerable(), sol2.AsEnumerable())[0].ToList();
            });
            ga.create(this.getConfiguration());
            return ga.fullIteration();
        }
    }
}
