//=====================================================================
//
//  File: cubic-splines.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// The CubicSpline class resides in the
// Numerics.NET.Curves namespace.
using Numerics.NET.Curves;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates creating natural and clamped cubic splines using
    /// the CubicSpline class in the Numerics.NET.Curves
    /// namespace.
    /// </summary>
    class CubicSplines
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // A cubic spline is a piecewise curve that is made up
            // of pieces of cubic polynomials. Its value as well as its first
            // derivative are continuous, giving it a smooth appearance.
            //
            // Cubic splines are implemented by the CubicSpline class,
            // which inherits from PiecewiseCurve.
            //
            // For an example of piecewise constant and piecewise
            // linear curves, see the PiecewiseCurves QuickStart
            // Sample.
            //

            //
            // Creating Cubic Splines
            //

            // In order to define a spline curve completely, two extra
            // conditions must be imposed.

            // 'Natural' splines have zero second derivatives at the
            // end points. This is the default.

            // The data points are specified as double arrays containing
            // the x and y values:
            double[] xValues = {1, 2, 3, 4, 5, 6};
            double[] yValues = {1, 3, 4, 3, 4, 2};
            CubicSpline naturalSpline = new CubicSpline(xValues, yValues);

            // There is also a static method that constructs natural splines:
            naturalSpline = CubicSpline.CreateNatural(xValues, yValues);

            // 'Clamped' splines have a fixed slope or first derivative at the
            // leftmost and rightmost points. The slopes are specified as
            // two extra parameters in the constructor:
            CubicSpline clampedSpline = new CubicSpline(xValues, yValues, -1, 1);
            // Here is the factory method:
            clampedSpline = CubicSpline.CreateClamped(xValues, yValues, -1, 1);

            // 'Akima' splines minimize the oscillations in the interpolating
            // curve. The constructor takes an extra argument of type
            // CubicSplineKind:
            CubicSpline akimaSpline = new CubicSpline(xValues, yValues, CubicSplineKind.Akima);
            // The factory method does not require the extra parameter:
            akimaSpline = CubicSpline.CreateAkima(xValues, yValues);

            // Hermite splines have fixed values for the first derivative at each
            // data point. The first derivatives must be supplied as an array
            // or list:
            double[] yPrimeValues = { 0, 1, -1, 1, 0, -1 };
            CubicSpline hermiteSpline = new CubicSpline(xValues, yValues, yPrimeValues);
            // Likewise for the factory method:
            hermiteSpline = CubicSpline.CreateHermiteInterpolant(xValues, yValues, yPrimeValues);

            //
            // Curve Parameters
            //

            // The shape of any curve is determined by a set of parameters.
            // These parameters can be retrieved and set through the
            // Parameters collection. The number of parameters for a curve
            // is given by this collection's Count property.
            //
            // Cubic splines have 2n+2 parameters, where n is the number of
            // data points. The first n parameters are the x-values. The next
            // n parameters are the y-values. The last two parameters are
            // the values of the derivative at the first and last point. For natural
            // splines, these parameters are unused.

            Console.WriteLine("naturalSpline.Parameters.Count = {0}",
                naturalSpline.Parameters.Count);
            // Parameters can easily be retrieved:
            Console.WriteLine("naturalSpline.Parameters[0] = {0}",
                naturalSpline.Parameters[0]);
            // Parameters can also be set:
            naturalSpline.Parameters[0] = 1;

            //
            // Piecewise curve methods and properties
            //

            // The NumberOfIntervals property returns the number of subintervals
            // on which the curve has unique definitions.
            Console.WriteLine("Number of intervals: {0}",
                naturalSpline.NumberOfIntervals);

            // The IndexOf method returns the index of the interval
            // that contains a specific value.
            Console.WriteLine($"naturalSpline.IndexOf(1.4) = {naturalSpline.IndexOf(1.4)}");
            // The method returns -1 when the value is smaller than the lower bound
            // of the first interval, and NumberOfIntervals if the value is equal to or larger than
            // the upper bound of the last interval.


            //
            // Curve Methods
            //

            // The ValueAt method returns the y value of the
            // curve at the specified x value:
            Console.WriteLine($"naturalSpline.ValueAt(2.4) = {naturalSpline.ValueAt(2.4)}");

            // The SlopeAt method returns the slope of the curve
            // a the specified x value:
            Console.WriteLine($"naturalSpline.SlopeAt(2) = {naturalSpline.SlopeAt(2)}");
            // You can verify that the clamped spline has the correct slope at the end points:
            Console.WriteLine($"clampedSpline.SlopeAt(1) = {clampedSpline.SlopeAt(1)}");
            Console.WriteLine($"clampedSpline.SlopeAt(6) = {clampedSpline.SlopeAt(6)}");

            // Cubic splines do not have a defined derivative. The GetDerivative method
            // returns a GeneralCurve:
            Curve derivative = naturalSpline.GetDerivative();
            Console.WriteLine($"Type of derivative: {derivative.GetType().ToString()}");
            Console.WriteLine($"derivative(2) = {derivative.ValueAt(2)}");

            // You can get a Line that is the tangent to a curve
            // at a specified x value using the TangentAt method:
            Polynomial tangent = clampedSpline.TangentAt(2);
            Console.WriteLine("Slope of tangent line at 2 = {0}",
                tangent.Parameters[1]);

            // The integral of a spline curve can be calculated exactly. This technique is
            // often used to approximate the integral of a tabulated function:
            Console.WriteLine("Integral of naturalSpline between 1.4 and 4.6 = {0}",
                naturalSpline.Integral(1.4, 4.6));

        }
    }
}
