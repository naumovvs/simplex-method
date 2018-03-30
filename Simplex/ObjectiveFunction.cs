using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    /// <summary>
    /// Целевая функция
    /// </summary>
    public class ObjectiveFunction
    {
        private double[] coefficients;

        public int VariablesNumber
        { get { return this.coefficients.Length; } }

        public double[] Coefficients
        { get { return this.coefficients; } }

        public ObjectiveFunction(double[] coefficients)
        {
            this.coefficients = coefficients;
        }

        public double Value(double[] variables)
        {
            double value = 0;

            if (VariablesNumber != variables.Length)
                throw new ArgumentException("The number of variables (" + variables.Length + ") shoud be equal to the number of function coefficients (" + this.VariablesNumber + ").");

            if (this.coefficients.Length > 0)
                for (int i = 0; i < VariablesNumber; i++)
                    value += this.coefficients[i] * variables[i];

            return value;
        }
    }
}
