using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Tests.Problems;
using MSearch.HillClimb;

namespace MSearch.Tests.HillClimb
{
    public class HillClimb_Knapsack_Test: Knapsack
    {
        public List<int> solveWithHillClimb()
        {
            HillClimb<List<int>> hc = new HillClimb<List<int>>();
            hc.create(this.getConfiguration());
            return hc.fullIteration();
        }
    }
}
