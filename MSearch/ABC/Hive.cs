using System;
using System.Collections.Generic;
using System.Linq;
using MSearch.Common;
using MSearch.Extensions;
using Newtonsoft.Json;

namespace MSearch.ABC
{
    public partial class Hive<FoodType, IBee> : IMetaHeuristic<FoodType>
    {
        public Configuration<FoodType> Config { get; set; }
        //Note that FoodType and FoodSource should be of Same Class/Type
        private FoodType _bestFood { get; set; } // the best food source
        private double _bestFitness { get; set; } // the quality of the best food source
        private int _failureLimit = 20;
        private double _acceptProbability = 0.4; // the probability that an onlooker bee will accept a food source proposed by an employee bee
        //private Func<FoodType, FoodType> _cloneFunc { get; set; } // a function for cloning a food source. This is important to prevent two bees working on the same object in memory at a time
        //private Func<IEnumerable<FoodType>, IEnumerable<double>, int, IEnumerable<FoodType>> _selectionMethod { get; set; }
        //public Search.Direction Movement { get; set; }
        public List<IBee<FoodType>> Bees = new List<IBee<FoodType>>();

        private int _iterationCount = 0;
        private List<double> _iterationFitnessSequence = new List<double>();

        public Hive()
        {

        }

        public Hive(int _failureLimit = 20, double _acceptanceProbability = 0.4)
        {
            this._failureLimit = _failureLimit;
            this._acceptProbability = _acceptanceProbability;
        }

        public void create(Configuration<FoodType> config)
        {
            this.Config = config;
            int noOfBees = config.populationSize;
            if (noOfBees <= 1)
            {
                throw new Exception("You dey Craze? Dem tell you say one (1) bee na Swarm?");
            }
            if (Bees.Count == 0)
            {
                for (int index = 1; index <= noOfBees; index++)
                {
                    BeeTypeClass _type = default(BeeTypeClass);
                    if (index < (noOfBees / 2))
                    {
                        _type = BeeTypeClass.Employed;
                    }
                    else
                    {
                        _type = BeeTypeClass.Onlooker;
                    }
                    IBee<FoodType> bee = (IBee<FoodType>)Activator.CreateInstance<IBee>();
                    bee.Init(config.mutationFunction, config.objectiveFunction, _type, index - 1, _failureLimit, config.movement);
                    Bees.Add(bee);
                }
            }
            this.Start();
        }

        private void Start()
        {
            if (this.Config.movement == Search.Direction.Optimization)
            {
                _bestFitness = double.MaxValue;
            }
            else if (this.Config.movement == Search.Direction.Divergence)
            {
                _bestFitness = double.MinValue;
            }
            if (Bees.AsEnumerable().Count(_bee => { return _bee.GetFood() != null; }) == 0)
            {
                for (int index = 0; index <= Bees.Count - 1; index++)
                {
                    IBee<FoodType> _bee = this.Bees[index];
                    _bee.SetBeeID(index);
                    if (_bee.GetBeeType() == BeeTypeClass.Employed)
                    {
                        _bee.SetFood(Config.initializeSolutionFunction());
                        _bee.GetFitness();
                    }
                }
            }
        }

