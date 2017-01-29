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
        public static double generateLevy(double alpha = 2, double beta = 0)
        {
            double kAlpha = 1 - Math.Abs(1 - alpha);
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
        }
    }
}
