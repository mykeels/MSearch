using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Extensions;
using MSearch.Common;

namespace MSearch.Tests.Problems
{
    public partial class Knapsack
    {
        public List<Item> items = new List<Item>();
        public List<double> weights = new List<double>();
        public int noOfKnapsacks, noOfItems;
        public int goal = 0;

        public class Item
        {
            public double value { get; set; }
            public List<double> weights { get; set; }

            public Item()
            {

            }

            public Item(double value, List<double> weights)
            {
                this.value = value;
                this.weights = weights;
            }

            public bool Equals(Item item)
            {
                if (item.value == this.value && item.weights.Count == this.weights.Count)
                {
                    for (int i = 0; i < item.weights.Count; i++)
                    {
                        if (item.weights[i] != this.weights[i]) return false;
                    }
                    return true;
                }
                return false;
            }
        }

        public List<int> getInitialSolution()
        {
            line1:
            List<int> ret = new List<int>();
            while (ret.Count < 1)
            {
                int x = (int)Math.Floor(Number.Rnd() * items.Count);
                if (!ret.Contains(x)) ret.Add(x);
            }
            if (this.getFitness(ret) == Double.MaxValue)
            {
                ret.Clear();
                goto line1;
            }
            return ret.Distinct().ToList();
        }

        public double getTotalWeight(List<int> solution, int kIndex)
        {
            double sumWeight = 0;
            foreach (int i in solution)
            {
                sumWeight += items[i].weights[kIndex];
            }
            return sumWeight;
        }

        public double getFitness(IEnumerable<int> solution)
        {
            double sumValue = 0;
            List<double> sumWeights = new List<double>();
            for (int i = 0; i < weights.Count; i++)
            {
                sumWeights.Add(0);
            }
            foreach (int i in solution)
            {
                int xi = 0;
                foreach (var weight in items[i].weights)
                {
                    sumWeights[xi] += weight;
                    xi++;
                }
                sumValue += items[i].value;
            }
            bool allKnapsacksOkay = true;
            int wi = 0;
            foreach (var weight in sumWeights)
            {
                if (weight > weights[wi]) allKnapsacksOkay = false;
                wi++;
            }
            if (allKnapsacksOkay) return sumValue;
            else return Double.MaxValue;
        }

        public List<int> getKnapsackIndicesThatSupportWeight(double weight)
        {
            List<int> ret = new List<int>();
            for (int index = 0; index < items.Count; index++)
            {
                if (items[index].weights.First() < weight) ret.Add(index);
            }
            return ret;
        }

        public List<int> mutate(List<int> solution)
        {
            double r = Number.Rnd();
            List<int> sol = null;
            if (r < 0.3) sol = replaceTwoWithOne(solution).ToList();
            else if (r < 0.6) sol = replaceOneWithTwo(solution).ToList();
            else sol = replaceOneWithOne(solution).ToList();
            return sol;
        }

        private IEnumerable<int> replaceOneWithOne(IEnumerable<int> solution)
        {
            List<int> ret = clone(solution.ToList()).ToList();
            if (ret != null)
            {
                if (ret.Count >= 1)
                {
                    int i = (int)Math.Floor(Number.Rnd() * solution.Count());
                    int val = (int)Math.Floor(Number.Rnd() * items.Count);
                    if (!solution.Contains(val)) ret[i] = val;
                }
            }
            return ret.Distinct().AsEnumerable();
        }

        private IEnumerable<int> replaceOneWithTwo(IEnumerable<int> solution)
        {
            if (solution != null)
            {
                if (solution.Count() >= 1)
                {
                    List<int> ret = clone(solution.ToList()).ToList();
                    int i = (int)Math.Floor(Number.Rnd() * ret.Count());
                    line1:
                    int val1 = (int)Math.Floor(Number.Rnd() * items.Count());
                    int val2 = (int)Math.Floor(Number.Rnd() * items.Count());
                    if (val1 == val2 || ret.Contains(val1) || ret.Contains(val2)) goto line1;
                    ret[i] = val1;
                    ret.Insert(i, val2);
                    return ret.Distinct().AsEnumerable();
                }
                throw new Exception("No of items in Solution should be at least 1");
            }
            throw new Exception("Solution should not be null");
        }

        private IEnumerable<int> replaceTwoWithOne(IEnumerable<int> solution)
        {
            List<int> ret = clone(solution.ToList()).ToList();
            if (ret != null)
            {
                if (ret.Count > 1)
                {
                    line1:
                    int i1 = (int)Math.Floor(Number.Rnd() * ret.Count);
                    int i2 = (int)Math.Floor(Number.Rnd() * ret.Count);
                    if (i1 == i2)
                    {
                        goto line1;
                    }
                    ret.RemoveAt(Math.Max(i1, i2));
                    ret.RemoveAt(Math.Min(i1, i2));
                    int val = (int)Math.Floor(Number.Rnd() * items.Count);
                    ret.Insert(Math.Min(i1, i2), val);
                }
            }
            return ret.Distinct().AsEnumerable();
        }

