using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Tests.Problems.Knapsacks;
using MSearch.Tests.Problems.Sphere;
using MSearch.HillClimb;
using MSearch.Tests.Common;
using NUnit.Framework;

namespace MSearch.Tests.HillClimb
{
    [TestFixture]
    public class HillClimb_Tests: Knapsack
    {
        [TestCase]
        public void Test_That_Knapsack_Hill_Climbing_Works()
        {
            this.Load(Constants.SAMPLE_MKNAPCB4_DATASET);
            Console.WriteLine($"Goal:\t{this.goal}");
            HillClimb<List<int>> hillClimb = new HillClimb<List<int>>();
            hillClimb.create(this.getConfiguration());
            List<int> finalSolution = hillClimb.fullIteration();
        }

        [TestCase]
        public void Test_That_HillClimb_On_Sphere_Works()
        {
            HillClimb<double[]> hill = new HillClimb<double[]>();
            Sphere sphere = new Sphere();
            hill.create(sphere.getConfiguration());
            hill.fullIteration();
        }
    }
}
