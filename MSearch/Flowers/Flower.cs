using MSearch.Common;
using MSearch.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Flowers
{
    public class Flower<TPollenType>
    {
        private Configuration<List<TPollenType>> config = null;
        private List<TPollenType> solution = null;
        private double fitness = 0;
        private bool fitnessNeedsUpdate = true;

        private Flower() { }
        public Flower(Configuration<List<TPollenType>> config)
        {
            this.config = config;
            this.solution = config.initializeSolutionFunction();
            this.fitnessNeedsUpdate = true;
        }
        
        public List<TPollenType> getSolution() 
        {
            return this.solution;
        }

        public double getFitness()
        {
            if (this.fitnessNeedsUpdate) this.fitness = this.config.objectiveFunction(this.solution);
            this.fitnessNeedsUpdate = false;
            return this.fitness;
        }

        public Flower<TPollenType> clone()
        {
            var ret = new Flower<TPollenType>();
            ret.solution = this.config.cloneFunction(this.solution);
            ret.fitness = this.getFitness();
            ret.fitnessNeedsUpdate = this.fitnessNeedsUpdate;
            ret.config = this.config;
            return ret;
        }

        public Flower<TPollenType> doGlobalPollination(Flower<TPollenType> gBest)
        {
            var newFlower = this.clone();
            var gBestList = gBest.solution;
            /*for (int i = 0; i < gBestList.Count; i++)
            {
                newFlowerSolList[i] = (double)newFlowerSolList[i] + Distribution.generateLevy((double)gBestList[i] - (double)newFlowerSolList[i]);
            }*/
            var newFlowerSolList = config.mutationFunction(newFlower.solution);
            newFlower.solution = newFlowerSolList;
            if (config.enforceHardObjective && !config.hardObjectiveFunction(newFlower.solution))
            {
                return this.clone();
            }
            newFlower.fitnessNeedsUpdate = true;
            return newFlower;
        }

        public Flower<TPollenType> doLocalPollination(Flower<TPollenType> flower1, Flower<TPollenType> flower2)
        {
            var newFlower = this.clone();
            var newFlowerSolList = newFlower.solution;
            var flower1SolList = flower1.solution;
            var flower2SolList = flower2.solution;
            if (flower1SolList.Count != flower2SolList.Count) throw new Exception(Constants.FLOWERS_SAME_LENGTH_EXCEPTION);
            for (int i = 0; i < flower1SolList.Count; i++)
            {
                newFlowerSolList[i] = (TPollenType)Convert.ChangeType(Convert.ToDouble(newFlowerSolList[i]) + Number.Rnd((Convert.ToDouble(flower1SolList[i]) - Convert.ToDouble(flower2SolList[i]))), typeof(TPollenType));
            }
            newFlower.solution = newFlowerSolList;
            if (config.enforceHardObjective && !config.hardObjectiveFunction(newFlower.solution))
            {
                return this.clone();
            }
            newFlower.fitnessNeedsUpdate = true;
            return newFlower;
        }
    }
}
