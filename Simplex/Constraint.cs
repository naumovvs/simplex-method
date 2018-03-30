using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    public class Constraint
    {
        private double[] coefficients;
        private double restriction;
        private int coefficientsNumber;

        public double[] Coefficients
        { get { return this.coefficients; } }

        public double Restriction
        { get { return this.restriction; } }

        public Constraint(double[] coefficients, double restriction)
        {
            this.coefficientsNumber = coefficients.Length;
            this.coefficients = coefficients;
            this.restriction = restriction;
        }
    }

}
