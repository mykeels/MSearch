using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Tests.Problems.Knapsacks;
using MSearch.Tests.Common;
using Xunit;
using MSearch.Tests.Problems.Sphere;
using MSearch.SA;

namespace MSearch.Tests.SA
{
    public class SimulatedAnnealing_Tests: Knapsack
    {
        [Fact]
        public void Test_That_Knapsack_Simulated_Annealing_Works()
        {
            this.Load(Constants.SAMPLE_MKNAPCB4_DATASET);
            Console.WriteLine($"Goal:\t{this.goal}");
            SimulatedAnnealing<List<int>> sa = new SimulatedAnnealing<List<int>>();
            sa.create(this.getConfiguration());
            List<int> finalSolution = sa.fullIteration();
        }

        [Fact]
        public void Test_That_Simulated_Annealing_On_Sphere_Works()
        {
            SimulatedAnnealing<double[]> garden = new SimulatedAnnealing<double[]>();
            Sphere sphere = new Sphere();
            garden.create(sphere.getConfiguration());
            garden.fullIteration();
        }
    }
}
