using System;
using MSearch.Common;

namespace MSearch.ABC
{
    public class Bee<FoodType> : IBee<FoodType>
    {
        public int ID { get; set; }
        public BeeTypeClass Type { get; set; }
        public FoodType Food { get; set; }
        public double Fitness { get; set; }
        public double defaultFitness { get; set; }
        public Search.Direction Movement { get; set; }
        protected virtual int _timeSinceLastImprovement { get; set; }
        protected virtual int _nonImprovementLimit { get; set; }
        protected virtual Func<FoodType, FoodType> _mutationFunc { get; set; }
        protected virtual Func<FoodType, double> _fitnessFunc { get; set; }

        public Bee()
        {
            this.Fitness = double.MaxValue;
            this.defaultFitness = double.MaxValue;
            this.Type = BeeTypeClass.Scout;
        }

        public Bee(Func<FoodType, FoodType> mFunc, Func<FoodType, double> fFunc, BeeTypeClass _type, int ID = 0, int _failureLimit = 20, Search.Direction movement = Search.Direction.Optimization)
        {
            this.Init(mFunc, fFunc, _type, ID, _failureLimit, movement);
        }

        public void Init(Func<FoodType, FoodType> mFunc, Func<FoodType, double> fFunc, BeeTypeClass _type, int ID = 0, int _failureLimit = 20, Search.Direction movement = Search.Direction.Optimization)
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
            this.Movement = movement;
        }

        public void ChangeToEmployed(FoodType _food, Func<FoodType, FoodType> mFunc = null, Func<FoodType, double> fFunc = null)
        {
            this._timeSinceLastImprovement = 0;
            this.Type = BeeTypeClass.Employed;
            this.Food = _food;
            this.GetFitness();
            if (mFunc != null)
            {
                this._mutationFunc = mFunc;
            }
            if (fFunc != null)
            {
                this._fitnessFunc = fFunc;
            }
        }

        public void ChangeToOnlooker()
        {
            this.Type = BeeTypeClass.Onlooker;
            this.Food = default(FoodType);
            this.Fitness = defaultFitness;
            this._timeSinceLastImprovement = 0;
        }

        public void ChangeToScout()
        {
            this.Type = BeeTypeClass.Scout;
            this.Food = default(FoodType);
            this.Fitness = defaultFitness;
        }

        public int GetBeeID()
        {
            return ID;
        }

        public int SetBeeID(int ID)
        {
            this.ID = ID;
            return ID;
        }

        public BeeTypeClass GetBeeType()
        {
            return Type;
        }

        public FoodType GetFood()
        {
            return Food;
        }

        public FoodType SetFood(FoodType food)
        {
            this.Food = food;
            return food;
        }

        #region "EmployedBees"
        public virtual FoodType Mutate()
        {
            FoodType ret = _mutationFunc(this.Food);
            double _fitness = Bee<FoodType>.GetFitness(ret, _fitnessFunc);
            if (this.Fitness.Equals(defaultFitness) | (Movement == Search.Direction.Optimization && _fitness < this.Fitness) ||
                (Movement == Search.Direction.Divergence && _fitness > this.Fitness))
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

        public double GetFitness()
        {
            if (this.Food == null)
            {
                this.Fitness = Double.MaxValue;
                this.ChangeToOnlooker();
                return this.Fitness;
            }
            this.Fitness = _fitnessFunc(this.Food);
            return this.Fitness;
        }
        #endregion

        public static double GetFitness(FoodType _food, Func<FoodType, double> _fitnessFunc)
        {
            return _fitnessFunc(_food);
        }

        #region "OnlookerBees"

        #endregion


    }
}
