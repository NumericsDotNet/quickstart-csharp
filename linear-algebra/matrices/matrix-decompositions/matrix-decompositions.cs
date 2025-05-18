//=====================================================================
//
//  File: matrix-decompositions.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

using Numerics.NET;

// Illustrates the use of matrix decompositions for solving systems of
// simultaneous linear equations and related operations using the
// Decomposition class and its derived classes from the
// Numerics.NET.LinearAlgebra namespace of Numerics.NET.

var success = Numerics.NET.License.Verify("your-trial-key-here");

Console.WriteLine(success);

// For details on the basic workings of var
// objects, including constructing, copying and
// cloning vectors, see the BasicVectors QuickStart
// Sample.
//
// For details on the basic workings of Matrix
// objects, including constructing, copying and
// cloning vectors, see the BasicVectors QuickStart
// Sample.
//

//
// LU Decomposition
//

// The LU decomposition of a matrix rewrites a matrix A in the
// form A = PLU with P a permutation matrix, L a unit-
// lower triangular matrix, and U an upper triangular matrix.

var aLU = Matrix.CreateFromArray(4, 4,
    new double[]
{
    1.80, 5.25, 1.58, -1.11,
    2.88,-2.95, -2.69, -0.66,
    2.05,-0.95, -2.90, -0.59,
    -0.89,-3.80,-1.04, 0.80
}, MatrixElementOrder.ColumnMajor);

var bLU = Matrix.CreateFromArray(4, 2, new double[]
{
    9.52,24.35,0.77,-6.22,
    18.47,2.25,-13.28,-6.21
}, MatrixElementOrder.ColumnMajor);

// The decomposition is obtained by calling the GetLUDecomposition
// method of the matrix. It takes zero or one parameters. The
// parameter is a bool value that indicates whether the
// matrix may be overwritten with its decomposition.
var lu = aLU.GetLUDecomposition(true);
Console.WriteLine($"A = {aLU:F2}");

// The Decompose method performs the decomposition. You don't need
// to call it explicitly, as it is called automatically as needed.

// The IsSingular method checks for singularity.
Console.WriteLine($"'A is singular' is {lu.IsSingular():F6}.");
// The LowerTriangularFactor and UpperTriangularFactor properties
// return the two main components of the decomposition.
Console.WriteLine($"L = {lu.LowerTriangularFactor:F6}");
Console.WriteLine($"U = {lu.UpperTriangularFactor:F6}");

// GetInverse() gives the matrix inverse, Determinant() the determinant:
Console.WriteLine($"Inv A = {lu.GetInverse():F6}");
Console.WriteLine($"Det A = {lu.GetDeterminant():F6}");

// The Solve method solves a system of simultaneous linear equations, with
// one or more right-hand-sides:
var xLU = lu.Solve(bLU);
Console.WriteLine($"x = {xLU:F6}");

// The permutation is available through the RowPermutation property:
Console.WriteLine($"P = { lu.RowPermutation}");
Console.WriteLine($"Px = {xLU.PermuteRowsInPlace(lu.RowPermutation):F6}");

//
// QR Decomposition
//

// The QR decomposition of a matrix A rewrites the matrix
// in the form A = QR, with Q a square, orthogonal matrix,
// and R an upper triangular matrix.

var aQR = Matrix.CreateFromArray(5, 3,
    new double[]
    {
        2.0, 2.0, 1.6, 2.0, 1.2,
        2.5, 2.5,-0.4,-0.5,-0.3,
        2.5, 2.5, 2.8, 0.5,-2.9
    }, MatrixElementOrder.ColumnMajor);
var bQR = Vector.Create(1.1, 0.9, 0.6, 0.0,-0.8);

// The decomposition is obtained by calling the GetQRDecomposition
// method of the matrix. It takes zero or one parameters. The
// parameter is a bool value that indicates whether the
// matrix may be overwritten with its decomposition.
var qr = aQR.GetQRDecomposition(true);
Console.WriteLine($"A = {aQR:F1}");

// The Decompose method performs the decomposition. You don't need
// to call it explicitly, as it is called automatically as needed.

// The IsSingular method checks for singularity.
Console.WriteLine($"'A is singular' is {qr.IsSingular():F6}.");

// GetInverse() gives the matrix inverse, Determinant() the determinant,
// but these are defined only for square matrices.

// The Solve method solves a system of simultaneous linear equations, with
// one or more right-hand-sides. If the matrix is over-determined, you can
// use the LeastSquaresSolve method to find a least squares solution:
var xQR = qr.LeastSquaresSolve(bQR);
Console.WriteLine($"x = {xQR:F6}");

// The OrthogonalFactor and UpperTriangularFactor properties
// return the two main components of the decomposition.
Console.WriteLine($"Q = {qr.OrthogonalFactor.ToDenseMatrix():F6}");
Console.WriteLine($"R = {qr.UpperTriangularFactor:F6}");

// You don't usually need to form Q explicitly. You can multiply
// a vector or matrix on either side by Q using the Multiply method:
var Q = qr.OrthogonalFactor;
Console.WriteLine($"Qb = {qr.OrthogonalFactor * bQR:F6}");
Console.WriteLine("transpose(Q)b = {0:F6}",
    qr.OrthogonalFactor.Transpose() * bQR);

//
// Singular Value Decomposition
//

// The singular value decomposition of a matrix A rewrites the matrix
// in the form A = USVt, with U and V orthogonal matrices,
// S a diagonal matrix. The diagonal elements of S are called
// the singular values.

