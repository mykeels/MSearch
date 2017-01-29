using System;
using System.Collections.Generic;
using System.Linq;
using MSearch.Extensions;
using MSearch.Common;

namespace MSearch.GA
{
    public class GeneticAlgorithm<IndividualType> : IMetaHeuristic<IndividualType>
    {
        public Configuration<IndividualType> Config { get; set; }
        private IndividualType _bestIndividual { get; set; }
        private double _bestFitness { get; set; }
        private int _iterationCount { get; set; }
        //private Func<IndividualType> _initFunc { get; set; }
        private Func<IndividualType, IndividualType, IndividualType> _crossOverFunc { get; set; }
        public List<KeyValue<IndividualType, double>> Population { get; set; }
        private List<double> _iterationFitnessSequence = new List<double>();

        public GeneticAlgorithm()
        {

        }

        public GeneticAlgorithm(Func<IndividualType, IndividualType, IndividualType> crossOverFunction)
        {
            this._crossOverFunc = crossOverFunction;
        }

        public void create(Configuration<IndividualType> config)
        {
            this.Config = config;
            this.Population = new List<KeyValue<IndividualType, double>>();
            if (config.populationSize <= 2) throw new Exception("Population Size must be more than 2");
            if (_crossOverFunc == null) throw new Exception("Please provide a Cross Over Function");
            for (int i = 0; i < config.populationSize; i++)
            {
                this.Population.Add(new KeyValue<IndividualType, double>());
            }
            if (config.movement == Search.Direction.Optimization)
            {
                _bestFitness = double.MaxValue;
            }
            else if (config.movement == Search.Direction.Divergence)
            {
                _bestFitness = double.MinValue;
            }
            for (int index = 0; index <= Population.Count - 1; index++)
            {
                Population[index].key = config.initializeSolutionFunction();
                Population[index].value = config.objectiveFunction.Invoke(Population[index].key);
            }
        }

        public List<double> getIterationSequence()
        {
            return _iterationFitnessSequence;
        }

        public double getBestFitness()
        {
            return this._bestFitness;
        }

        public IndividualType fullIteration()
        {
            for (int count = 1; count <= Config.noOfIterations; count++)
            {
                _iterationCount = count;
                IndividualType _bestIndividual = singleIteration();
                _iterationFitnessSequence.Add(_bestFitness);
            }
            Console.WriteLine("End of Iterations");
            return _bestIndividual;
        }

        public IndividualType singleIteration()
        {
            var individuals = Config.selectionFunction.Invoke(Population.Select((individual) => individual.key), Population.Select((individual) => individual.value), 2);
            IndividualType individualA = individuals.ElementAt(0);
            IndividualType individualB = individuals.ElementAt(1);
            double fitnessA = Config.objectiveFunction.Invoke(individualA);
            double fitnessB = Config.objectiveFunction.Invoke(individualB);
            int indexA = Population.Select((individual) => individual.value).ToList().IndexOf(fitnessA);
            int indexB = Population.Select((individual) => individual.value).ToList().IndexOf(fitnessB);

            //cross-over
            IndividualType newIndividual = this._crossOverFunc(Config.cloneFunction.Invoke(individualA), Config.cloneFunction.Invoke(individualB));
            double newFitness = Config.objectiveFunction.Invoke(newIndividual);



            if ((Config.hardObjectiveFunction != null &&
                    ((Config.enforceHardObjective && Config.hardObjectiveFunction(newIndividual)) || (!Config.enforceHardObjective))) ||
                    Config.hardObjectiveFunction == null)
            {

                if ((Config.movement == Search.Direction.Optimization && newFitness < _bestFitness) ||
                (Config.movement == Search.Direction.Divergence && newFitness > _bestFitness))
                {

                    _bestIndividual = newIndividual;
                    _bestFitness = newFitness;
                }

                if ((Config.movement == Search.Direction.Optimization && newFitness < fitnessA) ||
                        (Config.movement == Search.Direction.Divergence && newFitness > fitnessA))
                {
                    individualA = newIndividual;
                    Population[indexA].key = individualA;
                    Population[indexA].value = newFitness;
                    fitnessA = newFitness;
                }
                else if ((Config.movement == Search.Direction.Optimization && newFitness < fitnessB) ||
                    (Config.movement == Search.Direction.Divergence && newFitness > fitnessB))
                {
                    individualB = newIndividual;
                    Population[indexA].key = individualB;
                    Population[indexA].value = newFitness;
                    fitnessB = newFitness;
                }
            }

            //mutation
            line1:
            IndividualType individualAClone = Config.mutationFunction.Invoke(Config.cloneFunction.Invoke(individualA));
            IndividualType individualBClone = Config.mutationFunction.Invoke(Config.cloneFunction.Invoke(individualB));
            double fitnessAClone = Config.objectiveFunction.Invoke(individualAClone);
            double fitnessBClone = Config.objectiveFunction.Invoke(individualBClone);

            if (Config.hardObjectiveFunction != null &&
                    ((Config.enforceHardObjective &&
                    !Config.hardObjectiveFunction(individualAClone) || !Config.hardObjectiveFunction(individualBClone))))
            {
                goto line1;
            }

            if ((Config.movement == Search.Direction.Optimization && fitnessAClone < _bestFitness) ||
                (Config.movement == Search.Direction.Divergence && fitnessAClone > _bestFitness))
            {
                _bestIndividual = individualAClone;
                _bestFitness = fitnessAClone;
            }
            else if ((Config.movement == Search.Direction.Optimization && fitnessBClone < _bestFitness) ||
                (Config.movement == Search.Direction.Divergence && fitnessBClone > _bestFitness))
            {
                _bestIndividual = individualBClone;
                _bestFitness = fitnessBClone;
            }

            if ((Config.movement == Search.Direction.Optimization && fitnessAClone < fitnessA) ||
                (Config.movement == Search.Direction.Divergence && fitnessAClone > fitnessA))
            {
                Population[indexA].key = individualAClone;
                Population[indexA].value = fitnessAClone;
            }
            else if ((Config.movement == Search.Direction.Optimization && fitnessAClone < fitnessB) ||
                (Config.movement == Search.Direction.Divergence && fitnessAClone > fitnessB))
            {
                Population[indexB].key = individualAClone;
                Population[indexB].value = fitnessAClone;
            }
            else if ((Config.movement == Search.Direction.Optimization && fitnessBClone < fitnessA) ||
                (Config.movement == Search.Direction.Divergence && fitnessBClone > fitnessA))
            {
                Population[indexA].key = individualBClone;
                Population[indexA].value = fitnessBClone;
            }
            else if ((Config.movement == Search.Direction.Optimization && fitnessBClone < fitnessB) ||
                (Config.movement == Search.Direction.Divergence && fitnessBClone > fitnessB))
            {
                Population[indexB].key = individualBClone;
                Population[indexB].value = fitnessBClone;
            }
            if (Config.writeToConsole && _iterationCount % Config.consoleWriteInterval == 0)
            {
                Console.WriteLine(_iterationCount + "\t" + _bestIndividual.ToJson() + " = " + _bestFitness);
            }
            return _bestIndividual;
        }
    }
}
