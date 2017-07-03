using MSearch.ABC;
using MSearch.Common;
using MSearch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Pigeons
{
    public class BinaryPIO<TData> : IMetaHeuristic<TData>
    {
        public BinaryPigeon<TData> current;
        private List<BinaryPigeon<TData>> pigeons { get; set; } = new List<BinaryPigeon<TData>>();
        private double mapFactor { get; set; } = 0.05;
        private TData[] globalBestSolution { get; set; }
        private TData[] localBestSolution { get; set; }
        private double globalBestFitness { get; set; }
        private double localBestFitness { get; set; }
        private LAHC lateAcceptance { get; set; }
        private Configuration<TData[]> config { get; set; }
        private List<double> iterationFitnessSequence = new List<double>();
        private int iterationCount { get; set; }
        public double _switchProbability { get; set; } = 0.2;

        private TData[] getBestIndividual()
        {
            if (localBestFitness > globalBestFitness) return localBestSolution;
            else return globalBestSolution;
        }

        public void create(Configuration<TData[]> config)
        {
            this.config = config;
            for (int i = 0; i < config.populationSize; i++) this.pigeons.Add(new BinaryPigeon<TData>(config)); //generate pigeons in swarm
            if (this.config.movement == Search.Direction.Optimization)
            {
                globalBestFitness = double.MaxValue;
                localBestFitness = double.MaxValue;
            }
            else if (this.config.movement == Search.Direction.Divergence)
            {
                globalBestFitness = double.MinValue;
                localBestFitness = double.MinValue;
            }
            this.lateAcceptance = new LAHC(config.tableSize, localBestFitness, this.config.movement);
            this.globalBestSolution = this.config.initializeSolutionFunction();
            this.globalBestFitness = this.config.objectiveFunction(this.globalBestSolution);
            this.localBestSolution = this.config.cloneFunction(globalBestSolution);
            this.localBestFitness = this.globalBestFitness + 0;
        }

        public TData[] fullIteration()
        {
            for (int count = 1; count <= this.config.noOfIterations; count++)
            {
                this.iterationCount = count;
                this.singleIteration();
                iterationFitnessSequence.Add(this.getBestFitness());
            }
            if (this.config.writeToConsole) Console.WriteLine("End of Iterations");
            return getBestIndividual();
        }

        public double getBestFitness()
        {
            return this.config.movement == Search.Direction.Optimization ? 
                                Math.Min(this.globalBestFitness, this.localBestFitness) :
                                Math.Max(this.globalBestFitness, this.localBestFitness);
        }

        public List<double> getIterationSequence()
        {
            return this.iterationFitnessSequence;
        }

        public TData[] singleIteration()
        {
            Action<TData[]> updateBestFn = (TData[] sol) =>
            {
                double fitness = config.objectiveFunction(sol);
                if ((config.hardObjectiveFunction != null &&
                    ((config.enforceHardObjective && config.hardObjectiveFunction(sol)) || (!config.enforceHardObjective))) ||
                    config.hardObjectiveFunction == null)
                {
                    if ((config.newFitnessIsBetter(localBestFitness, fitness) || lateAcceptance.Update(fitness)))
                    {
                        localBestFitness = fitness;
                        localBestSolution = this.config.cloneFunction(sol);
                    }
                    if (config.newFitnessIsBetter(globalBestFitness, fitness))
                    {
                        globalBestFitness = fitness;
                        globalBestSolution = this.config.cloneFunction(sol);
                    }
                }
            };
            if (Number.Rnd() < _switchProbability)
            {
                int a = Convert.ToInt32(Math.Floor(Number.Rnd() * pigeons.Count));
                int b = Convert.ToInt32(Math.Floor(Number.Rnd() * pigeons.Count));
                var newSol = GA.CrossOver.AutoTwoPoint(pigeons[a].getSolution(), pigeons[b].getSolution());
                updateBestFn(newSol.ToArray().First().ToArray());
                updateBestFn(newSol.ToArray().Last().ToArray());
                pigeons[a].setSolution(newSol.ToArray().First().ToArray());
                pigeons[b].setSolution(newSol.ToArray().Last().ToArray());
            }
            else
            {
                for (int i = 0; i < pigeons.Count; i++)
                {
                    this.current = pigeons[i];
                    var newSol = this.config.mutationFunction(pigeons[i].getSolution());
                    updateBestFn(newSol);
                    pigeons[i].setSolution(newSol);
                }
            }
            if (config.writeToConsole && ((iterationCount % config.consoleWriteInterval == 0) || (iterationCount - 1 == 0))) Console.WriteLine(iterationCount + " = " + localBestFitness + " \t " + globalBestFitness); // + "\t" + localBestSolution.ToJson()
            return this.getBestIndividual();
        }

        public void create(Configuration<TData> config)
        {
            throw new NotImplementedException();
        }

        TData IMetaHeuristic<TData>.singleIteration()
        {
            throw new NotImplementedException();
        }

        TData IMetaHeuristic<TData>.fullIteration()
        {
            throw new NotImplementedException();
        }

        public TData[] getLocalBestSolution() => this.localBestSolution;

        public TData[] getGlobalBestSolution() => this.globalBestSolution;
    }
}
