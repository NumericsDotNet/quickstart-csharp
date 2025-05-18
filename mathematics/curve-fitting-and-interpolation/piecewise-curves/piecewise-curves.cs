//=====================================================================
//
//  File: piecewise-curves.cs
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
// The piecewise curve classes reside in the
// Numerics.NET.Curves namespace.
using Numerics.NET.Curves;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates the use of the PiecewiseConstantCurve and
    /// PiecewiseLinearCurve classes.
    /// </summary>
    class PiecewiseCurves
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // A piecewise curve is a curve that has a different definition
            // on subintervals of its domain.
            //
            // This QuickStart Sample illustrates constant and linear piecewise
            // curves, which - as the name suggest - are constant or linear
            // on each interval.
            //
            // For an example of cubic splines, see the CubicSplines QuickStart
            // Sample.
            //

            //
            // Piecewise constants
            //

            // All piecewise curves inherit from the PiecewiseCurve class.
            // Piecewise constant curves are implemented by the
            // PiecewiseConstantCurve class. It has three constructors.

            // The first constructor takes two double arrays as parameters.
            // These contain the x and y values of the data points:
            double[] xValues = {1, 2, 3, 4, 5, 6};
            double[] yValues = {1, 3, 4, 3, 4, 2};
            var constant1 = new PiecewiseConstantCurve(xValues, yValues);

            // The second constructor takes two vectors, containing the
            // x and y-values of the data points:
            var xVector = Vector.Create(xValues);
            var yVector = Vector.Create(yValues);
            var constant2 = new PiecewiseConstantCurve(xVector, yVector);

            // The third constructor only takes one parameter: an array of
            // Point structures that represent the data point.
            var dataPoints = new Point[] {new Point(1, 1), new Point(2, 3), new Point(3, 4),
                new Point(4, 3), new Point(5, 4), new Point(6, 2)};
            var constant3 = new PiecewiseConstantCurve(dataPoints);

            //
            // Curve Parameters
            //

            // The shape of any curve is determined by a set of parameters.
            // These parameters can be retrieved and set through the
            // Parameters collection. The number of parameters for a curve
            // is given by this collection's Count property.
            //
            // Piecewise constant curves have 2n parameters, where n is the number of
            // data points. The first n parameters are the x-values. The next
            // n parameters are the y-values.

            Console.WriteLine("constant1.Parameters.Count = {0}",
                constant1.Parameters.Count);
            // Parameters can easily be retrieved:
            Console.WriteLine("constant1.Parameters[0] = {0}",
                constant1.Parameters[0]);
            // Parameters can also be set:
            constant1.Parameters[0] = 1;

            //
            // Curve Methods
            //

            // The ValueAt method returns the y value of the
            // curve at the specified x value:
            Console.WriteLine($"constant1.ValueAt(2.4) = {constant1.ValueAt(2.4)}");

            // The SlopeAt method returns the slope of the curve
            // a the specified x value:
            Console.WriteLine($"constant1.SlopeAt(2.4) = {constant1.SlopeAt(2.4)}");
            // The slope at the data points is Double.NaN if the value of the constant
            // is different on either side of the data point:
            Console.WriteLine($"constant1.SlopeAt(2) = {constant1.SlopeAt(2)}");

            // Piecewise constant curves do not have a defined derivative.
            // The GetDerivative method returns a GeneralCurve:
            Curve derivative = constant1.GetDerivative();
            Console.WriteLine($"Type of derivative: {derivative.GetType().ToString()}");
            Console.WriteLine($"derivative(2.4) = {derivative.ValueAt(2.4)}");

            // You can get a Line that is the tangent to a curve
            // at a specified x value using the TangentAt method:
            Polynomial tangent = constant1.TangentAt(2.4);
            Console.WriteLine("Slope of tangent line at 2.4 = {0}",
                tangent.Parameters[1]);

            // The integral of a piecewise constant curve can be calculated exactly.
            Console.WriteLine("Integral of constant1 between 1.4 and 4.6 = {0}",
                constant1.Integral(1.4, 4.6));

            //
            // Piecewise linear curves
            //

            // Piecewise linear curves are used for linear interpolation
            // between data points. They are implemented by the
            // PiecewiseLinearCurve class. It has three constructors,
            // similar to the constructors for the PiecewiseLinearCurve
            // class..These constructors create the linear interpolating
            // curve between the data points.

            // The first constructor takes two double arrays as parameters.
            // These contain the x and y values of the data points:
            double[] xValues2 = {1, 2, 3, 4, 5, 6};
            double[] yValues2 = {1, 3, 4, 3, 4, 2};
            var line1 = new PiecewiseLinearCurve(xValues2, yValues2);

            // The second constructor takes two vectors, containing the
            // x and y-values of the data points:
            var xVector2 = Vector.Create(xValues2);
            var yVector2 = Vector.Create(yValues2);
            var line2 = new PiecewiseLinearCurve(xVector2, yVector2);

            // The third constructor only takes one parameter: an array of
            // Point structures that represent the data point.
            Point[] dataPoints2 = new Point[] {new Point(1, 1), new Point(2, 3), new Point(3, 4),
                 new Point(4, 3), new Point(5, 4), new Point(6, 2)};
            PiecewiseLinearCurve line3 = new PiecewiseLinearCurve(dataPoints);

            //
            // Curve Parameters
            //

            // Piecewise linear curves have 2n parameters, where n is the number of
            // data points. The first n parameters are the x-values. The next
            // n parameters are the y-values.

            Console.WriteLine("line1.Parameters.Count = {0}",
                line1.Parameters.Count);
            // Parameters can easily be retrieved:
            Console.WriteLine("line1.Parameters[0] = {0}",
                line1.Parameters[0]);
            // Parameters can also be set:
            line1.Parameters[0] = 1;

            //
            // Curve Methods
            //

            // The ValueAt method returns the y value of the
            // curve at the specified x value:
            Console.WriteLine($"line1.ValueAt(2.4) = {line1.ValueAt(2.4)}");

            // The SlopeAt method returns the slope of the curve
            // a the specified x value:
            Console.WriteLine($"line1.SlopeAt(2.4) = {line1.SlopeAt(2.4)}");
            // The slope at the data points is Double.NaN if the slope of the line
            // is different on either side of the data point:
            Console.WriteLine($"line1.SlopeAt(2) = {line1.SlopeAt(2)}");

            // Piecewise line curves do not have a defined derivative.
            // The GetDerivative method returns a GeneralCurve:
            derivative = line1.GetDerivative();
            Console.WriteLine($"Type of derivative: {derivative.GetType().ToString()}");
            Console.WriteLine($"derivative(2.4) = {derivative.ValueAt(2.4)}");

            // You can get a Line that is the tangent to a curve
            // at a specified x value using the TangentAt method:
            tangent = line1.TangentAt(2.4);
            Console.WriteLine("Slope of tangent line at 2.4 = {0}",
                tangent.Parameters[1]);

            // The integral of a piecewise line curve can be calculated exactly.
            Console.WriteLine("Integral of line1 between 1.4 and 4.6 = {0}",
                line1.Integral(1.4, 4.6));

        }
    }
}
