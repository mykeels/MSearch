using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSearch.Tests
{
    [TestClass]
    public class Distribution_Tests
    {
        [TestMethod]
        public void Test_That_Normal_Levy_Distribution_Works()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Normal Distribution:\t{Distribution.generateLevy()}");
            }
        }
    }
}
