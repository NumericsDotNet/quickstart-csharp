//=====================================================================
//
//  File: nonlinear-programming.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// Vectors and matrices are in the Numerics.NET namespace
using Numerics.NET;
// The nonlinear programming classes reside in a namespace with
// other optimization-related classes.
using Numerics.NET.Optimization;

namespace Numerics.NET.QuickStart.CSharp {
    /// <summary>
    /// Illustrates solving nonlinear programming problems
    /// using the classes in the Numerics.NET.Optimization
    /// namespace of Numerics.NET.
    /// </summary>
    class NonlinearProgramming {
        static void Main(string[] args) {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // This QuickStart Sample illustrates the two ways to create a Nonlinear Program.

            // The first is in terms of matrices. The coefficients
            // are supplied as a matrix. The cost vector, right-hand side
            // and constraints on the variables are supplied as a vector.

            Console.WriteLine("Problem with only linear constraints:");


            // The variables are the concentrations of each chemical compound:
            // H, H2, H2O, N, N2, NH, NO, O, O2, OH

            // The objective function is the free energy, which we want to minimize:
            var c = Vector.Create(
                -6.089, -17.164, -34.054, -5.914, -24.721,
                -14.986, -24.100, -10.708, -26.662, -22.179);
            Func<Vector<double>, double> objectiveFunction =
                x => x.DotProduct(c + Vector.Log(x) - Math.Log(x.Sum()));
            Func<Vector<double>, Vector<double>, Vector<double>> objectiveGradient = (x, y) =>
            {
                double s = x.Sum();
                // y = c + log(x) - log(s)
                // return (c + Vector.Log(x) - Math.Log(s)).CopyTo(y);
                return Vector.LogInto(x, y).AddInPlace(c - Math.Log(s));
            };

            // The constraints are the mass balance relationships for each element.
            // The rows correspond to the elements H, N, and O.
            // The columns are the index of the variable.
            // The value is the number of times the element occurs in the
            // compound corresponding to the variable:
            // H, H2, H2O, N, N2, NH, NO, O, O2, OH
            // All this is stored in a sparse matrix, so 0 values are omitted:
            var A = Matrix.CreateSparse(3, 10,
                new int[]    { 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2 },
                new int[]    { 0, 1, 2, 5, 9, 3, 4, 5, 6, 2, 6, 7, 8, 9 },
                new double[] { 1, 2, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1 });
            // The right-hand sides are the atomic weights of the elements
            // in the mixture.
            var b = Vector.Create(new double[] { 2, 1, 1 });

            // The number of moles for each compound must be positive.
            var l = Vector.CreateConstant(10, 1e-6);
            var u = Vector.CreateConstant(10, double.PositiveInfinity);

            // We create the nonlinear program with the specified constraints:
            // Because we have variable bounds, we use the constructor
            // that lets us do this.
            var nlp1 = new NonlinearProgram(objectiveFunction, objectiveGradient, A, b, b, l, u);

            // We could add more (linear or nonlinear) constraints here,
            // but this is all we have in our problem.

            nlp1.InitialGuess = Vector.CreateConstant(10, 0.1);
            var solution = nlp1.Solve();
            Console.WriteLine($"Solution: {solution:F3}");
            Console.WriteLine($"Optimal value:   {nlp1.OptimalValue:F5}");
            Console.WriteLine($"# iterations: {nlp1.SolutionReport.IterationsNeeded}");
            Console.WriteLine();

            // The second method is building the nonlinear program from scratch.

            Console.WriteLine("Problem with nonlinear constraints:");

            // We start by creating a nonlinear program object. We supply
            // the number of variables in the constructor.
            var nlp2 = new NonlinearProgram(2);

            nlp2.ObjectiveFunction = x => Math.Exp(x[0]) * (4 * x[0] * x[0] + 2 * x[1] * x[1] + 4 * x[0] * x[1] + 2 * x[1] + 1);
            nlp2.ObjectiveGradient = (x, y) => {
                double exp = Math.Exp(x[0]);
                y[0] = exp * (4 * x[0] * x[0] + 2 * x[1] * x[1] + 4 * x[0] * x[1] + 8 * x[0] + 6 * x[1] + 1);
                y[1] = exp * (4 * x[0] + 4 * x[1] + 2);
                return y;
            };

            // Add constraint x0*x1 - x0 -x1 <= -1.5
            nlp2.AddNonlinearConstraint(x => x[0] * x[1] - x[0] - x[1] + 1.5, ConstraintType.LessThanOrEqual, 0.0,
                (x, y) => { y[0] = x[1] - 1; y[1] = x[0] - 1; return y; });

            // Add constraint x0*x1 >= -10
            // If the gradient is omitted, it is approximated using divided differences.
            nlp2.AddNonlinearConstraint(x => x[0] * x[1], ConstraintType.GreaterThanOrEqual, -10.0);

            nlp2.InitialGuess = Vector.Create(-1.0, 1.0);

            solution = nlp2.Solve();
            Console.WriteLine($"Solution: {solution:F6}");
            Console.WriteLine($"Optimal value:   {nlp2.OptimalValue:F6}");
            Console.WriteLine($"# iterations: {nlp2.SolutionReport.IterationsNeeded}");

            // We can use Automatic Differentiation to compute the gradients.
            // This makes our life a lot easier.
            var nlp3 = new NonlinearProgram(2);

            nlp3.SymbolicObjectiveFunction = x => Math.Exp(x[0]) * (4 * x[0] * x[0] + 2 * x[1] * x[1] + 4 * x[0] * x[1] + 2 * x[1] + 1);

            // Add constraint x0*x1 - x0 -x1 <= -1.5
            nlp3.AddSymbolicConstraint(x => x[0] * x[1] - x[0] - x[1] + 1.5, ConstraintType.LessThanOrEqual, 0.0);
            // Add constraint x0*x1 >= -10
            nlp3.AddSymbolicConstraint(x => x[0] * x[1], ConstraintType.GreaterThanOrEqual, -10.0);

            nlp3.InitialGuess = Vector.Create(-1.0, 1.0);

            solution = nlp3.Solve();
            Console.WriteLine($"Solution: {solution:F6}");
            Console.WriteLine($"Optimal value:   {nlp3.OptimalValue:F6}");
            Console.WriteLine($"# iterations: {nlp3.SolutionReport.IterationsNeeded}");

        }
    }
}
