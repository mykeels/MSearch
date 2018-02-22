using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MSearch.Tests
{
    public class Distribution_Tests
    {
        [Fact]
        public void Test_That_Normal_Levy_Distribution_Works()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Normal Distribution:\t{Distribution.generateLevy()}");
            }
        }
    }
}
