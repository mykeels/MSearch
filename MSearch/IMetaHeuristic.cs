using System;
using System.Collections.Generic;

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