        public List<int> clone(List<int> solution)
        {
            List<int> ret = new List<int>();
            foreach (int x in solution)
            {
                ret.Add(x + 0);
            }
            return ret;
        }

        public Knapsack readProblemTypeOne(string filepath)
        {
            int i = 0;
            int countItemValues = 0, countItemsWeights = 0;
            List<Item> retItems = new List<Item>();
            int knapsackCapacitiesLineReached = 0;

            string[] lines = System.IO.File.ReadAllLines(filepath);
            for (int index = 0; index < lines.Length; index++)
            {
                string line = lines[index];
                line = line.TrimStart(' ');
                string[] ss = line.Split(' ');
                if (i == 0)
                {
                    this.noOfKnapsacks = Convert.ToInt32(ss[0]);
                    this.noOfItems = Convert.ToInt32(ss[1]);
                }
                else
                {
                    if (countItemValues < this.noOfItems)
                    {
                        foreach (string sss in ss)
                        {
                            if (sss == "//") break;
                            else
                            {
                                if (sss != "")
                                {
                                    Item item = new Item(Convert.ToDouble(sss), new List<double>());
                                    this.items.Add(item);
                                    countItemValues++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (knapsackCapacitiesLineReached < this.noOfKnapsacks)
                        {
                            foreach (string sss in ss)
                            {
                                if (sss == "//") break;
                                else
                                {
                                    if (sss != "")
                                    {
                                        this.weights.Add(Convert.ToDouble(sss));
                                        knapsackCapacitiesLineReached++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //last lines
                            if (countItemsWeights < this.noOfItems && i < lines.Count() - 1)
                            {
                                foreach (string sss in ss)
                                {
                                    if (sss == "//") break;
                                    else
                                    {
                                        if (sss != "")
                                        {
                                            this.items[countItemsWeights].weights.Add(Convert.ToDouble(sss));
                                            countItemsWeights++;
                                            if (countItemsWeights == this.noOfItems) countItemsWeights = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (i == lines.Count() - 1)
                {
                    string[] sss = line.Split(' ');
                    foreach (string ssss in sss)
                    {
                        if (ssss != "")
                        {
                            this.goal = Convert.ToInt32(ssss);
                            break;
                        }
                    }
                }
                i++;
            }
            return this;
        }

        public Knapsack readProblemTypeTwo(string filename)
        {
            int i = 0;
            int countMaxWeights = 0;
            int countItemsWeights = 0;
            int stage = 0;
            string[] lines = System.IO.File.ReadAllLines(filename);
            foreach (string s_loopVariable in lines)
            {
                string s = s_loopVariable;
                s = s.Trim();
                string[] ss = s.Split(' ');
                if (i == 1)
                {
                    this.noOfKnapsacks = Convert.ToInt32(ss[1]);
                    this.noOfItems = Convert.ToInt32(ss[0]);
                    if (ss.Length > 2)
                    {
                        this.goal = Convert.ToInt32(ss[2]);
                    }
                    i += 1;
                    //first stage
                }
                else if (i == 0) { i++; }
                else if (i == 2)
                {
                    if (this.items.Count() < this.noOfItems)
                    {
                        foreach (string sss_loopVariable in ss)
                        {
                            string sss = sss_loopVariable;
                            this.items.Add(new Item(Convert.ToDouble(sss), new List<double>()));
                        }
                    }
                    else
                    {
                        i += 1;
                    }
                }
                if (i == 3)
                {
                    if (countItemsWeights < this.noOfKnapsacks)
                    {
                        for (int index = 0; index <= ss.Length - 1; index++)
                        {
                            this.items[stage].weights.Add(Convert.ToDouble(ss[index]));
                            stage += 1;
                        }
                        if (stage == this.noOfItems)
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
                    if (countMaxWeights < this.noOfKnapsacks)
                    {
                        foreach (string sss_loopVariable in ss)
                        {
                            string sss = sss_loopVariable;
                            this.weights.Add(Convert.ToDouble(sss));
                            countMaxWeights += 1;
                        }
                    }
                    else
                    {
                        i += 1;
                    }
                }
            }
            return this;
        }

        public Configuration<List<int>> getConfiguration()
        {
            Configuration<List<int>> config = new Configuration<List<int>>();
            config.cloneFunction = this.clone;
            config.initializeSolutionFunction = this.getInitialSolution;
            config.movement = Search.Direction.Divergence;
            config.mutationFunction = this.mutate;
            config.noOfIterations = 100500;
            config.objectiveFunction = this.getFitness;
            config.populationSize = 50;
            config.selectionFunction = Selection.RoulleteWheel;
            config.writeToConsole = true;
            config.hardObjectiveFunction = (List<int> sol) =>
            {
                return this.getFitness(sol) < Double.MaxValue;
            };
            config.enforceHardObjective = true;
            return config;
        }
    }
}
