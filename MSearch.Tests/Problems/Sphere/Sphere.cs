using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Extensions;
using MSearch.Common;

namespace MSearch.Tests.Problems.Sphere
{
    public class Sphere
    {
        public double probability { get; set; }

        public const int NO_OF_SPHERE_DIMENSIONS = 5;

        public Sphere()
        {
            this.probability = 0.3;
        }

        public double[] getInitialSolution() => this.getInitialSolution(NO_OF_SPHERE_DIMENSIONS);

        public double[] getInitialSolution(int width)
        {
            var sol = new List<double>();
            for (int i = 0; i < width; i++)
            {
                sol.Add(Number.Rnd(-10.24) + 5.12);
            }
            return sol.ToArray();
        }

        public double getFitness(double[] sol)
        {
            double sum = 0;
            for (int i = 0; i < sol.Length; i++)
            {
                sum += (sol[i] * sol[i]);
            }
            return sum;
        }

        public double[] mutate(double[] sol)
        {
            double[] newSol = clone(sol);
            for (int i = 0; i < newSol.Length; i++)
            {
                double num = newSol[i];
                if (Number.Rnd() < this.probability) newSol[i] = num + (Number.Rnd(-2) + 1);
            }
            return newSol;
        }

        public double[] clone(double[] sol)
        {
            double[] newSol = new double[sol.Length];
            for (int i = 0; i < sol.Length; i++)
            {
                newSol[i] = sol[i] + 0;
            }
            return newSol;
        }

        public Configuration<double[]> getConfiguration()
        {
            Configuration<double[]> config = new Configuration<double[]>();
            config.cloneFunction = this.clone;
            config.initializeSolutionFunction = this.getInitialSolution;
            config.movement = Search.Direction.Optimization;
            config.mutationFunction = this.mutate;
            config.noOfIterations = 5000;
            config.objectiveFunction = this.getFitness;
            config.populationSize = 5;
            config.writeToConsole = true;
            config.consoleWriteInterval = 100;
            config.enforceHardObjective = true;
            config.selectionFunction = Selection.RoulleteWheel;
            return config;
        }
    }
}