var aSvd = Matrix.CreateFromArray(3, 5,
    new double[]
    {
        2.0, 2.0, 1.6, 2.0, 1.2,
        2.5, 2.5,-0.4,-0.5,-0.3,
        2.5, 2.5, 2.8, 0.5,-2.9
    }, MatrixElementOrder.RowMajor);
var bSvd = Vector.Create(1.1, 0.9, 0.6);

// The decomposition is obtained by calling the GetSingularValueDecomposition
// method of the matrix. It takes zero or one parameters. The
// parameter indicates which parts of the decomposition
// are to be calculated. The default is All.
var svd = aSvd.GetSingularValueDecomposition();
Console.WriteLine($"A = {aSvd:F1}");

// The Decompose method performs the decomposition. You don't need
// to call it explicitly, as it is called automatically as needed.

// The IsSingular method checks for singularity.
Console.WriteLine($"'A is singular' is {svd.IsSingular():F6}.");

// Several methods are specific to this class. The GetPseudoInverse
// method returns the Moore-Penrose pseudo-inverse, a generalization
// of the inverse of a matrix to rectangular and/or singular matrices:
var aInv = svd.GetPseudoInverse();

// It can be used to solve over- or under-determined systems.
var xSvd = aInv * bSvd;
Console.WriteLine($"x = {xSvd:F6}");

// The SingularValues property returns a vector that contains
// the singular values in descending order:
Console.WriteLine($"S = {svd.SingularValues:F6}");

// The LeftSingularVectors and RightSingularVectors properties
// return matrices that contain the U and V factors
// of the decomposition.
Console.WriteLine($"U = {svd.LeftSingularVectors:F6}");
Console.WriteLine($"V = {svd.RightSingularVectors:F6}");

//
// Cholesky decomposition
//

// The Cholesky decomposition of a symmetric matrix A
// rewrites the matrix in the form A = GGt with
// G a lower-triangular matrix.

// Remember the column-major storage mode: each line of
// components contains one COLUMN of the matrix.
var aC = Matrix.CreateSymmetric(4,
    new double[]
{
    4.16,-3.12, 0.56,-0.10,
    0, 5.03,-0.83, 1.18,
    0,0, 0.76, 0.34,
    0,0,0, 1.18
}, MatrixTriangle.Lower, MatrixElementOrder.ColumnMajor);
var bC = Matrix.CreateFromArray(4, 2,
    new double[] {8.70,-13.35,1.89,-4.14,8.30,2.13,1.61,5.00},
    MatrixElementOrder.ColumnMajor);

// The decomposition is obtained by calling the GetCholeskyDecomposition
// method of the matrix. It takes zero or one parameters. The
// parameter is a bool value that indicates whether the
// matrix should be overwritten with its decomposition.
var c = aC.GetCholeskyDecomposition(true);
Console.WriteLine($"A = {aC:F2}");

// The Decompose method performs the decomposition. You don't need
// to call it explicitly, as it is called automatically as needed.

// The IsSingular method checks for singularity.
Console.WriteLine($"'A is singular' is {c.IsSingular():F6}.");
// The LowerTriangularFactor returns the component of the decomposition.
Console.WriteLine($"L = {c.LowerTriangularFactor:F6}");

// GetInverse() gives the matrix inverse, Determinant() the determinant:
Console.WriteLine($"Inv A = {c.GetInverse():F6}");
Console.WriteLine($"Det A = {c.GetDeterminant():F6}");

// The Solve method solves a system of simultaneous linear equations, with
// one or more right-hand-sides:
var xC = c.Solve(bC);
Console.WriteLine($"x = {xC:F6}");

//
// Symmetric eigenvalue decomposition
//

// The eigenvalue decomposition of a symmetric matrix A
// rewrites the matrix in the form A = XLXt with
// X an orthogonal matrix and L a diagonal matrix.
// The diagonal elements of L are the eigenvalues.
// The columns of X are the eigenvectors.

// Remember the column-major storage mode: each line of
// components contains one COLUMN of the matrix.
var aEig = Matrix.CreateSymmetric(4,
    new double[]
{
      0.5,  0.0,  2.3, -2.6,
      0.0,  0.5, -1.4, -0.7,
      2.3, -1.4,  0.5,  0.0,
     -2.6, -0.7,  0.0,  0.5
}, MatrixTriangle.Lower, MatrixElementOrder.ColumnMajor);

// The decomposition is obtained by calling the GetLUDecomposition
// method of the matrix. It takes zero or one parameters. The
// parameter is a bool value that indicates whether the
// matrix should be overwritten with its decomposition.
var eig = aEig.GetEigenvalueDecomposition();
Console.WriteLine($"A = {aEig:F2}");

// The Decompose method performs the decomposition. You don't need
// to call it explicitly, as it is called automatically as needed.

// The IsSingular method checks for singularity.
Console.WriteLine($"'A is singular' is {eig.IsSingular():F6}.");
// The eigenvalues and eigenvectors of a symmetric matrix are all real.
// The RealEigenvalues property returns a vector containing the eigenvalues:
Console.WriteLine($"L = {eig.Eigenvalues:F6}");
// The RealEigenvectors property returns a matrix whose columns
// contain the corresponding eigenvectors:
Console.WriteLine($"X = {eig.Eigenvectors:F6}");

// GetInverse() gives the matrix inverse, Determinant() the determinant:
Console.WriteLine($"Inv A = {eig.GetInverse():F6}");
Console.WriteLine($"Det A = {eig.GetDeterminant():F6}");

