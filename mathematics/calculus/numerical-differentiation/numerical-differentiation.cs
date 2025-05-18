//=====================================================================
//
//  File: numerical-differentiation.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// The numerical differentiation classes reside in the
// Numerics.NET.Calculus namespace.
using Numerics.NET.Calculus;
// Function delegates reside in the Numerics.NET
// namespace.
using Numerics.NET;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates numerical differentiation using the
    /// FunctionMath class in the Numerics.NET
    /// namespace of Numerics.NET.
    /// </summary>
    class NumericalDifferentiationSample
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // Numerical differentiation is a fairly simple
            // procedure. Its accuracy is inherently limited
            // because of unavoidable round-off error.
            //
            // All calculations are performed by static methods
            // of the FunctionMath class. All methods are extension
            // methods, so they can be applied to the delegates
            // directly.

            double result;
            double estimatedError;

            //
            // Standard numerical differentiation.
            //

            // Central differences are the standard way of
            // approximating the result of a function.
            // For this to work, it must be possible to
            // evaluate the target function on both sides of
            // the point where the numerical result is
            // requested.

            // The function must be provided as a
            // Func<double, double>. For more information about
            // this delegate, see the FunctionDelegates
            // QuickStart Sample.
            Func<double, double> fCentral = Math.Cos;

            Console.WriteLine("Central differences:");
            // The actual calculation is performed by the
            // CentralDerivative method.
            result = fCentral.CentralDerivative(1.0);
            Console.WriteLine($"  Result = {result}");
            Console.WriteLine($"  Actual = {-Math.Sin(1.0)}");
            // This method is overloaded. It has an optional
            // out parameter that returns an estimate for the
            // error in the result.
            result = fCentral.CentralDerivative(1.0, out estimatedError);
            Console.WriteLine("Estimated error = {0}",
                estimatedError);

            //
            // Forward and backward differences.
            //

            // Some functions are not defined everywhere.
            // If the result is required on a boundary
            // of the domain where it is defined, the central
            // differences method breaks down. This also happens
            // if the function has a discontinuity close to the
            // differentiation point.
            //
            // In these cases, either forward or backward
            // differences may be used instead.
            //
            // Here is an example of a function that may require
            // forward differences. It is undefined for
            // x < -2:
            Func<double, double> fForward = x => (x+2) * (x+2) * Math.Sqrt(x+2);

            // Calculating the derivative using central
            // differences returns NaN (Not a Number):
            result = fForward.CentralDerivative(-2.0, out estimatedError);
            Console.WriteLine("Using central differences may not work:");
            Console.WriteLine($"  Derivative = {result}");
            Console.WriteLine($"  Estimated error = {estimatedError}");

            // Using the ForwardDerivative method does work:
            Console.WriteLine("Using forward differences instead:");
            result = fForward.ForwardDerivative(-2.0, out estimatedError);
            Console.WriteLine($"  Derivative = {result}");
            Console.WriteLine($"  Estimated error = {estimatedError}");

            // The FBackward function at the end of this file
            // is an example of a function that requires
            // backward differences for differentiation at
            // x = 0.
            Func<double, double> fBackward = x => x > 0.0 ? 1.0 : Math.Sin(x);
            Console.WriteLine("Using backward differences:");
            result = fBackward.BackwardDerivative(0.0, out estimatedError);
            Console.WriteLine($"  Derivative = {result}");
            Console.WriteLine($"  Estimated error = {estimatedError}");

            //
            // Derivative function
            //

            // In some cases, it may be useful to have the
            // derivative of a function in the form of a
            // Func<double, double>, so it can be passed as
            // an argument to other methods. This is very
            // easy to do.
            Console.WriteLine("Using delegates:");

            // For central differences:
            Func<double, double> dfCentral = fCentral.GetNumericalDifferentiator();
            Console.WriteLine($"Central: f'(1) = {dfCentral(1)}");

            // For forward differences:
            Func<double, double> dfForward = fForward.GetForwardDifferentiator();
            Console.WriteLine($"Forward: f'(-2) = {dfForward(-2)}");

            // For backward differences:
            Func<double, double> dfBackward = fBackward.GetBackwardDifferentiator();
            Console.WriteLine($"Backward: f'(0) = {dfBackward(0)}");

        }
    }
}
