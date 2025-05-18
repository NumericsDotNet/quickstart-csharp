//=====================================================================
//
//  File: sparse-matrices.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// The sparse vector and matrix classes reside in the
// Numerics.NET.LinearAlgebra namespace.
using Numerics.NET;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates using sparse vectors and matrices using the classes
    /// in the Numerics.NET.LinearAlgebra.Sparse namespace
    /// of Numerics.NET.
    /// </summary>
    class SparseMatrices
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");
            //
            // Sparse vectors
            //

            // The SparseVector class has three constructors. The
            // first simply takes the length of the vector. All elements
            // are initially zero.
            var v1 = Vector.CreateSparse<double>(1000000);

            // The second constructor lets you specify how many elements
            // are expected to be nonzero. This 'fill factor' is a number
            // between 0 and 1.
            var v2 = Vector.CreateSparse<double>(1000000, 1e-4);

            // The second constructor lets you specify how many elements
            // are expected to be nonzero. This 'fill factor' is a number
            // between 0 and 1.
            var v3 = Vector.CreateSparse<double>(1000000, 100);

            // The fourth constructor lets you specify the indexes of the nonzero
            // elements and their values as arrays:
            int[] indexes = { 1, 10, 100, 1000, 10000 };
            double[] values = { 1.0, 10.0, 100.0, 1000.0, 10000.0 };
            var v4 = Vector.CreateSparse(1000000, indexes, values);

            // Elements can be accessed individually:
            v1[1000] = 2.0;
            Console.WriteLine($"v1[1000] = {v1[1000]}");

            // The NonzeroCount returns how many elements are non zero:
            Console.WriteLine($"v1 has {v1.NonzeroCount} nonzeros");
            Console.WriteLine($"v4 has {v4.NonzeroCount} nonzeros");

            // The NonzeroElements property returns a collection of
            // IndexValuePair structures that you can use to iterate
            // over the elements of the vector:
            Console.WriteLine("Nonzero elements of v4:");
            foreach(var pair in v4.NonzeroElements)
                Console.WriteLine("Element {0} = {1}", pair.Index, pair.Value);

            // All other vector methods and properties are also available,
            // Their implementations take advantage of sparsity.
            Console.WriteLine($"Norm(v4) = {v4.Norm()}");
            Console.WriteLine($"Sum(v4) = {v4.Sum()}");

            // Note that some operations convert a sparse vector to a
            // DenseVector, causing memory to be allocated for all
            // elements.

            //
            // Sparse Matrices
            //

            // All sparse matrix classes inherit from SparseMatrix. This is an abstract class.
            // There currently is only one implementation class:
            // SparseCompressedColumnMatrix.

            // Sparse matrices are created by calling the CreateSparse factory
            // method on the Matrix class. It has 4 overloads:

            // The first overload takes the number of rows and columns as arguments:
            var m1 = Matrix.CreateSparse<double>(100000, 100000);

            // The second overload adds a fill factor:
            var m2 = Matrix.CreateSparse<double>(100000, 100000, 1e-5);

            // The third overload uses the actual number of nonzero elements rather than
            // the fraction:
            var m3 = Matrix.CreateSparse<double>(10000, 10000, 20000);

            // The fourth overload lets you specify the locations and values of the
            // nonzero elements:
            int[] rows = { 1, 11, 111, 1111 };
            int[] columns = { 2, 22, 222, 2222 };
            double[] matrixValues = { 3.0, 33.0, 333.0, 3333.0 };
            var m4 = Matrix.CreateSparse(10000, 10000, rows, columns, matrixValues);

            // You can access elements as before...
            Console.WriteLine("m4[111, 222] = {0}", m4[111, 222]);
            m4[99, 22] = 99.0;

            // A series of Insert methods lets you build a sparse matrix from scratch:
            // A single value:
            m1.InsertEntry(25.0, 200, 500);
            // Multiple values:
            m1.InsertEntries(matrixValues, rows, columns);
            // Multiple values all in the same column:
            m1.InsertColumn(33, values, indexes);
            // Multiple values all in the same row:
            m1.InsertRow(55, values, indexes);

            // A clique is a 2-dimensional submatrix with indexed rows and columns.
            var clique = Matrix.CreateFromArray(2, 2, new double[] {11, 12, 21, 22},
                MatrixElementOrder.ColumnMajor);
            int[] cliqueIndexes = new int[] { 5, 8 };
            m1.InsertClique(clique, cliqueIndexes, cliqueIndexes);

            // You can use the NonzeroElements collection to iterate
            // over the nonzero elements of the matrix. The items
            // are of type RowColumnValueTriplet:
            Console.WriteLine("Nonzero elements of m1:");
            foreach(var triplet in m1.NonzeroElements)
                Console.WriteLine("m1[{0},{1}] = {2}", triplet.Row, triplet.Column, triplet.Value);

            // ... including rows and columns.
            var column = m4.GetColumn(22);
            Console.WriteLine("Nonzero elements in column 22 of m4:");
            foreach (var pair in column.NonzeroElements)
                Console.WriteLine("Element {0} = {1}", pair.Index, pair.Value);

            // Many matrix methods have been optimized to take advantage of sparsity:
            Console.WriteLine($"F-norm(m1) = {m1.FrobeniusNorm()}");

            // But beware: some revert to a dense algorithm and will fail on huge matrices:
            try
            {
                var inverse = m1.GetInverse();
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
