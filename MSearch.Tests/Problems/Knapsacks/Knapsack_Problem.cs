using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSearch.Tests.Problems.Knapsacks
{
    public class Knapsack_Problem
    {
        public List<Knapsack> readProblem(string filename)
        {
            int countMaxWeights = 0;
            int countItemsWeights = 0;
            int stage = 0;
            string[] lines = System.IO.File.ReadAllLines(filename);
            var ret = new List<Knapsack>();
            Knapsack knapsack = new Knapsack();
            int knapsacks_count = Convert.ToInt32(lines.FirstOrDefault());
            for (int k = 1; k <= knapsacks_count; k++)
            {
                int i = 1;
                foreach (string s_loopVariable in lines.Skip(1))
                {
                    string s = s_loopVariable;
                    s = s.Trim();
                    string[] ss = s.Split(' ');
                    if (i == 1)
                    {
                        knapsack.noOfKnapsacks = Convert.ToInt32(ss[1]);
                        knapsack.noOfItems = Convert.ToInt32(ss[0]);
                        if (ss.Length > 2)
                        {
                            knapsack.goal = Convert.ToInt32(ss[2]);
                        }
                        i += 1;
                        //first stage
                    }
                    else if (i == 2)
                    {
                        if (knapsack.items.Count() < knapsack.noOfItems)
                        {
                            foreach (string sss_loopVariable in ss)
                            {
                                string sss = sss_loopVariable;
                                knapsack.items.Add(new Knapsack.Item(Convert.ToDouble(sss), new List<double>()));
                            }
                        }
                        else
                        {
                            i += 1;
                        }
                    }
                    if (i == 3)
                    {
                        if (countItemsWeights < knapsack.noOfKnapsacks)
                        {
                            for (int index = 0; index <= ss.Length - 1; index++)
                            {
                                knapsack.items[stage].weights.Add(Convert.ToDouble(ss[index]));
                                stage += 1;
                            }
                            if (stage == knapsack.noOfItems)
                            {
                                countItemsWeights += 1;
                                stage = 0;
                            }

                        }
                        else
                        {
                            i += 1;
                        }
                    }
                    if (i == 4)
                    {
                        if (countMaxWeights < knapsack.noOfKnapsacks)
                        {
                            foreach (string sss_loopVariable in ss)
                            {
                                string sss = sss_loopVariable;
                                knapsack.weights.Add(Convert.ToDouble(sss));
                                countMaxWeights += 1;
                            }
                        }
                        else
                        {
                            i += 1;
                        }
                    }
                }
                ret.Add(knapsack);
            }
            return ret;
        }

        public class BestResult
        {
            public string filename { get; set; }
            public int best { get; set; }
            public int index { get; set; }
        }
    }
}
