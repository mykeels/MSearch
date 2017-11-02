using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Extensions;
using MSearch.Common;
using Newtonsoft.Json;

namespace MSearch.SA
{
    public class SimulatedAnnealing<SolutionType> : IMetaHeuristic<SolutionType>
    {
        public Configuration<SolutionType> Config { get; set; }
        private SolutionType _bestIndividual { get; set; }
        private double _bestFitness { get; set; }
        private SolutionType _currentIndividual { get; set; }
        private double _currentFitness { get; set; }
        private int _iterationCount { get; set; }
        private List<double> _iterationFitnessSequence { get; set; }
        private double _initialTemperature { get; set; }
        private double _temperature { get; set; }
        private Func<double, double, double> _acceptanceProbabilityFunction { get; set; }
        private Func<double, double> _temperatureUpdateFunction { get; set; }
        private TemperatureUpdate _temperatureUpdateType { get; set; }

        public enum TemperatureUpdate
        {
            Default,
            Fast,
            Boltz,
            Other
        }

        public SimulatedAnnealing()
        {
            this._initialTemperature = 100;
            this._temperature = 100;
            this._iterationFitnessSequence = new List<double>();
            this._acceptanceProbabilityFunction = defaultAcceptanceProbabilityFunction;
            this._temperatureUpdateFunction = defaultTemperatureUpdate;
        }

        public SimulatedAnnealing(Func<double, double, double> _acceptanceProbabilityFunction, TemperatureUpdate _temperatureUpdateType,
            Func<double, double> _temperatureUpdateFunction = null) : this()
        {
            this._acceptanceProbabilityFunction = _acceptanceProbabilityFunction;
            if (_temperatureUpdateType == TemperatureUpdate.Other)
            {
                if (_temperatureUpdateFunction == null) this._temperatureUpdateFunction = defaultTemperatureUpdate;
                else this._temperatureUpdateFunction = _temperatureUpdateFunction;
            }
            else if (_temperatureUpdateType == TemperatureUpdate.Default) this._temperatureUpdateFunction = defaultTemperatureUpdate;
            else if (_temperatureUpdateType == TemperatureUpdate.Boltz) this._temperatureUpdateFunction = boltzTemperatureUpdate;
            else this._temperatureUpdateFunction = fastTemperatureUpdate;
        }

        public void create(Configuration<SolutionType> config)
        {
            this.Config = config;
            if (_acceptanceProbabilityFunction == null) _acceptanceProbabilityFunction = defaultAcceptanceProbabilityFunction;
            _currentIndividual = Config.initializeSolutionFunction();
            _currentFitness = Config.objectiveFunction(_currentIndividual);
            _bestIndividual = Config.cloneFunction(_currentIndividual);
            _bestFitness = _currentFitness + 0;
        }

        public SolutionType fullIteration()
        {
            for (int count = 1; count <= Config.noOfIterations; count++)
            {
                _iterationCount = count;
                _currentIndividual = singleIteration();
                _iterationFitnessSequence.Add(_currentFitness);
            }
            if (Config.writeToConsole) Console.WriteLine("End of Iterations");
            return _bestIndividual;
        }

        public List<double> getIterationSequence()
        {
            return _iterationFitnessSequence;
        }

        public SolutionType singleIteration()
        {
            _temperature = _temperatureUpdateFunction(_temperature);
            SolutionType newSol = Config.mutationFunction(_currentIndividual);
            double newFitness = Config.objectiveFunction(newSol);

            if ((Config.hardObjectiveFunction != null &&
                    ((Config.enforceHardObjective && Config.hardObjectiveFunction(newSol)) || (!Config.enforceHardObjective))) ||
                    Config.hardObjectiveFunction == null)
            {
                if (_acceptanceProbabilityFunction(_currentFitness, newFitness) >= Number.Rnd())
                {
                    _currentIndividual = newSol;
                    _currentFitness = newFitness + 0;
                }

                if ((Config.newFitnessIsBetter(_bestFitness, _currentFitness))) //store the best individual if the current is better
                {
                    _bestIndividual = Config.cloneFunction(_currentIndividual);
                    _bestFitness = _currentFitness + 0;
                }
            }

            if (Config.writeToConsole && ((_iterationCount % Config.consoleWriteInterval) == 0) || (_iterationCount - 1 == 0))
            {
                if (Config.consoleWriteFunction == null) Console.WriteLine(_iterationCount + "\t" + JsonConvert.SerializeObject(_bestIndividual) + " = " + _bestFitness);
                else Config.consoleWriteFunction(_bestIndividual, _bestFitness, _iterationCount);
            }

            return _currentIndividual; //current individual is returned
        }

        public double getBestFitness()
        {
            return this._bestFitness;
        }

        private double defaultAcceptanceProbabilityFunction(double oldFitness, double newFitness)
        {
            if (Config.newFitnessIsBetter(oldFitness, newFitness)) return 1;
            else if (Config.movement == Search.Direction.Divergence) return 1 / (1 + Math.Exp((oldFitness - newFitness) / _temperature));
            else return 1 / (1 + Math.Exp((newFitness - oldFitness) / _temperature));
        }

        private double defaultTemperatureUpdate(double temperature)
        {
            return _initialTemperature * Math.Pow(0.95, _iterationCount);
        }

        private double fastTemperatureUpdate(double temperature)
        {
            return _initialTemperature / _iterationCount;
        }

        private double boltzTemperatureUpdate(double temperature)
        {
            return _initialTemperature / Math.Log(_iterationCount);
        }
    }
}
