using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Flowers
{
    public class Pollination<PollenType>: IMetaHeuristic<PollenType>
    {
        private Flower<PollenType>[] flowers = null;
        public Pollination(int noOfFlowers)
        {

        }

        public void create(Configuration<PollenType> config)
        {
            throw new NotImplementedException();
        }

        public PollenType fullIteration()
        {
            throw new NotImplementedException();
        }

        public double getBestFitness()
        {
            throw new NotImplementedException();
        }

        public List<double> getIterationSequence()
        {
            throw new NotImplementedException();
        }

        public PollenType singleIteration()
        {
            throw new NotImplementedException();
        }
    }
}
