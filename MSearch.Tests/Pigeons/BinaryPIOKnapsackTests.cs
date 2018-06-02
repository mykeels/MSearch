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

        [TestMethod]
        public void Test_That_Binary_PIO_Works()
        {
            double mapFactor = 0.05;
            double switchProbability = 0.2;
            BinaryKnapsack bKnapsack = new BinaryKnapsack();
            bKnapsack.Load($"Data/Knapsacks/json/mknapcb1/mknapcb1-1.json");
            BinaryPIO<double> pio = new BinaryPIO<double>();
            pio._switchProbability = switchProbability;
            var config = bKnapsack.getConfiguration();
            config.populationSize = 200;
            config.tableSize = 150;
            config.noOfIterations = 1500;
            config.mutationFunction = (double[] sol) =>
            {
                double[] velocity = BinaryPigeon<double>.updateVelocity(Number.Rnd() <= switchProbability ? pio.getLocalBestSolution() : pio.getGlobalBestSolution(), sol, pio.current.velocity,
                    mapFactor, config.noOfIterations);
                return BinaryPigeon<double>.updateLocation(sol, velocity);
            };
            pio.create(config);
            pio.fullIteration();
        }
    }
}
