using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplex;
using EquationSolve;

namespace SimplexTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectiveFunction objF = new ObjectiveFunction(new double[6] { 5, 4, 3,
                                                                           2, 3, 1});
            
            Constraint c1 = new Constraint(new double[6] { 1, 1, 1,
                                                           0, 0, 0 }, 10);
            
            Constraint c2 = new Constraint(new double[6] { 0, 0, 0,
                                                           1, 1, 1 }, 15);
            
            Constraint c3 = new Constraint(new double[6] { 1, 0, 0,
                                                           1, 0, 0 }, 10);
            
            Constraint c4 = new Constraint(new double[6] { 0, 1, 0,
                                                           0, 1, 0 }, 5);
            
            Constraint c5 = new Constraint(new double[6] { 0, 0, 1,
                                                           0, 0, 1 }, 5);

            LPP lpp = new LPP(objF, new Constraint[5] { c1, c2, c3, c4, c5});
                        
            lpp.Solve();

            //Solver solver = new Solver();
            //solver.LowBound = 1;
            //solver.HighBound = 100;
            //solver.Accuracy = 0.01;
            //solver.FuncCoefs = new double[5] { c1, c2, c3, c4, c5 };
            //solver.VarPows = new double[5] { 0, -0.336, -1.429, -1.054, -0.695 };

            //Console.WriteLine((int)solver.Solve());

            //try { Console.WriteLine(s.Solve()); }
            //catch (Exception e) { Console.WriteLine(e.Message); }

        }
    }
}
