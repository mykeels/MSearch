using MSearch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Flowers
{
    public class Pollination<PollenType>: IMetaHeuristic<PollenType>
    {
        private Flower<PollenType>[] _flowers = null;
        private Configuration<PollenType> _config = null;
        private Flower<PollenType> _gBest = null;
        private double _switchProbability = 0;
        private int _iterationCount = 0;
        private List<double> _iterationFitnessSequence = new List<double>();
        public Pollination()
        {

        }

        private Flower<PollenType>[] generateFlowers(int count)
        {
            List<Flower<PollenType>> flowers = new List<Flower<PollenType>>();
            for (int i = 0; i < count; i++)
            {
                flowers.Add(new Flower<PollenType>(_config));
            }
            return flowers.ToArray();
        }

        public void create(Configuration<PollenType> config)
        {
            this._config = config;
            this._flowers = this.generateFlowers(config.populationSize);
        }

        public PollenType fullIteration()
        {
            while (_iterationCount < _config.noOfIterations)
            {
                var _bestSolution = this.singleIteration();
                var _bestFitness = this.getBestFlower().getFitness();
                this._iterationFitnessSequence.Add(_bestFitness);
                if (this._config.writeToConsole && ((_iterationCount % this._config.consoleWriteInterval) == 0)) Console.WriteLine(_iterationCount + "\t" + _bestSolution.ToJson() + " = " + _bestFitness);
                this._iterationCount++;
            }
            return this.getBestFlower().getSolution();
        }

        public double getLocalBestFitness()
        {
            var localBestFitness = _config.movement == Common.Search.Direction.Divergence ? this._flowers.Max((flower) => flower.getFitness()) : this._flowers.Min((flower) => flower.getFitness());
            return localBestFitness;
        }

        public Flower<PollenType> getLocalBestFlower()
        {
            var localBest = this._flowers.Where(flower => flower.getFitness() == getLocalBestFitness()).FirstOrDefault();
            return localBest;
        }

        public double getBestFitness()
        {
            var localBestFitness = getLocalBestFitness();
            return _gBest == null || _config.newFitnessIsBetter(_gBest.getFitness(), localBestFitness) ? localBestFitness : _gBest.getFitness();
        }

        public List<double> getIterationSequence()
        {
            return this._iterationFitnessSequence;
        }

        public Flower<PollenType> getBestFlower()
        {
            var localBest = getLocalBestFlower();
            if (_gBest == null || _config.newFitnessIsBetter(_gBest.getFitness(), localBest.getFitness()))
            {
                this._gBest = localBest.clone();
            }
            return _gBest;
        }

        public PollenType singleIteration()
        {
            for (int i = 0; i < this._flowers.Length; i++)
            {
                var flower = this._flowers[i];
                Flower<PollenType> flowerClone = null;
                if (Number.Rnd() < this._switchProbability)
                {
                    //perform global pollination
                    flowerClone = flower.doGlobalPollination(_gBest);
                }
                else
                {
                    //perform local pollination
                    int a = 0, b = 0;
                    line1:
                    a = Convert.ToInt32(Math.Floor(Number.Rnd(this._flowers.Length)));
                    b = Convert.ToInt32(Math.Floor(Number.Rnd(this._flowers.Length)));
                    if (a == b) goto line1;
                    flowerClone = flower.doLocalPollination(this._flowers[a], this._flowers[b]);
                }
                if (_config.newFitnessIsBetter(this._flowers[i].getFitness(), flowerClone.getFitness()))
                {
                    this._flowers[i] = flowerClone;
                }
            }
            this._gBest = this.getBestFlower();
            return this._gBest.getSolution();
        }
    }
}
