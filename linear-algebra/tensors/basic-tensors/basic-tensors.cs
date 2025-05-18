//=====================================================================
//
//  File: basic-tensors.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// Tensor classes reside in the Numerics.NET.Tensors
// namespace.
using Numerics.NET.Tensors;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates the use of the Tensor class in the
    /// Numerics.NET namespace of Numerics.NET.
    /// </summary>
    class BasicTensors
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            //
            // Constructing tensors
            //

            // The simplest tensor is a scalar. It has no dimensions.
            // It is created using the CreateScalar method:
            var s = Tensor.CreateScalar(5.0);
            Console.WriteLine($"s.IsScalar = {s.IsScalar}");

            // To create tensors with 1 or more dimensions, there are many options,
            // including:

            // Option #1: Create an empty tensor.
            // The element type must be specified as a generic
            // type parameter. The dimensions are specified
            // as itnegers. The following constructs a tensor
            // with 3 rows and 5 columns:
            var t1 = Tensor.Create<double>(3, 5);
            Console.WriteLine($"t1 = {t1}");

            // Option #2: specify an array of rank up to 4.
            // By default, elements are taken in C# order.
            // Therefore, the following creates a tensor
            // with 4 rows and 3 columns:
            double[,] elements2D =
                {
                    {1, 2, 3},
                    {2, 3, 4},
                    {3, 4, 5},
                    {4, 5, 6}
                };
            var t2 = Tensor.CreateFromArray(elements2D);
            Console.WriteLine($"t2 = {t2}");

            // Option #3: Cast an array to a tensor.
            var t3 = (Tensor<double>)elements2D;
            Console.WriteLine($"t3 = {t3}");

            // Option #4: Specify an array of elements, and
            // a tuple with the dimensions. The elements are listed
            // in C# style order (last index changes fastest).
            // The following tensor is identical to t2:
            double[] elements = new double[]
            {
                1, 2, 3,
                2, 3, 4,
                3, 4, 5,
                4, 5, 6
            };
            var t4 = Tensor.CreateFromArray(elements, (4, 3));
            Console.WriteLine($"t4 = {t4}");

            // Option #5: same as above, but use a memory block
            // to store the elements. This can be both managed
            // or unmanaged memory. The following tensor is
            // identical to t3:
            var t5 = Tensor.CreateFromMemory(elements.AsMemory(), (4, 3), createView: true);
            Console.WriteLine($"t5 = {t5}");

            // Option #6: Create a tensor from a function.
            // Specify the dimensions and a function that
            // returns the value of each element:
            var t6 = Tensor.CreateFromFunction((10, 10), (i, j) => 1.0 / (1.0 + i + j));
            Console.WriteLine($"t6 = {t6:F5}");

            // Option #7-9: Create a range of values:
            var t7 = Tensor.CreateRange(10);
            // t7 -> [ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 ]
            var t8 = Tensor.CreateGeometricRange(1.0, 1000.0, 4);
            // t8 -> [ 1, 10, 100, 1000 ]
            var t9 = Tensor.CreateLogarithmicRange(0.0, 3.0, 4);
            // t9 -> [ 1, 10, 100, 1000 ]

            // Option #10-11: Create a tensor with random elements:
            var t10 = Tensor.CreateRandom((10, 10));
            var t11 = Tensor.CreateRandomNormal((5, 7, 18), new System.Random(17));
            //
            // Tensor properties
            //

            // The Rank property gives the rank or
            // number of dimensions of the tensor:
            Console.WriteLine($"t1.Rank = {t1.Rank}");

            // The Shape property gives the number of elements
            // along each dimension:
            Console.WriteLine($"t1.Shape = {t1.Shape}");
            Console.WriteLine($"t1 has {t1.Shape[0]} rows and {t1.Shape[1]} columns.");

            // The FlattenedLength property gives the total number of
            // elements in the tensor:
            Console.WriteLine($"t1.Length = {t1.FlattenedLength}");

            var t1Layout = t1.Layout;

            // The Layout gives the strides between elements
            // along each axis in the tensor:
            Console.WriteLine($"t1 strides along axis 0: {t1Layout[0]}");
            Console.WriteLine($"t1 strides along axis 1: {t1Layout[1]}");

        }
    }
}
