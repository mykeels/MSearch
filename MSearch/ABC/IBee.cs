using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Common;

namespace MSearch.ABC
{
    public interface IBee<FoodSource>
    {
        void Init(Func<FoodSource, FoodSource> mFunc, Func<FoodSource, double> fFunc, BeeTypeClass _type, int ID = 0, int _failureLimit = 20, Search.Direction movement = Search.Direction.Optimization);
        void ChangeToEmployed(FoodSource _food, Func<FoodSource, FoodSource> mFunc = null, Func<FoodSource, double> fFunc = null);
        void ChangeToOnlooker();
        void ChangeToScout();
        FoodSource Mutate();
        double GetFitness();
        BeeTypeClass GetBeeType();
        FoodSource GetFood();
        FoodSource SetFood(FoodSource food);
        int GetBeeID();
        int SetBeeID(int ID);
    }
}
