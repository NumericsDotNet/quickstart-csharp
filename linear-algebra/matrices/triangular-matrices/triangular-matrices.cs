//=====================================================================
//
//  File: triangular-matrices.cs
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
// The TriangularMatrix class resides in the Numerics.NET.LinearAlgebra
// namespace.
using Numerics.NET.LinearAlgebra;

// Illustrates the use of the TriangularMatrix class in the
// Numerics.NET.LinearAlgebra namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// Triangular matrices are matrices whose elements
// above or below the diagonal are all zero. The
// former is called lower triangular, the latter
// lower triangular. In addition, triangular matrices
// can have all 1's on the diagonal.

//
// Constructing triangular matrices
//

// Constructing triangular matrices is similar to
// constructing general matrices. See the
// BasicMatrices QuickStart samples for a more
// complete discussion.
//
// All constructors take a MatrixTriangle
// value as their first parameter. This indicates
// whether an upper or lower triangular matrix
// should be created. The following creates a
// 5x5 lower triangular matrix:
var t1 = Matrix.CreateLowerTriangular<double>(5, 5);
// You can also specify whether the diagonal
// consists of all 1's using a unitDiagonal parameter:
var t2 = Matrix.CreateLowerTriangular<double>(
    5, 5, MatrixDiagonal.UnitDiagonal);
// Triangular matrices access and modify only the
// elements that are non-zero. If the diagonal
// mode is UnitDiagonal, the diagonal elements
// are not used, since they are all equal to 1.
double[] elements = new double[]
    {
        11, 12, 13, 14, 15,
        21, 22, 23, 24, 25,
        31, 32, 33, 34, 35,
        41, 42, 43, 44, 45,
        51, 52, 53, 54, 55
    };
// The following creates a matrix using the
// upper triangular part of the above.
var t3 = Matrix.CreateUpperTriangular(5, 5,
    elements, MatrixElementOrder.RowMajor);
Console.WriteLine($"t3 = {t3:F0}");
// Same as above, but unit diagonal:
var t4 = Matrix.CreateUpperTriangular(5, 5,
    elements, MatrixDiagonal.UnitDiagonal,
    MatrixElementOrder.RowMajor, true);
Console.WriteLine($"t4 = {t4:F0}");

//
// Extracting triangular matrices
//

// You may want to use part of a dense matrix
// as a triangular matrix. The static
// ExtractUpperTriangle and ExtractLowerTriangle
// methods perform this task.
var m = Matrix.CreateFromArray(5, 5, elements, MatrixElementOrder.ColumnMajor);
Console.WriteLine($"m = {m:F0}");
// Both methods are overloaded. The simplest
// returns a triangular matrix of the same dimension:
var t5 = Matrix.ExtractLowerTriangle(m);
Console.WriteLine($"t5 = {t5:F0}");
// You can also specify if the matrix is unit diagonal:
var t6 = Matrix.ExtractUpperTriangle(
        m, MatrixDiagonal.UnitDiagonal);
Console.WriteLine($"t6 = {t6:F0}");
// Or the dimensions of the matrix if they don't
// match the original:
var t7 = Matrix.ExtractUpperTriangle(
    m, 3, 3, MatrixDiagonal.UnitDiagonal);
Console.WriteLine($"t7 = {t7:F0}");
Console.WriteLine();

//
// TriangularMatrix properties
//

// The IsLowerTriangular and IsUpperTriangular return
// a boolean value:
Console.WriteLine("t4 is lower triangular? - {0:F0}",
    t4.IsLowerTriangular);
Console.WriteLine("t4 is upper triangular? - {0:F0}",
    t4.IsUpperTriangular);
// The IsUnitDiagonal property indicates whether the
// matrix has all 1's on its diagonal:
Console.WriteLine("t3 is unit diagonal? - {0:F0}",
    t3.IsUnitDiagonal);
Console.WriteLine("t4 is unit diagonal? - {0:F0}",
    t4.IsUnitDiagonal);
Console.WriteLine();
// You can get and set matrix elements:
t3[1, 3] = 55;
Console.WriteLine("t3[1, 3] = {0:F0}", t3[1, 3]);
// But trying to set an element that is zero or
// is on the diagonal for a unit diagonal matrix
// causes an exception to be thrown:
try
{
    t3[3, 1] = 100;
}
catch(ComponentReadOnlyException e)
{
    Console.WriteLine("Error accessing element: {0:F0}",
        e.Message);
}

//
// Rows and columns
//

// The GetRow and GetColumn methods are
// available.
var row = t3.GetRow(1);
Console.WriteLine($"row 2 of t3 = {row:F0}");
var column = t4.GetColumn(1, 0, 2);
Console.WriteLine($"2nd column of t4 from row 1 to 3 = {column:F0}");

