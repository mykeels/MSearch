using System;
using MSearch.Common;

namespace MSearch.ABC
{
    public class BeeLahc<FoodType> : Bee<FoodType>, IBee<FoodType>
    {
        protected override int _timeSinceLastImprovement { get; set; }
        protected override int _nonImprovementLimit { get; set; }
        protected override Func<FoodType, FoodType> _mutationFunc { get; set; }
        protected override Func<FoodType, double> _fitnessFunc { get; set; }
        private LAHC lahc { get; set; }
        public bool useLahc { get; set; }

        public BeeLahc()
        {
            this.Fitness = double.MaxValue;
            this.defaultFitness = double.MaxValue;
            this.Type = BeeTypeClass.Scout;
            this.lahc = new LAHC(20, this.Fitness, Search.Direction.Optimization);
        }

        public BeeLahc(Func<FoodType, FoodType> mFunc, Func<FoodType, double> fFunc, BeeTypeClass _type,
            int ID = 0, int _failureLimit = 20, int _tableSize = 50, Search.Direction movement = Search.Direction.Optimization)
        {
            if ((movement == Search.Direction.Divergence))
            {
                this.Fitness = double.MinValue;
                defaultFitness = double.MinValue;
            }
            else if (movement == Search.Direction.Optimization)
            {
                this.Fitness = double.MaxValue;
                defaultFitness = double.MaxValue;
            }
            if (mFunc == null | fFunc == null)
            {
                throw new Exception(string.Format("Ogbeni, na Bee #{0} be this. How i wan take mutate na?", ID));
            }
            this._nonImprovementLimit = _failureLimit;
            this._mutationFunc = mFunc;
            this._fitnessFunc = fFunc;
            this.Type = _type;
            this.lahc = new LAHC(_tableSize, this.Fitness, movement);
            this.Movement = movement;
        }

        public override FoodType Mutate()
        {
            FoodType ret = _mutationFunc(this.Food);
            double _fitness = Bee<FoodType>.GetFitness(ret, _fitnessFunc);
            if (this.Fitness.Equals(defaultFitness) || _fitness < this.Fitness || (useLahc && this.lahc.Update(_fitness)))
            {
                this.Fitness = _fitness;
                this.Food = ret;
                this._timeSinceLastImprovement = 0;
            }
            else
            {
                this._timeSinceLastImprovement += 1;
                //Solution has not improved
                if (this._timeSinceLastImprovement >= this._nonImprovementLimit)
                {
                    this.ChangeToOnlooker();
                }
            }
            return this.Food;
        }

    }
}
