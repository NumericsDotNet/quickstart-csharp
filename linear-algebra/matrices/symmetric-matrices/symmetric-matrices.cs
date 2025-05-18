//=====================================================================
//
//  File: symmetric-matrices.cs
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
// The SymmetricMatrix class resides in the Numerics.NET.LinearAlgebra
// namespace.
using Numerics.NET.LinearAlgebra;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates the use of the SymmetricMatrix class in the
    /// Numerics.NET.LinearAlgebra namespace of Numerics.NET.
    /// </summary>
    class SymmetricMatrices
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // Symmetric matrices are matrices whose elements
            // are symmetrical around the main diagonal.
            // Symmetric matrices are always square, and are
            // equal to their own transpose.

            //
            // Constructing symmetric matrices
            //

            // Constructing symmetric matrices is similar to
            // constructing general matrices. See the
            // BasicMatrices QuickStart samples for a more
            // complete discussion.

            // Symmetric matrices are always square. You don't
            // have to specify both the number of rows and the
            // number of columns.
            //
            // The following creates a 5x5 symmetric matrix:
            var s1 = Matrix.CreateSymmetric<double>(5);
            // Symmetric matrices access and modify only the
            // elements on and either above or below the
            // main diagonal. When initializing a
            // symmetric matrix in a constructor, you must
            // specify a triangleMode parameter that specifies
            // whether to use the upper or lower triangle:
            double[] components = new double[]
                {
                    11, 12, 13, 14, 15,
                    21, 22, 23, 24, 25,
                    31, 32, 33, 34, 35,
                    41, 42, 43, 44, 45,
                    51, 52, 53, 54, 55
                };
            var s2 = Matrix.CreateSymmetric<double>(5, components,
                MatrixTriangle.Upper, MatrixElementOrder.ColumnMajor);
            Console.WriteLine($"s2 = {s2:F0}");

            // You can also create a symmetric matrix by
            // multiplying any matrix by its transpose:
            var m = Matrix.CreateFromArray(3, 4, new double[]
                {
                    1, 2, 3,
                    2, 3, 4,
                    3, 4, 5,
                    4, 5, 7
                }, MatrixElementOrder.ColumnMajor);
            Console.WriteLine($"m = {m:F0}");
            // This calculates transpose(m) times m:
            var s3 = SymmetricMatrix<double>.FromOuterProduct(m);
            Console.WriteLine($"s3 = {s3:F0}");
            // An optional 'side' parameter lets you specify
            // whether the left or right operand of the
            // multiplication is the transposed matrix.
            // This calculates m times transpose(m):
            var s4 = SymmetricMatrix<double>.FromOuterProduct(m,
                    MatrixOperationSide.Right);
            Console.WriteLine($"s4 = {s4:F0}");

            //
            // SymmetricMatrix methods
            //

            // The GetEigenvalues method returns a vector
            // containing the eigenvalues.
            var l = s4.GetEigenvalues();
            Console.WriteLine($"Eigenvalues: {l:F4}");

            // The ApplyMatrixFunction calculates a function
            // of the entire matrix. For example, to calculate
            // the 'sine' of a matrix:
            var sinS = s4.ApplyMatrixFunction(new Func<double, double>(Math.Sin));
            Console.WriteLine($"sin(s4): {sinS:F4}");

            // Symmetric matrices don't have any specific
            // properties.

            // You can get and set matrix elements:
            s3[1, 3] = 55;
            Console.WriteLine("s3[1, 3] = {0:F0}", s3[1, 3]);
            // And the change will automatically be reflected
            // in the symmetric element:
            Console.WriteLine("s3[3, 1] = {0:F0}", s3[3, 1]);

            //
            // Row and column views
            //

            // The GetRow and GetColumn methods are
            // available.
            var row = s2.GetRow(1);
            Console.WriteLine($"row 1 of s2 = {row:F0}");
            var column = s2.GetColumn(2, 3, 4);
            Console.WriteLine("column 3 of s2 from row 4 to ");
            Console.WriteLine($"  row 5 = {column:F0}");

        }
    }
}
