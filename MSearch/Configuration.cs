using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Common;

namespace MSearch
{
    public class Configuration<SolutionType>
    {
        public Func<SolutionType, SolutionType> mutationFunction { get; set; }
        public Func<SolutionType, double> objectiveFunction { get; set; }
        public Func<SolutionType, SolutionType> cloneFunction { get; set; }
        public Func<IEnumerable<SolutionType>, IEnumerable<double>, int, IEnumerable<SolutionType>> selectionFunction { get; set; }
        public Func<SolutionType> initializeSolutionFunction { get; set; }
        public Func<SolutionType, bool> hardObjectiveFunction { get; set; }
        /// <summary>
        /// Passes the Best Solution, Best Fitness and No of Iterations
        /// </summary>
        public Action<SolutionType, double, int> consoleWriteFunction { get; set; }
        public bool enforceHardObjective { get; set; }
        public bool writeToConsole { get; set; }
        public int consoleWriteInterval { get; set; }
        public int noOfIterations { get; set; }
        public int populationSize { get; set; }
        public Search.Direction movement { get; set; }
        /// <summary>
        /// Useful for LAHC Implementations
        /// </summary>
        public int tableSize { get; set; }

        public Configuration()
        {
            this.noOfIterations = 500;
            this.writeToConsole = true;
            this.enforceHardObjective = false;
            this.movement = Search.Direction.Optimization;
            this.populationSize = 1;
            this.consoleWriteInterval = 10;
        }

        public bool newFitnessIsBetter(double oldFitness, double newFitness)
        {
            return (movement == Search.Direction.Optimization && newFitness < oldFitness) ||
                (movement == Search.Direction.Divergence && newFitness > oldFitness);
        }
    }
}
