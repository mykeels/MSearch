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

        public static double[] updateVelocity(double[] globalBestSolution, double[] current, double[] velocity, double mapFactor, int noOfIterations)
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

        public static double[] updateLocation(double[] sol, double[] velocity)
        {
            List<double> ret = new List<double>();
            for (int i = 0; i < Math.Min(sol.Length, velocity.Count()); i++)
            {
                ret.Add(Math.Abs(sol[i] + velocity[i]));
            }
            return ret.ToArray();
        }
    }
}
