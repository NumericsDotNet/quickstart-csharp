//=====================================================================
//
//  File: structured-linear-equations.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// The structured matrix classes reside in the
// Numerics.NET.LinearAlgebra namespace.
using Numerics.NET;
using Numerics.NET.LinearAlgebra;

// Illustrates solving symmetrical and triangular systems
// of simultaneous linear equations using classes
// in the Numerics.NET.LinearAlgebra namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// To learn more about solving general systems of
// simultaneous linear equations, see the
// LinearEquations QuickStart Sample.
//
// The methods and classes available for solving
// structured systems of equations are similar
// to those for general equations.

//
// Triangular systems and matrices
//

Console.WriteLine("Triangular matrices:");
// For the basics of working with triangular
// matrices, see the TriangularMatrices QuickStart
// Sample.
//
// Let's start with a triangular matrix. Remember
// that elements are stored in column-major order
// by default.
var t = Matrix.CreateUpperTriangular(
    4, 4, new double[]
    {
        1, 0, 0, 0,
        1, 2, 0, 0,
        1, 4, 1, 0,
        1, 3, 1, 2
    }, MatrixElementOrder.ColumnMajor);
var b1 = Vector.Create(new double[] { 1, 3, 6, 3 });
var b2 = Matrix.CreateFromArray(4, 2, new double[]
    {
        1, 3, 6, 3,
        2, 3, 5, 8
    }, MatrixElementOrder.ColumnMajor);
Console.WriteLine($"t = {t:F4}");

//
// The Solve method
//

// The following solves m x = b1. The second
// parameter specifies whether to overwrite the
// right-hand side with the result.
var x1 = t.Solve(b1, false);
Console.WriteLine($"x1 = {x1:F4}");
// If the overwrite parameter is omitted, the
// right-hand-side is overwritten with the solution:
t.Solve(b1);
Console.WriteLine($"b1 = {b1:F4}");
// You can solve for multiple right hand side
// vectors by passing them in a DenseMatrix:
var x2 = t.Solve(b2, false);
Console.WriteLine($"x2 = {x2:F4}");

//
// Related Methods
//

// You can verify whether a matrix is singular
// using the IsSingular method:
Console.WriteLine("IsSingular(t) = {0:F4}",
    t.IsSingular());
// The inverse matrix is returned by the Inverse
// method:
Console.WriteLine($"Inverse(t) = {t.GetInverse():F4}");
// The determinant is also available:
Console.WriteLine($"Det(t) = {t.GetDeterminant():F4}");
// The condition number is an estimate for the
// loss of precision in solving the equations
Console.WriteLine($"Cond(t) = {t.EstimateConditionNumber():F4}");
Console.WriteLine();

//
// Symmetric systems and matrices
//

Console.WriteLine("Symmetric matrices:");
// For the basics of working with symmetric
// matrices, see the SymmetricMatrices QuickStart
// Sample.
//
// Let's start with a symmetric matrix. Remember
// that elements are stored in column-major order
// by default.
var s = Matrix.CreateSymmetric(4, new double[]
    {
        1, 0, 0, 0,
        1, 2, 0, 0,
        1, 1, 2, 0,
        1, 0, 1, 4
    }, MatrixTriangle.Upper, MatrixElementOrder.ColumnMajor);
b1 = Vector.Create(new double[] { 1, 3, 6, 3 });
Console.WriteLine($"s = {s:F4}");

//
// The Solve method
//

// The following solves m x = b1. The second
// parameter specifies whether to overwrite the
// right-hand side with the result.
x1 = s.Solve(b1, false);
Console.WriteLine($"x1 = {x1:F4}");
// If the overwrite parameter is omitted, the
// right-hand-side is overwritten with the solution:
s.Solve(b1);
Console.WriteLine($"b1 = {b1:F4}");
// You can solve for multiple right hand side
// vectors by passing them in a DenseMatrix:
x2 = s.Solve(b2, false);
Console.WriteLine($"x2 = {x2:F4}");

//
// Related Methods
//

// You can verify whether a matrix is singular
// using the IsSingular method:
Console.WriteLine("IsSingular(s) = {0:F4}",
    s.IsSingular());
// The inverse matrix is returned by the Inverse
// method:
Console.WriteLine($"Inverse(s) = {s.GetInverse():F4}");
// The determinant is also available:
Console.WriteLine($"Det(s) = {s.GetDeterminant():F4}");
// The condition number is an estimate for the
// loss of precision in solving the equations
Console.WriteLine($"Cond(s) = {s.EstimateConditionNumber():F4}");
Console.WriteLine();

//
// The CholeskyDecomposition class
//

// If the symmetric matrix is positive definite,
// you can use the CholeskyDecomposition class
// to optimize performance if multiple operations
// need to be performed. This class does the
// bulk of the calculations only once. This
// decomposes the matrix into G x transpose(G)
// where G is a lower triangular matrix.
//
// If the matrix is indefinite, you need to use
// the LUDecomposition class instead. See the
// LinearEquations QuickStart Sample for details.
Console.WriteLine("Using Cholesky Decomposition:");
// The constructor takes an optional second argument
// indicating whether to overwrite the original
// matrix with its decomposition:
var cf = s.GetCholeskyDecomposition(false);
// The Factor method performs the actual
// factorization. It is called automatically
// if needed.
cf.Decompose();
// All methods mentioned earlier are still available:
x2 = cf.Solve(b2, false);
Console.WriteLine($"x2 = {x2:F4}");
Console.WriteLine("IsSingular(m) = {0:F4}",
    cf.IsSingular());
Console.WriteLine($"Inverse(m) = {cf.GetInverse():F4}");
Console.WriteLine($"Det(m) = {cf.GetDeterminant():F4}");
Console.WriteLine($"Cond(m) = {cf.EstimateConditionNumber():F4}");
// In addition, you have access to the
// triangular matrix, G, of the composition.
Console.WriteLine($"  G = {cf.LowerTriangularFactor:F4}");

// Note that if the matrix is indefinite,
// the factorization will fail and throw a
// MatrixNotPositiveDefiniteException.
s[0, 0] = -99;
cf = s.GetCholeskyDecomposition();
try {
    cf.Decompose();
} catch (MatrixNotPositiveDefiniteException e) {
    Console.WriteLine(e.Message);
}
// To avoid this, you can use the TryDecompose method,
// which returns true if the decomposition succeeds,
// and false otherwise:
if (cf.TryDecompose())
    Console.WriteLine("Decomposition succeeded!");
else
    Console.WriteLine("Decomposition failed!");

