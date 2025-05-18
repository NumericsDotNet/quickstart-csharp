//=====================================================================
//
//  File: basic-vectors.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// The Vector class resides in the Numerics.NET.LinearAlgebra
// namespace.
using Numerics.NET;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates the use of the Vector class in the
    /// Numerics.NET.LinearAlgebra namespace of Numerics.NET.
    /// </summary>
    class BasicVectors
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            //
            // Constructing vectors
            //

            // Option #1: specify the number of elements.
            // All elements are set to zero.
            // In this case, you must specify the element type
            // as a generic type argument:
            var v1 = Vector.Create<double>(5);

            // Option #2: specify the elements.
            // The element type can be inferred from the arguments.
            var v2 = Vector.Create(1.0, 2.0, 3.0, 4.0, 5.0);
            // Option #3: specify the elements as a double array.
            // By default, the elements are copied to a storage
            // area internal to the Vector.
            double[] elements = new double[] {1, 2, 3, 4, 5};
            var v3 = Vector.Create(elements);
            // Option #4: same as above, and specify mutability.
            var v4 = Vector.Create(elements, ArrayMutability.Immutable);
            // Changing a value in the original vector changes
            // the resulting vector.
            Console.WriteLine($"v4 = {v4:F4}");
            elements[3] = 1;
            Console.WriteLine($"v4 = {v4:F4}");
            // Option #5: same as #4, but specify if the values array
            // should be reused for storage.
            var v5 = Vector.CreateFromArray(4, elements, true, ArrayMutability.MutableValues);

            //
            // Vector properties
            //

            // The Length property gives the number of elements
            // of a Vector:
            Console.WriteLine($"v1.Length = {v1.Length}");
            // The ToArray method returns a double array
            // that contains the elements of the vector.
            // This is always a copy:
            elements = v2.ToArray();
            Console.WriteLine("Effect of shared storage:");
            Console.WriteLine($"v2[2] = {v2[2]}");
            elements[2] = 1;
            Console.WriteLine($"v2[2] = {v2[2]}");

            //
            // Accessing vector elements
            //

            // The Vector class defines an indexer property that
            // takes a zero-based index.
            Console.WriteLine("Assigning with private storage:");
            Console.WriteLine($"v1[2] = {v1[2]}");
            // You can assign to this property:
            v1[2] = 7;
            Console.WriteLine($"v1[2] = {v1[2]}");
            // The vectors v4 and v5 had the copy parameter in the
            // constructor set to false. As a result, they share
            // their element storage. Changing one vector also
            // changes the other:
            Console.WriteLine("Assigning with shared storage:");
            Console.WriteLine($"v5[1] = {v5[1]}");
            v5[1] = 7;
            Console.WriteLine($"v5[1] = {v5[1]}");

            // The SetValue method sets all elements of a vector
            // to the same value:
            v1.SetValue(1);
            Console.WriteLine($"v1 = {v1:F4}");
            // The SetToZero method sets all elements to 0:
            v1.SetToZero();
            Console.WriteLine($"v1 = {v1:F4}");

            //
            // Copying and cloning vectors
            //

            // A shallow copy of a vector constructs a vector
            // that shares the element storage with the original.
            // This is done using the ShallowCopy method:
            Console.WriteLine("Shallow copy vs. clone:");
            var v7 = v2.ShallowCopy();
            // The clone method creates a full copy.
            var v8 = v2.Clone();
            // When we change v2, v7 changes, but v8 is left
            // unchanged.
            Console.WriteLine($"v2[1] = {v2[1]}");
            v2[1] = -2;
            Console.WriteLine($"v7[1] = {v7[1]}");
            Console.WriteLine($"v8[1] = {v8[1]}");
            // We can give a vector its own element storage
            // by calling the CloneData method:
            Console.WriteLine("CloneData:");
            v7.CloneData();
            // Now, changing the original v2 no longer changes v7:
            v2[1] = 4;
            Console.WriteLine($"v7[1] = {v7[1]}");
            // The CopyTo method copies the elements of a Vector
            // to a variety of destinations. It may be a Vector:
            Console.WriteLine("CopyTo:");
            v5.CopyTo(v1);
            Console.WriteLine($"v5 = {v5:F4}");
            Console.WriteLine($"v1 = {v1:F4}");
            // You can specify an index where to start copying
            // in the destination vector:
            v5.CopyTo(v1, 1);
            Console.WriteLine($"v1 = {v1:F4}");
            // Or you can copy to a double array:
            v5.CopyTo(elements);

            //
            // Mutability options
            //

            // When creating a vector, you can specify whether
            // the values and non-zero pattern of the vector
            // may be modified.

        }
    }
}
