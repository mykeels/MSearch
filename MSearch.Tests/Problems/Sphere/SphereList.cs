using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Extensions;
using MSearch.Common;

namespace MSearch.Tests.Problems.Sphere
{
    public class SphereList
    {
        public double probability { get; set; }

        public const int NO_OF_SPHERE_DIMENSIONS = 5;

        public SphereList()
        {
            this.probability = 0.3;
        }

        public List<double> getInitialSolution() => this.getInitialSolution(NO_OF_SPHERE_DIMENSIONS);

        public List<double> getInitialSolution(int width)
        {
            var sol = new List<double>();
            for (int i = 0; i < width; i++)
            {
                sol.Add(Number.Rnd(-10.24) + 5.12);
            }
            return sol;
        }

        public double getFitness(List<double> sol)
        {
            double sum = 0;
            for (int i = 0; i < sol.Count(); i++)
            {
                sum += (sol[i] * sol[i]);
            }
            return sum;
        }

        public List<double> mutate(List<double> sol)
        {
            List<double> newSol = clone(sol);
            for (int i = 0; i < newSol.Count(); i++)
            {
                double num = newSol[i];
                if (Number.Rnd() < this.probability) newSol[i] = num + (Number.Rnd(-2) + 1);
            }
            return newSol;
        }

        public List<double> clone(List<double> sol)
        {
            List<double> newSol = new List<double>();
            for (int i = 0; i < sol.Count(); i++)
            {
                newSol.Add(sol[i] + 0);
            }
            return newSol;
        }

        public Configuration<List<double>> getConfiguration()
        {
            Configuration<List<double>> config = new Configuration<List<double>>();
            config.cloneFunction = this.clone;
            config.initializeSolutionFunction = this.getInitialSolution;
            config.movement = Search.Direction.Optimization;
            config.mutationFunction = this.mutate;
            config.noOfIterations = 5000;
            config.objectiveFunction = this.getFitness;
            config.populationSize = 5;
            config.writeToConsole = true;
            config.consoleWriteInterval = 100;
            config.enforceHardObjective = false;
            config.selectionFunction = Selection.RoulleteWheel;
            return config;
        }
    }
}
