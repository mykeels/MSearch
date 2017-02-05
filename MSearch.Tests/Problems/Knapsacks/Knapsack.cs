using System;
using System.Collections.Generic;
using System.Linq;
using MSearch.Extensions;
using MSearch.Common;
using Newtonsoft.Json;

namespace MSearch.Tests.Problems.Knapsacks
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

        public List<int> getInitialSolution() => getInitialSolution(1);

        public List<int> getInitialSolution(int length)
        {
            line1:
            List<int> ret = new List<int>();
            while (ret.Count < length)
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

        public Configuration<List<int>> getConfiguration()
        {
            Configuration<List<int>> config = new Configuration<List<int>>();
            config.cloneFunction = this.clone;
            config.initializeSolutionFunction = this.getInitialSolution;
            config.movement = Search.Direction.Divergence;
            config.mutationFunction = this.mutate;
            config.noOfIterations = 5000;
            config.objectiveFunction = this.getFitness;
            config.populationSize = 50;
            config.selectionFunction = Selection.RoulleteWheel;
            config.writeToConsole = true;
            config.consoleWriteInterval = 100;
            config.hardObjectiveFunction = (List<int> sol) =>
            {
                return this.getFitness(sol) < Double.MaxValue;
            };
            config.enforceHardObjective = true;
            return config;
        }
        
        public Knapsack Load(string filePath)
        {
            if (!filePath.EndsWith(".json")) throw new Exception($"File Path: [{filePath}] should match pattern *.json");
            var knapsack = JsonConvert.DeserializeObject<Knapsack>(System.IO.File.ReadAllText(filePath));
            this.goal = knapsack.goal;
            this.items = knapsack.items;
            this.noOfItems = knapsack.noOfItems;
            this.noOfKnapsacks = knapsack.noOfKnapsacks;
            this.weights = knapsack.weights;
            return this;
        }
    }
}
