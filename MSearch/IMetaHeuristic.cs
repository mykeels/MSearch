using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch
{
    public interface IMetaHeuristic<SolutionType>
    {
        void create(Configuration<SolutionType> config);

        SolutionType singleIteration();

        SolutionType fullIteration();

        double getBestFitness();

        List<double> getIterationSequence();
    }
}
