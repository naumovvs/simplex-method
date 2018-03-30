using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    public class LPP
    {
        public ObjectiveFunction ObjFunc;
        public Constraint[] Constraints;
        public double[] Variables;

        public LPP(ObjectiveFunction objFunc, Constraint[] constraints)
        {
            this.ObjFunc = objFunc;
            this.Constraints = constraints;
            this.Variables = new double[ObjFunc.VariablesNumber];
        }

        public bool SolutionFound(Dictionary d)
        {
            return d.EntersBasis() == -1; // || d.LeavesBasis(d.EntersBasis()) != -1 (?)
        }

        public void Solve()
        {
            Dictionary dict = new Dictionary(this);

            if (!dict.IsFeasible()) dict = this.initialize();

            Console.WriteLine("Finding solution...");
            Console.WriteLine("-------------------------------");
            Console.WriteLine();

            while (!SolutionFound(dict))
            {
                dict.Print();
                dict.Improve();
            }
            dict.Print();

            for (int i = 0; i < dict.basic.Length; i++)
                if (dict.basic[i] < Variables.Length + 1)
                    Variables[dict.basic[i] - 1] = 0;
            for (int i = 0; i < dict.slack.Length; i++)
                if (dict.slack[i] < Variables.Length + 1)
                    Variables[dict.slack[i] - 1] = dict.c[i, 0];
            
        }

        private Dictionary initialize()
        {
            Console.WriteLine("Initialization phase...");
            Console.WriteLine("-------------------------------");
            Console.WriteLine();

            double[] auxC = new double[ObjFunc.VariablesNumber + 1];
            auxC[0] = -1;
            for (int i = 0; i < auxC.Length - 1; i++) auxC[i + 1] = 0;
            ObjectiveFunction auxOF = new ObjectiveFunction(auxC);

            Constraint[] auxCS = new Constraint[this.Constraints.Length];
            int leavesBasis = 0;
            double minB = Constraints[0].Restriction;
            for (int i = 0; i < auxCS.Length; i++)
            {
                double[] auxCC = new double[ObjFunc.VariablesNumber + 1];
                auxCC[0] = -1;
                for (int j = 0; j < auxCC.Length - 1; j++)
                    auxCC[j + 1] = Constraints[i].Coefficients[j];
                auxCS[i] = new Constraint(auxCC, Constraints[i].Restriction);
                if (Constraints[i].Restriction < minB) { minB = Constraints[i].Restriction; leavesBasis = i; }
            }

            LPP auxLPP = new LPP(auxOF, auxCS);
            Dictionary auxD = new Dictionary(auxLPP);
            auxD.Print(false);
            auxD.Recalculate(0, leavesBasis);
            while (!SolutionFound(auxD))
            {
                auxD.Print(preferToLeave: 1);
                auxD.Improve(preferToLeave: 1);
            }
            auxD.Print(preferToLeave: 1);

            int len = auxD.basic.Length;
            int[] bb = new int[len];
            double[,] cc = new double[auxLPP.Constraints.Length, len + 1];
            int[] ss = new int[auxLPP.Constraints.Length];
            for (int i = 0; i < len; i++)
            {
                bb[i] = auxD.basic[i];
                for (int j = 0; j < auxLPP.Constraints.Length; j++) cc[j, i + 1] = auxD.c[j, i + 1];
            }
            for (int j = 0; j < auxLPP.Constraints.Length; j++)
            { cc[j, 0] = auxD.c[j, 0]; ss[j] = auxD.slack[j]; }

            auxD.a = new double[len - 1];
            auxD.basic = new int[len - 1];
            auxD.slack = new int[auxLPP.Constraints.Length];
            auxD.c = new double[auxLPP.Constraints.Length, len];

            for (int i = 0; i < auxLPP.Constraints.Length; i++)
            {
                auxD.c[i, 0] = cc[i, 0];
                int j = 1;
                while (bb[j - 1] != 1) { auxD.c[i, j] = cc[i, j]; j++; }
                while (j < bb.Length) { j++; auxD.c[i, j - 1] = cc[i, j]; }
                auxD.slack[i] = ss[i] - 1;
            }
            int k = 0;
            while (bb[k] != 1) { auxD.basic[k] = bb[k] - 1; k++; }
            k++;
            while (k < bb.Length) { auxD.basic[k - 1] = bb[k] - 1; k++; }

            auxD.z0 = 0;
            for (int i = 0; i < this.ObjFunc.Coefficients.Length; i++)
                for (int j = 0; j < auxD.slack.Length; j++)
                    if (auxD.slack[j] == i + 1)
                    {
                        auxD.z0 += ObjFunc.Coefficients[i] * auxD.c[j, 0];
                        for (int m = 0; m < ObjFunc.Coefficients.Length; m++)
                            auxD.a[m] += ObjFunc.Coefficients[i] * auxD.c[j, m + 1];
                    }
            auxD.Print(false);
            
            Console.WriteLine();
            
            return auxD;
            
        }
    }
}


