//=====================================================================
//
//  File: iterative-sparse-solvers.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

using Numerics.NET.Data.Text;

using Numerics.NET;
// Sparse matrices are in the Numerics.NET.LinearAlgebra
// namespace
using Numerics.NET.LinearAlgebra;
using Numerics.NET.LinearAlgebra.IterativeSolvers;
using Numerics.NET.LinearAlgebra.IterativeSolvers.Preconditioners;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates the use of iterative sparse solvers for efficiently
    /// solving large, sparse systems of linear equations using the
    /// iterative sparse solver and preconditioner classes from the
    /// Numerics.NET.LinearAlgebra.IterativeSolvers namespace of Numerics.NET.
    /// </summary>
    class IterativeSparseSolvers
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // This QuickStart Sample illustrates how to solve sparse linear systems
            // using iterative solvers.

            // IterativeSparseSolver is the base class for all
            // iterative solver classes:

            //
            // Non-symmetric systems
            //

            Console.WriteLine("Non-symmetric systems");

            // We load a sparse matrix and right-hand side from a data file:

            var A = (SparseMatrix<double>)MatrixMarketFile.ReadMatrix<double>
                (@"..\..\..\..\..\data\sherman3.mtx");
            var b = MatrixMarketFile.ReadVector<double>(
                @"..\..\..\..\..\data\sherman3_rhs1.mtx");

            Console.WriteLine("Solve Ax = b");
            Console.WriteLine("A is {0}x{1} with {2} nonzeros.", A.RowCount, A.ColumnCount, A.NonzeroCount);

            // Some solvers are suitable for symmetric matrices only.
            // Our matrix is not symmetric, so we need a solver that
            // can handle this:
            IterativeSparseSolver<double> solver = new BiConjugateGradientSolver<double>(A);

            solver.Solve(b);
            Console.WriteLine($"Solved in {solver.IterationsNeeded} iterations.");
            Console.WriteLine($"Estimated error: {solver.SolutionReport.Error}");

            // Using a preconditioner can improve convergence. You can use
            // one of the predefined preconditioners, or supply your own.

            // With incomplete LU preconditioner
            solver.Preconditioner = new IncompleteLUPreconditioner<double>(A);
            solver.Solve(b);
            Console.WriteLine($"Solved in {solver.IterationsNeeded} iterations.");
            Console.WriteLine($"Estimated error: {solver.EstimatedError}");

            //
            // Symmetrical systems
            //

            Console.WriteLine("Symmetric systems");

            // In this example we solve the Laplace equation on a rectangular grid
            // with Dirichlet boundary conditions.

            // We create 100 divisions in each direction, giving us 99 interior points
            // in each direction:
            int nx = 99;
            int ny = 99;

            // The boundary conditions are just some arbitrary functions.
            var left = Vector.CreateFromFunction(ny,
                i => { double x = (i / (nx - 1.0)); return x * x; });
            var right = Vector.CreateFromFunction(ny,
                i => { double x = (i / (nx - 1.0)); return 1 - x; });
            var top = Vector.CreateFromFunction(nx,
                i => { double x = (i / (nx - 1.0)); return Elementary.SinPi(5 * x); });
            var bottom = Vector.CreateFromFunction(nx,
                i => { double x = (i / (nx - 1.0)); return Elementary.CosPi(5 * x); });

            // We discretize the Laplace operator using the 5 point stencil.
            var laplacian = Matrix.CreateSparse<double>(nx * ny, nx * ny, 5 * nx * ny);
            var rhs = Vector.Create<double>(nx * ny);
            for (int j = 0; j < ny; j++) {
                for (int i = 0; i < nx; i++) {
                    int ix = j * nx + i;
                    if (j > 0)
                        laplacian[ix, ix - nx] = 0.25;
                    if (i > 0)
                        laplacian[ix, ix - 1] = 0.25;
                    laplacian[ix, ix] = -1.0;
                    if (i + 1 < nx)
                        laplacian[ix, ix + 1] = 0.25;
                    if (j + 1 < ny)
                        laplacian[ix, ix + nx] = 0.25;
                }
            }
            // We build up the right-hand sides using the boundary conditions:
            for (int i = 0; i < nx; i++) {
                rhs[i] = -0.25 * top[i];
                rhs[nx * (ny - 1) + i] = -0.25 * bottom[i];
            }
            for (int j = 0; j < ny; j++) {
                rhs[j * nx] -= 0.25 * left[j];
                rhs[j * nx + nx - 1] -= 0.25 * right[j];
            }

            // Finally, we create an iterative solver suitable for
            // symmetric systems...
            solver = new QuasiMinimalResidualSolver<double>(laplacian);
            // and solve using the right-hand side we just calculated:
            solver.Solve(rhs);

            Console.WriteLine("Solve Ax = b");
            Console.WriteLine("A is {0}x{1} with {2} nonzeros.", A.RowCount, A.ColumnCount, A.NonzeroCount);
            Console.WriteLine($"Solved in {solver.IterationsNeeded} iterations.");
            Console.WriteLine($"Estimated error: {solver.EstimatedError}");

        }
    }
}
