using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Pigeons
{
    public class PigeonOptimize<TSolutionType> : IMetaHeuristic<TSolutionType>
    {
        public void create(Configuration<TSolutionType> config)
        {
            throw new NotImplementedException();
        }

        public TSolutionType fullIteration()
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

        public TSolutionType singleIteration()
        {
            throw new NotImplementedException();
        }
    }
}
