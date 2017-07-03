using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Extensions;
using MSearch.Pigeons;
using MSearch.Tests.Problems.Knapsacks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSearch.Tests.Pigeons
{
    [TestClass]
    public class BinaryPIOKnapsackTests
    {
        private double[] updateVelocity(double[] globalBestSolution, double[] current, double[] velocity, double mapFactor, int noOfIterations)
        {
            List<double> ret = new List<double>();
            double w = Math.Pow(Math.E, -(mapFactor * noOfIterations));
            List<double> fx1 = velocity.Select(v => v * w).ToList();
            List<double> fx2 = new List<double>();
            for (int i = 0; i < Math.Min(globalBestSolution.Length, current.Length); i++)
            {
                int numX = Convert.ToInt32(globalBestSolution[i]) - Convert.ToInt32(current[i]);
                double num = Number.Rnd() * numX;
                fx2.Add(num);
            }
            for (int i = 0; i < Math.Min(fx1.Count, fx2.Count); i++)
            {
                ret.Add(fx1[i] + fx2[i]);
            }
            return ret.ToArray();
        }

        private double[] updateLocation(double[] sol, double[] velocity)
        {
            List<double> ret = new List<double>();
            for (int i = 0; i < Math.Min(sol.Length, velocity.Count()); i++)
            {
                ret.Add(Math.Abs(sol[i] + velocity[i]));
            }
            return ret.ToArray();
        }

        [TestMethod]
        public void Test_That_Binary_PIO_Works()
        {
            BinaryKnapsack bKnapsack = new BinaryKnapsack();
            bKnapsack.Load($"data/knapsacks/json/mknapcb1/mknapcb1-1.json");
            BinaryPIO<double> pio = new BinaryPIO<double>();
            double mapFactor = 0.2;
            var config = bKnapsack.getConfiguration();
            config.populationSize = 15;
            config.tableSize = 3;
            config.noOfIterations = 50000;
            config.mutationFunction = (double[] sol) =>
            {
                double[] velocity = updateVelocity(pio.getLocalBestSolution(), sol, pio.current.velocity, 
                    mapFactor, config.noOfIterations);
                return updateLocation(sol, velocity);
            };
            pio.create(config);
            pio.fullIteration();
        }
    }
}
