using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Tests.Problems;
using MSearch.SA;

namespace MSearch.Tests.SA
{
    public class SimulatedAnnealing_Knapsack_Test: Knapsack
    {
        public List<int> solveWithSimulatedAnnealing()
        {
            SimulatedAnnealing<List<int>> sa = new SimulatedAnnealing<List<int>>();
            sa.create(this.getConfiguration());
            return sa.fullIteration();
        }
    }
}
