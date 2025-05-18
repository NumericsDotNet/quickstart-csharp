//=====================================================================
//
//  File: chebyshev-expansions.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// The ChebyshevSeries class resides in the Numerics.NET.Curves
// namespace.
using Numerics.NET.Curves;
// The Func<double, double> delegate resides in the
// Numerics.NET namespace.
using Numerics.NET;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates the use of the ChebyshevSeries class
    /// in the Numerics.NET.Curve namespace of Numerics.NET.
    /// </summary>
    class ChebyshevExpansions
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // Chebyshev polynomials form an alternative basis
            // for polynomials. A Chebyshev expansion is a
            // polynomial expressed as a sum of Chebyshev
            // polynomials.
            //
            // Using the ChebyshevSeries class instead of
            // Polynomial can have two major advantages:
            //   1. They are numerically more stable. Higher
            //      accuracy is maintained even for large problems.
            //   2. When approximating other functions with
            //      polynomials, the coefficients in the
            //      Chebyshev expansion will tend to decrease
            //      in size, where those of the normal polynomial
            //      approximation will tend to oscillate wildly.

            //
            // Constructing Chebyshev expansions
            //

            // Chebyshev expansions are defined over an interval.
            // The first constructor requires you to specify the
            // boundaries of the interval, and the coefficients
            // of the expansion.
            double[] coefficients = new double[] {1, 0.5, -0.3, 0.1};
            ChebyshevSeries chebyshev1 = new ChebyshevSeries(coefficients, 0, 2);
            // If you omit the boundaries, they are assumed to be
            // -1 and +1:
            ChebyshevSeries chebyshev2 = new ChebyshevSeries(coefficients);

            //
            // Chebyshev approximations
            //

            // A third constructor creates a Chebyshev
            // approximation to an arbitrary function. For more
            // about the Func<double, double> delegate, see the
            // FunctionDelegates QuickStart Sample.
            //
            // Chebyshev expansions allow us to obtain an
            // excellent approximation at minimal cost.
            //
            // The following creates a Chebyshev approximation
            // of degree 7 to Cos(x) over the interval [0, 2]:
            Func<double, double> cos = Math.Cos;
            ChebyshevSeries approximation1 = ChebyshevSeries.GetInterpolatingPolynomial(cos, 0, 2, 7);

            // The coefficients of the expansion are available through
            // the indexer property of the ChebyshevSeries object:
            Console.WriteLine("Chebyshev approximation of cos(x):");
            for(int index = 0; index <= 7; index++)
                Console.WriteLine("  c{0} = {1}", index,
                    approximation1[index]);

            // The largest errors are approximately at the
            // zeroes of the Chebyshev polynomial of degree 8:
            for(int index = 0; index <= 8; index++)
            {
                double zero = 1 + Math.Cos(index * Constants.Pi / 8);
                double error = approximation1.ValueAt(zero)
                    - Math.Cos(zero);
                Console.WriteLine(" Error {0} = {1}", index, error);
            }

            //
            // Least squares approximations
            //

            // We will now calculate the least squares polynomial
            // of degree 7 through 33 points.
            // First, calculate the points:
            double[] xValues = new double[33];
            double[] yValues = new double[33];
            for(int index = 0; index <= 32; index++)
            {
                double angle = index * Constants.Pi / 32;
                xValues[index] = 1 + Math.Cos(angle);
                yValues[index] = Math.Cos(xValues[index]);
            }
            // Next, define a ChebyshevBasis object for the
            // approximation we want: interval [0,2] and degree
            // is 7.
            ChebyshevBasis basis = new ChebyshevBasis(0, 2, 7);
            // Now we can calculate the least squares fit:
            ChebyshevSeries approximation2 = (ChebyshevSeries)
                basis.LeastSquaresFit(xValues, yValues, xValues.Length);
            // We can see it is close to the original
            // approximation we found earlier:
            for(int index = 0; index <= 7; index++)
                Console.WriteLine("  c{0} = {1}", index,
                    approximation2[index]);

        }
    }
}
