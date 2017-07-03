using MSearch.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Pigeons
{
    /// <summary>
    /// A class for representing pigeons in Pigeon Inspired Optimization algorithm that deals with binary 1/0 problems
    /// </summary>
    public class BinaryPigeon<TData>
    {
        private TData[] current { get; set; }
        public double[] velocity { get; set; }
        private TData[] bestSolution { get; set; }
        private double bestFitness { get; set; }
        private Configuration<TData[]> config { get; set; }

        public BinaryPigeon(Configuration<TData[]> config)
        {
            this.config = config;
            this.current = config.initializeSolutionFunction();
            this.velocity = Array.CreateInstance(typeof(double), current.Length).OfType<double>().ToArray();
            this.bestSolution = this.current;
            this.bestFitness = config.objectiveFunction(this.current);
        }

        public TData[] getSolution()
        {
            return this.config.cloneFunction(bestSolution);
        }

        public void setSolution(TData[] sol)
        {
            this.current = sol;
            this.bestFitness = config.objectiveFunction(this.current);
        }

        public double getFitness()
        {
            return this.bestFitness;
        }
    }
}
