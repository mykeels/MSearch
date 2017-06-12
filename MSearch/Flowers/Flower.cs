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
        private Configuration<TPollenType> config = null;
        private TPollenType solution = default(TPollenType);
        private double fitness = 0;
        private bool fitnessNeedsUpdate = true;

        private Flower() { }
        public Flower(Configuration<TPollenType> config)
        {
            this.config = config;
            this.solution = config.initializeSolutionFunction();
            this.fitnessNeedsUpdate = true;
        }
        
        public TPollenType getSolution() 
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
            Flower<TPollenType> ret = new Flower<TPollenType>();
            ret.solution = this.config.cloneFunction(this.solution);
            ret.fitness = this.getFitness();
            ret.fitnessNeedsUpdate = this.fitnessNeedsUpdate;
            ret.config = this.config;
            return ret;
        }

        public Flower<TPollenType> doGlobalPollination(Flower<TPollenType> gBest)
        {
            var newFlower = this.clone();
            IList newFlowerSolList = newFlower.solution as IList;
            IList gBestList = gBest.solution as IList;
            /*for (int i = 0; i < gBestList.Count; i++)
            {
                newFlowerSolList[i] = (double)newFlowerSolList[i] + Distribution.generateLevy((double)gBestList[i] - (double)newFlowerSolList[i]);
            }*/
            newFlowerSolList = (IList)config.mutationFunction((TPollenType)newFlowerSolList);
            newFlower.solution = (TPollenType)newFlowerSolList;
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
            IList newFlowerSolList = newFlower.solution as IList;
            IList flower1SolList = flower1.solution as IList;
            IList flower2SolList = flower2.solution as IList;
            if (flower1SolList.Count != flower2SolList.Count) throw new Exception(Constants.FLOWERS_SAME_LENGTH_EXCEPTION);
            for (int i = 0; i < flower1SolList.Count; i++)
            {
                newFlowerSolList[i] = (double)newFlowerSolList[i] + Number.Rnd(((double)flower1SolList[i] - (double)flower2SolList[i]));
            }
            newFlower.solution = (TPollenType)newFlowerSolList;
            if (config.enforceHardObjective && !config.hardObjectiveFunction(newFlower.solution))
            {
                return this.clone();
            }
            newFlower.fitnessNeedsUpdate = true;
            return newFlower;
        }
    }
}
