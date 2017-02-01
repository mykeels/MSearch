using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.ABC;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSearch.Tests.ABC
{
    [TestClass]
    public class ABC_Test
    {
        [TestMethod]
        [Description("Test That FailureLimit and AcceptProbability are defaults when instantiated with default constructor")]
        public void Test_That_FailureLimit_And_AcceptProbability_Are_Defaults()
        {
            Hive<int[], Bee<int[]>> hive = new Hive<int[], Bee<int[]>>();
            Assert.AreEqual(hive.getFailureLimit(), 20);
            Assert.AreEqual(hive.getAcceptanceProbability(), 0.4);
            Console.WriteLine("Hive:\n" + JsonConvert.SerializeObject(hive, Formatting.Indented));
            Console.WriteLine("Hive Failure Limit:\t" + hive.getFailureLimit());
            Console.WriteLine("Hive Acceptance Probability:\t" + hive.getAcceptanceProbability());
        }
    }
}
