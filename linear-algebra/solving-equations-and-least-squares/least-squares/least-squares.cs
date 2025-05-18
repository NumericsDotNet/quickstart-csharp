//=====================================================================
//
//  File: least-squares.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// The DenseMatrix and LeastSquaresSolver classes reside in the
// Numerics.NET.LinearAlgebra namespace.
using Numerics.NET;
using Numerics.NET.LinearAlgebra;

// Illustrates solving least squares problems using the
// LeastSquaresSolver class in the Numerics.NET.LinearAlgebra
// namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// A least squares problem consists in finding
// the solution to an overdetermined system of
// simultaneous linear equations so that the
// sum of the squares of the error is minimal.
//
// A common application is fitting data to a
// curve. See the CurveFitting sample application
// for a complete example.

// Let's start with a general matrix. This will be
// the matrix a in the left hand side ax=b:
var a = Matrix.CreateFromArray(6, 4, new double[]
    {
        1, 1, 1, 1, 1, 1,
        1, 2, 3, 4, 5, 6,
        1, 4, 9, 16, 25, 36,
        1, 2, 1, 2, 1, 2
    }, MatrixElementOrder.ColumnMajor);
// Here is the right hand side:
var b = Vector.Create(new double[] { 1, 3, 6, 11, 15, 21 });
var b2 = Matrix.CreateFromArray(6, 2, new double[]
    {
        1, 3, 6, 11, 15, 21,
        1, 2, 3, 4, 5, 7
    }, MatrixElementOrder.ColumnMajor);
Console.WriteLine($"a = {a:F4}");
Console.WriteLine($"b = {b:F4}");

//
// The LeastSquaresSolver class
//

var x = a.LeastSquaresSolve(b);
var qr = a.GetQRDecomposition();
qr.LeastSquaresSolve(b);
// The following creates an instance of the
// LeastSquaresSolver class for our problem:
var solver = new LeastSquaresSolver<double>(a, b);
// We can specify the solution method: normal
// equations or QR decomposition. In most cases,
// a QR decomposition is the most desirable:
solver.SolutionMethod = LeastSquaresSolutionMethod.QRDecomposition;
// The Solve method calculates the solution:
x = solver.Solve();
Console.WriteLine($"x = {x:F4}");
// The Solution property also returns the solution:
Console.WriteLine($"x = {solver.Solution:F4}");
// More detailed information is available from
// additional methods.
// The values of the right hand side predicted
// by the solution:
Console.WriteLine($"Predictions = {solver.GetPredictions():F4}");
// The residuals (errors) of the solution:
Console.WriteLine($"Residuals = {solver.GetResiduals():F4}");
// The total sum of squares of the residues:
Console.WriteLine("Residual square error = {0}",
    solver.GetResidualSumOfSquares());

//
// Direct normal equations
//

// Alternatively, you can create a least squares
// solution by providing the normal equations
// directly. This may be useful when it is easy
// to calculate the normal equations directly.
//
// Here, we'll just calculate the normal equation:
var aTa = SymmetricMatrix<double>.FromOuterProduct(a);
var aTb = b * a; // a.Transpose() * b;
// We find the solution by solving the normal equations
// directly:
x = aTa.Solve(aTb);
Console.WriteLine($"x = {x:F4}");
// However, properties of the least squares solution, such as
// error estimates and residuals are not available.

