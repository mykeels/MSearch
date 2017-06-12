using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSearch.Extensions;

namespace MSearch
{
    public class Distribution
    {
        private static double _gamma(double x)
        {
            const double gamma = 0.577215664901532860606512090; // Euler's gamma constant
            if (x <= 0) throw new ArgumentOutOfRangeException("Invalid input argument {0}. Argument must be positive.", Convert.ToString(x));
            if (x < 0.001) return 1.0 / (x * (1.0 + gamma * x));
            if (x < 12.0)
            {
                double y = x;
                int n = 0;
                bool arg_was_less_than_one = (y < 1.0);
                if (arg_was_less_than_one) y += 1.0;
                else
                {
                    n = (int)(Math.Floor(y)) - 1;  // will use n later
                    y -= n;
                }

                // numerator coefficients for approximation over the interval (1,2)
                double[] p =
                {
                    -1.71618513886549492533811E+0,
                     2.47656508055759199108314E+1,
                    -3.79804256470945635097577E+2,
                     6.29331155312818442661052E+2,
                     8.66966202790413211295064E+2,
                    -3.14512729688483675254357E+4,
                    -3.61444134186911729807069E+4,
                     6.64561438202405440627855E+4
                };

                // denominator coefficients for approximation over the interval (1,2)
                double[] q =
                {
                    -3.08402300119738975254353E+1,
                     3.15350626979604161529144E+2,
                    -1.01515636749021914166146E+3,
                    -3.10777167157231109440444E+3,
                     2.25381184209801510330112E+4,
                     4.75584627752788110767815E+3,
                    -1.34659959864969306392456E+5,
                    -1.15132259675553483497211E+5
                };

                double num = 0.0;
                double den = 1.0;
                int i;

                double z = y - 1;
                for (i = 0; i < 8; i++)
                {
                    num = (num + p[i]) * z;
                    den = den * z + q[i];
                }
                double result = num / den + 1.0;

                // Apply correction if argument was not initially in (1,2)
                if (arg_was_less_than_one)
                {
                    // Use identity gamma(z) = gamma(z+1)/z
                    // The variable "result" now holds gamma of the original y + 1
                    // Thus we use y-1 to get back the orginal y.
                    result /= (y - 1.0);
                }
                else
                {
                    // Use the identity gamma(z+n) = z*(z+1)* ... *(z+n-1)*gamma(z)
                    for (i = 0; i < n; i++)
                        result *= y++;
                }

                return result;
            }

            ///////////////////////////////////////////////////////////////////////////
            // Third interval: [12, infinity)

            if (x > 171.624)
            {
                // Correct answer too large to display. 
                return double.PositiveInfinity;
            }

            return Math.Exp(_logGamma(x));
        }

        private static double _logGamma (double x)
        {
            if (x <= 0.0)
            {
                string msg = string.Format("Invalid input argument {0}. Argument must be positive.", x);
                throw new ArgumentOutOfRangeException(msg);
            }

            if (x < 12.0)
            {
                return Math.Log(Math.Abs(_gamma(x)));
            }

            // Abramowitz and Stegun 6.1.41
            // Asymptotic series should be good to at least 11 or 12 figures
            // For error analysis, see Whittiker and Watson
            // A Course in Modern Analysis (1927), page 252

            double[] c =
            {
                 1.0/12.0,
                -1.0/360.0,
                1.0/1260.0,
                -1.0/1680.0,
                1.0/1188.0,
                -691.0/360360.0,
                1.0/156.0,
                -3617.0/122400.0
            };
            double z = 1.0 / (x * x);
            double sum = c[7];
            for (int i = 6; i >= 0; i--)
            {
                sum *= z;
                sum += c[i];
            }
            double series = sum / x;

            double halfLogTwoPi = 0.91893853320467274178032973640562;
            double logGamma = (x - 0.5) * Math.Log(x) - x + halfLogTwoPi + series;
            return logGamma;
        }

        private static double _nextStdNormal()
        {
            double d1 = Math.Sqrt(Number.Rnd());
            double d2 = Math.Sqrt(Number.Rnd());
            return (double)(Math.Sqrt(-2.0 * Math.Log(d1)) * Math.Sin(2.0 * Math.PI * d2));
        }

        public static double generateLevy(double alpha = 0.075, double beta = 1.5)
        {
            double sigma = Math.Pow(_gamma(1.0 * beta) * Math.Sin(Math.PI * beta / 2.0) / (_gamma((1.0 * beta) / 2.0) * beta * Math.Pow(2.0, (beta - 1.0) / 2.0)), 1.0 / beta);
            return alpha * _nextStdNormal() * sigma / Math.Pow(Math.Abs(_nextStdNormal()), 1.0 / beta);
        }
    }
}

/*double kAlpha = 1 - Math.Abs(1 - alpha);
            double theta0 = -0.5 * beta * (kAlpha / alpha);
            double nBeta = alpha == 1 ? beta : -1 * (Math.Tan(0.5 * Math.PI * (1 - alpha)) * Math.Tan(alpha * theta0));
            double u = Number.Rnd();
            double theta = Math.PI * (u - 0.5);
            double e = 1 - alpha;
            //double t = -e * Math.Tan(alpha * theta0);
            double v = Number.Rnd();
            double w = -1 * Math.Log(v, Math.E);
            double z = Math.Cos(e * theta) - Math.Tan(alpha * theta0) * Math.Sin(e * theta0) / (w * Math.Cos(theta));
            //double d = Math.Pow(z, e / alpha) / e;
            return Math.Tan(alpha*theta0) + (Math.Pow(z, e / alpha) * (Math.Sin(alpha * theta) - (Math.Tan(alpha * theta0) * Math.Cos(alpha * theta))) / Math.Cos(theta));
*/