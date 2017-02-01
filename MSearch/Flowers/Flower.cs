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
    public class Flower<PollenType>
    {
        private Configuration<PollenType> config = null;
        private PollenType solution = default(PollenType);
        private double fitness = 0;
        private bool fitnessNeedsUpdate = true;

        private Flower() { }
        public Flower(Configuration<PollenType> config)
        {
            this.config = config;
            this.solution = config.initializeSolutionFunction();
            this.fitnessNeedsUpdate = true;
        }
        
        public PollenType getSolution() 
        {
            return this.solution;
        }

        public double getFitness()
        {
            if (this.fitnessNeedsUpdate) this.fitness = this.config.objectiveFunction(this.solution);
            this.fitnessNeedsUpdate = false;
            return this.fitness;
        }

        public Flower<PollenType> clone()
        {
            Flower<PollenType> ret = new Flower<PollenType>();
            ret.solution = this.config.cloneFunction(this.solution);
            ret.fitness = this.getFitness();
            ret.fitnessNeedsUpdate = this.fitnessNeedsUpdate;
            ret.config = this.config;
            return ret;
        }

        public Flower<PollenType> doGlobalPollination(Flower<PollenType> gBest)
        {
            var newFlower = this.clone();
            IList newFlowerSolList = newFlower.solution as IList;
            IList gBestList = gBest.solution as IList;
            for (int i = 0; i < gBestList.Count; i++)
            {
                newFlowerSolList[i] = (double)newFlowerSolList[i] + Distribution.generateLevy((double)gBestList[i] - (double)newFlowerSolList[i]);
            }
            newFlower.solution = (PollenType)newFlowerSolList;
            newFlower.fitnessNeedsUpdate = true;
            return newFlower;
        }

        public Flower<PollenType> doLocalPollination(Flower<PollenType> flower1, Flower<PollenType> flower2)
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
            newFlower.solution = (PollenType)newFlowerSolList;
            newFlower.fitnessNeedsUpdate = true;
            return newFlower;
        }
    }
}
