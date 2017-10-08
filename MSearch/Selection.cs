using System;
using System.Collections.Generic;
using System.Linq;
using MSearch.Extensions;

namespace MSearch
{
    public class Selection
    {
        #region private methods
        private static List<double> GetFitnesses<SolutionType>(IEnumerable<SolutionType> Solutions, Func<SolutionType, double> fitnessFunction)
        {
            List<double> fitnesses = new List<double>();
            for (int i = 0; i < Solutions.Count(); i++)
            {
                fitnesses.Add(fitnessFunction(Solutions.ElementAt(i)));
            }
            return fitnesses;
        }

        private static IEnumerable<SolutionType> Rank<SolutionType>(IEnumerable<SolutionType> Solutions, Func<SolutionType, double> fitnessFunction)
        {
            List<SolutionType> ret = Solutions.ToList(); //Solutions should be CLONED
            ret.Sort(delegate (SolutionType sol1, SolutionType sol2)
            {
                return fitnessFunction(sol1).CompareTo(fitnessFunction(sol2));
            });
            return ret.AsEnumerable();
        }

        private static IEnumerable<SolutionType> Rank<SolutionType>(IEnumerable<SolutionType> Solutions, IEnumerable<double> fitnesses)
        {
            List<KeyValuePair<SolutionType, double>> ret = new List<KeyValuePair<SolutionType, double>>();
            List<SolutionType> sols = Solutions.ToList(); //Solutions should be CLONED

            { //populate the keyvalue list
                int index = 0;
                sols.ForEach((sol) =>
                {
                    ret.Add(new KeyValuePair<SolutionType, double>(sol, fitnesses.ElementAt(index)));
                    index++;
                });
            }
            { //now sort ret by its fitnesses
                ret.Sort(delegate (KeyValuePair<SolutionType, double> sol1, KeyValuePair<SolutionType, double> sol2)
                {
                    return sol1.Value.CompareTo(sol2.Value);
                });
            }
            return ret.Select((item) => item.Key);
        }
        #endregion

        #region Roullete Wheel Selection Method
        public static IEnumerable<SolutionType> RoulleteWheel<SolutionType>(IEnumerable<SolutionType> Solutions,
            IEnumerable<double> fitnesses, int selectCount = 1)
        {
            List<SolutionType> ret = new List<SolutionType>();
            double sum = fitnesses.Sum();
            while (ret.Count < selectCount)
            {
                for (int i = 0; i < fitnesses.Count(); i++)
                {
                    if (ret.Count >= selectCount) return ret.AsEnumerable();
                    double probability = fitnesses.ElementAt(i) / sum;
                    double rnd = Number.Rnd();
                    if (rnd < probability) ret.Add(Solutions.ElementAt(i));
                }
            }
            return ret.AsEnumerable();
        }

        public static IEnumerable<SolutionType> RoulleteWheel<SolutionType>(IEnumerable<SolutionType> Solutions,
            Func<SolutionType, double> fitnessFunction, int selectCount = 1)
        {
            List<SolutionType> ret = new List<SolutionType>();
            List<double> fitnesses = GetFitnesses(Solutions, fitnessFunction);
            double sum = fitnesses.Sum();
            while (ret.Count < selectCount)
            {
                for (int i = 0; i < fitnesses.Count(); i++)
                {
                    if (ret.Count >= selectCount) return ret.AsEnumerable();
                    double probability = fitnesses[i] / sum;
                    double rnd = Number.Rnd();
                    if (rnd < probability) ret.Add(Solutions.ElementAt(i));
                }
            }
            return ret.AsEnumerable();
        }
        #endregion

        #region Rank Based Selection Method
        public static IEnumerable<SolutionType> RankBased<SolutionType>(IEnumerable<SolutionType> Solutions,
            IEnumerable<double> fitnesses, int selectCount = 1)
        {
            return RoulleteWheel(Rank(Solutions, fitnesses), fitnesses);
        }

        public static IEnumerable<SolutionType> RankBased<SolutionType>(IEnumerable<SolutionType> Solutions,
            Func<SolutionType, double> fitnessFunction, int selectCount = 1)
        {
            return RoulleteWheel(Rank(Solutions, fitnessFunction), fitnessFunction);
        }
        #endregion

        #region Stochastic Selection Method
        public static IEnumerable<SolutionType> Stochastic<SolutionType>(IEnumerable<SolutionType> Solutions,
            IEnumerable<double> fitnesses, int selectCount = 1)
        {
            List<SolutionType> ret = new List<SolutionType>();
            for (int i = 0; i < fitnesses.Count(); i++)
            {
                if (ret.Count >= selectCount) return ret.AsEnumerable();
                int j = 1;
                while (fitnesses.Take(j).Sum() < fitnesses.ElementAt(i)) j++;
                ret.Add(Solutions.ElementAt(j));
            }
            return ret.AsEnumerable();
        }

        public static IEnumerable<SolutionType> Stochastic<SolutionType>(IEnumerable<SolutionType> Solutions,
            Func<SolutionType, double> fitnessFunction, int selectCount = 1)
        {
            List<SolutionType> ret = new List<SolutionType>();
            List<double> fitnesses = GetFitnesses(Solutions, fitnessFunction);
            for (int i = 0; i < fitnesses.Count; i++)
            {
                if (ret.Count >= selectCount) return ret.AsEnumerable();
                int j = 1;
                while (fitnesses.Take(j).Sum() < fitnesses[i]) j++;
                ret.Add(Solutions.ElementAt(j));
            }
            return ret.AsEnumerable();
        }
        #endregion

        #region Tournament Selection Method
        public static IEnumerable<SolutionType> Tournament<SolutionType>(IEnumerable<SolutionType> Solutions,
            IEnumerable<double> fitnesses, double probability, int tournamentSize, int selectCount = 1)
        {
            List<SolutionType> ret = new List<SolutionType>();
            if (tournamentSize < 2) throw new Exception("Dude Naaa! Haba. Tournament Size should be >= 2");
            while (ret.Count < selectCount)
            {
                IEnumerable<SolutionType> contestants = Solutions.Random(tournamentSize);
                contestants = Rank(contestants, fitnesses);
                double addend = 1;
                for (int i = 0; i < contestants.Count(); i++)
                {
                    if (Number.Rnd() < probability) ret.Add(contestants.ElementAt(i));
                    addend *= (1 - probability);
                    probability = probability * addend;
                }
            }
            return ret;
        }

        public static IEnumerable<SolutionType> Tournament<SolutionType>(IEnumerable<SolutionType> Solutions,
            Func<SolutionType, double> fitnessFunction, double probability, int tournamentSize, int selectCount = 1)
        {
            List<SolutionType> ret = new List<SolutionType>();
            if (tournamentSize < 2) throw new Exception("Dude Naaa! Haba. Tournament Size should be >= 2");
            while (ret.Count < selectCount)
            {
                IEnumerable<SolutionType> contestants = Solutions.Random(tournamentSize);
                contestants = Rank(contestants, fitnessFunction);
                double addend = 1;
                for (int i = 0; i < contestants.Count(); i++)
                {
                    if (Number.Rnd() < probability) ret.Add(contestants.ElementAt(i));
                    addend *= (1 - probability);
                    probability = probability * addend;
                }
            }
            return ret;
        }
        #endregion
    }
}
