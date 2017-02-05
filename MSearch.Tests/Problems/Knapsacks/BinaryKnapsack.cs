using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Tests.Problems.Knapsacks;
using MSearch.Common;
using MSearch.Tests.Common;
using MSearch.Extensions;

namespace MSearch.Tests.Problems.Knapsacks
{
    public class BinaryKnapsack: Knapsack
    {
        private const double LEVY_JUMP_INDEX = 0.5;
        public double[] clone(double[] sol)
        {
            double[] newSol = new double[sol.Length];
            for (int i = 0; i < sol.Length; i++)
            {
                newSol[i] = sol[i];
            }
            return newSol;
        }

        public new double[] getInitialSolution()
        {
            double[] sol = new double[noOfItems];
            while (true)
            {
                int rIndex = Convert.ToInt32(Math.Floor(Number.Rnd(sol.Length)));
                sol[rIndex] = 1;
                if (getFitness(sol) == Double.MaxValue)
                {
                    sol[rIndex] = 0;
                    return sol;
                }
            }
        }

        private new double getFitness(IEnumerable<int> solution) { return base.getFitness(solution); }

        public IEnumerable<int> toKnapsackList(double[] sol)
        {
            var ret = new List<int>();
            int i = 0;
            foreach (double item in sol)
            {
                if (Convert.ToInt32(item) == 1) yield return i;
                i++;
            }
        }

        public double getFitness(double[] sol)
        {
            return this.getFitness(toKnapsackList(sol));
        }

        public double[] mutate(double[] sol)
        {
            double[] newSol = new double[sol.Length];
            double alpha = Number.Rnd(-(LEVY_JUMP_INDEX * 2)) + LEVY_JUMP_INDEX;
            for (int i = 0; i < sol.Length; i++)
            {
                newSol[i] = Distribution.generateLevy(alpha) + sol[i];
            }
            return newSol;
        }

        public new Configuration<double[]> getConfiguration()
        {
            Configuration<double[]> config = new Configuration<double[]>();
            config.cloneFunction = this.clone;
            config.initializeSolutionFunction = this.getInitialSolution;
            config.movement = Search.Direction.Divergence;
            config.mutationFunction = this.mutate;
            config.noOfIterations = 50000;
            config.objectiveFunction = this.getFitness;
            config.populationSize = 5;
            config.selectionFunction = Selection.RoulleteWheel;
            config.writeToConsole = true;
            config.consoleWriteInterval = 100;
            config.consoleWriteFunction = (sol, fit, noOfIterations) =>
            {
                Console.WriteLine($"{noOfIterations}\tFitness\t{fit}");
            };
            config.hardObjectiveFunction = (double[] sol) =>
            {
                double fitness = this.getFitness(sol);
                return fitness < Double.MaxValue && fitness >= 0;
            };
            config.enforceHardObjective = true;
            return config;
        }
    }
}