        public FoodType singleIteration()
        {
            FoodType ret = default(FoodType);
            List<IBee<FoodType>> _employedBees = Bees.Where((IBee<FoodType> _bee) => { return _bee.GetBeeType() == BeeTypeClass.Employed && _bee.GetFood() != null; }).ToList();
            int _employedCount = _employedBees.Count();
            for (int i = 0; i <= (_employedCount - 1); i++)
            {
                IBee<FoodType> _eBee = _employedBees.ElementAt(i);
                if (_eBee.GetBeeType() == BeeTypeClass.Employed && _eBee.GetFood() != null)
                {
                    FoodType currentFood = Config.cloneFunction.Invoke(_eBee.GetFood());
                    FoodType newFood = _eBee.Mutate();
                    if (Config.hardObjectiveFunction != null)
                    {
                        if (newFood != null)
                        {
                            bool passHardConstraints = Config.hardObjectiveFunction.Invoke(newFood);
                            if ((Config.enforceHardObjective && passHardConstraints) || passHardConstraints) _eBee.SetFood(newFood);
                            else _eBee.SetFood(currentFood);
                        }
                        else _eBee.SetFood(currentFood);
                    }
                    else if (newFood != null) _eBee.SetFood(newFood);
                    else _eBee.SetFood(currentFood);
                    Bees[_eBee.GetBeeID()] = _eBee;
                }
            }
            this.ShareInformation();
            IEnumerable<double> _fitnesses = Bees.Select((IBee<FoodType> _bee) => { return _bee.GetFitness(); });
            double _bestFit = 0;
            //collate bestFitness and bestFood
            if (this.Config.movement == Search.Direction.Divergence)
            {
                _bestFit = _fitnesses.Max();
                ret = Bees[_fitnesses.ToList().IndexOf(_bestFit)].GetFood();
                if (_bestFit > _bestFitness)
                {
                    _bestFitness = _bestFit;
                    _bestFood = this.Config.cloneFunction.Invoke(ret);
                }
            }
            else if (this.Config.movement == Search.Direction.Optimization)
            {
                _bestFit = _fitnesses.Min();
                ret = Bees[_fitnesses.ToList().IndexOf(_bestFit)].GetFood();
                if (_bestFit < _bestFitness)
                {
                    _bestFitness = _bestFit;
                    _bestFood = this.Config.cloneFunction.Invoke(ret);
                }
            }
            if (Config.writeToConsole && _iterationCount % Config.consoleWriteInterval == 0)
            {
                if (Config.consoleWriteFunction == null)
                {
                    Console.Write(_iterationCount + "\t" + _bestFitness + "");
                    Console.Write("\t" + JsonConvert.SerializeObject(_bestFood) + "\t");
                    Console.Write("E-Bees: " + _employedCount + '\t');
                    Console.Write("On-Bees: " + Convert.ToInt32(Bees.Count - _employedCount) + '\t');
                    if ((Config.hardObjectiveFunction != null)) Console.Write("Hard: " + Config.hardObjectiveFunction.Invoke(_bestFood));
                    Console.WriteLine();
                }
                else
                {
                    Config.consoleWriteFunction(_bestFood, _bestFitness, _iterationCount);
                }
            }
            return ret;
        }

        public List<double> getIterationSequence()
        {
            return _iterationFitnessSequence;
        }

        public FoodType fullIteration()
        {
            FoodType ret = default(FoodType);
            for (int count = 1; count <= Config.noOfIterations; count++)
            {
                _iterationCount = count;
                ret = singleIteration();
                _iterationFitnessSequence.Add(_bestFitness);
            }
            ret = _bestFood;
            Console.WriteLine("End of Iterations");
            return ret;
        }

        public void ShareInformation()
        {
            IEnumerable<IBee<FoodType>> _employedBees = Bees.Where((IBee<FoodType> _bee) => { return _bee.GetBeeType().Equals(BeeTypeClass.Employed); });
            IEnumerable<IBee<FoodType>> _onlookerBees = Bees.Where((IBee<FoodType> _bee) => { return _bee.GetBeeType().Equals(BeeTypeClass.Onlooker); });
            foreach (IBee<FoodType> _bee in _onlookerBees)
            {
                FoodType _food = SelectFood();
                if (_food != null && Number.Rnd() < _acceptProbability)
                {
                    _bee.ChangeToEmployed(_food);
                }
            }
            if (_employedBees.Count() == 0 & _onlookerBees.Count() > 0)
            {
                _onlookerBees.First().ChangeToEmployed(_bestFood);
            }
        }

        public FoodType SelectFood()
        {
            IEnumerable<IBee<FoodType>> _employedBees = Bees.Where((IBee<FoodType> _bee) => { return _bee.GetBeeType().Equals(BeeTypeClass.Employed) & _bee.GetFood() != null; });
            List<double> fitnesses = _employedBees.Select(_bee => { return _bee.GetFitness(); }).ToList();
            double sum = fitnesses.Sum();
            if (fitnesses.Count == 0) return default(FoodType);
            return this.Config.selectionFunction.Invoke(_employedBees.Select((_bee) => _bee.GetFood()), fitnesses, 1).First();
            /*while (true)
            {
                int selectedIndex = 0;
                foreach (double fitness in fitnesses)
                {
                    double probability = fitness / sum;
                    if (Number.Rnd() < probability)
                    {
                        return _cloneFunc(_employedBees.ElementAt(selectedIndex).Food);
                    }
                    selectedIndex += 1;
                }
            }*/
        }

        public double getBestFitness()
        {
            return this._bestFitness;
        }

        public int getFailureLimit()
        {
            return this._failureLimit + 0; //(val + 0) done to force pass-by-value
        }

        public double getAcceptanceProbability()
        {
            return this._acceptProbability + 0;  //(val + 0) done to force pass-by-value
        }
    }
}
