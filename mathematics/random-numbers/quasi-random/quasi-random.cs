//=====================================================================
//
//  File: quasi-random.cs
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
using Numerics.NET.Random;

namespace Numerics.NET.Quickstart.CSharp
{
    /// <summary>
    /// Illustrates the use of quasi-random sequences by computing
    /// a multi-dimensional integral.
    /// </summary>
    public class QuasiRandomSequences
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // This QuickStart Sample demonstrates the use of
            // quasi-random sequences by computing
            // a multi-dimensional integral.

            // We will use one million points.
            int length = 1000000;
            // The number of dimensions:
            int dimension = 5;

            // We will evaluate the function
            //
            //    Product(i = 1 -> # dimensions) |4 x[i] - 2|
            //
            // over the hypercube 0 <= x[i] <= 1. The value of this integral
            // is exactly 1.

            // Create the sequence:
            var sequence = QuasiRandom.HaltonSequence(dimension, length);

            Console.WriteLine("# iter.  Estimate");
            // Compute the integral by summing over all points:
            double sum = 0.0;

            int i = 0;
            foreach (var point in sequence)
            {
                if (i % 100000 == 0)
                    Console.WriteLine("{0,6}  {1,8:F4}", i, sum / i);

                // Evaluate the integrand:
                double functionValue = 1.0;
                for(int j = 0; j < dimension; j++)
                    functionValue *= Math.Abs(4.0*point[j]-2.0);
                sum += functionValue;
                i++;
            }
            Console.WriteLine($"Final estimate: {sum / length,8:F4}");
            Console.WriteLine("Exact value: 1.0000");

            // Sobol sequences require more data and more initialization.
            // Fortunately, different sequences of the same dimension
            // can share much of the work and storage. The
            // SobolSequenceGenerator class should be used in this case:

            int skip = 1000;
            var sobol = new SobolSequenceGenerator(dimension, length + skip);
            // Sobol sequences are more flexible: they let you skip
            // a number of points at the start of the sequence.
            // The cost of skipping points is O(1).
            i = 0;
            sum = 0.0;
            foreach (var point in sobol.Generate(length, skip))
            {
                if (i % 100000 == 0)
                    Console.WriteLine("{0,6}  {1,8:F4}", i, sum / i);

                // Evaluate the integrand:
                double functionValue = 1.0;
                for (int j = 0; j < dimension; j++)
                    functionValue *= Math.Abs(4.0 * point[j] - 2.0);
                sum += functionValue;
                i++;
            }
            // Print the final result.
            Console.WriteLine($"Final estimate: {sum / length,8:F4}");
            Console.WriteLine("Exact value: 1.0000");

        }
    }
}
