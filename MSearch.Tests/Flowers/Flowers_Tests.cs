using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSearch.Extensions;
using MSearch.Tests.Common;
using MSearch.Flowers;
using MSearch.Tests.Problems.Sphere;
using MSearch.Tests.Problems.Knapsacks;

namespace MSearch.Tests.Flowers
{
    [TestClass]
    public class Flowers_Tests
    {
        [TestMethod]
        public void Test_That_Flower_Pollination_On_Sphere_Works()
        {
            Pollination<double[]> garden = new Pollination<double[]>();
            Sphere sphere = new Sphere();
            garden.create(sphere.getConfiguration());
            garden.fullIteration();
        }
    }
}
