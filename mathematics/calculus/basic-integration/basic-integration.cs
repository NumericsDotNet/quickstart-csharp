//=====================================================================
//
//  File: basic-integration.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// The numerical integration classes reside in the
// Numerics.NET.Calculus namespace.
using Numerics.NET.Calculus;
// Function delegates reside in the Numerics.NET
// namespace.
using Numerics.NET;

// Illustrates the basic use of the numerical integration
// classes in the Numerics.NET.Calculus namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// Numerical integration algorithms fall into two
// main categories: adaptive and non-adaptive.
// This QuickStart Sample illustrates the use of
// the non-adaptive numerical integrators.
//
// All numerical integration classes derive from
// NumericalIntegrator. This abstract base class
// defines properties and methods that are shared
// by all numerical integration classes.

//
// The integrand
//

// Variable to hold the result:
double result;

//
// SimpsonIntegrator
//

// The simplest numerical integration algorithm
// is Simpson's rule.
SimpsonIntegrator simpson = new SimpsonIntegrator();
// You can set the relative or absolute tolerance
// to which to evaluate the integral.
simpson.RelativeTolerance = 1e-5;
// You can select the type of tolerance using the
// ConvergenceCriterion property:
simpson.ConvergenceCriterion =
    ConvergenceCriterion.WithinRelativeTolerance;
// The Integrate method performs the actual
// integration:
result = simpson.Integrate(Math.Sin, 0, 2);
Console.WriteLine("sin(x) on [0,2]");
Console.WriteLine("Simpson integrator:");
// The result is also available in the Result
// property:
Console.WriteLine($"  Value: {simpson.Result}");
// To see whether the algorithm ended normally,
// inspect the Status property:
Console.WriteLine($"  Status: {simpson.Status}");
// You can find out the estimated error of the result
// through the EstimatedError property:
Console.WriteLine($"  Estimated error: {simpson.EstimatedError}");
// The number of iterations to achieve the result
// is available through the IterationsNeeded property.
Console.WriteLine($"  Iterations: {simpson.IterationsNeeded}");
// The number of function evaluations is available
// through the EvaluationsNeeded property.
Console.WriteLine($"  Function evaluations: {simpson.EvaluationsNeeded}");

//
// Gauss-Kronrod Integration
//

// Gauss-Kronrod integrators also use a fixed point
// scheme, but with certain optimizations in the
// choice of points where the integrand is evaluated.

// The NonAdaptiveGaussKronrodIntegrator uses a
// succession of 10, 21, 43, and 87 point rules
// to approximate the integral.
NonAdaptiveGaussKronrodIntegrator nagk =
    new NonAdaptiveGaussKronrodIntegrator();
nagk.Integrate(Math.Sin, 0, 2);
Console.WriteLine("Non-adaptive Gauss-Kronrod rule:");
Console.WriteLine($"  Value: {nagk.Result}");
Console.WriteLine($"  Status: {nagk.Status}");
Console.WriteLine($"  Estimated error: {nagk.EstimatedError}");
Console.WriteLine($"  Iterations: {nagk.IterationsNeeded}");
Console.WriteLine($"  Function evaluations: {nagk.EvaluationsNeeded}");

//
// Romberg Integration
//

// Romberg integration combines Simpson's Rule
// with a scheme to accelerate convergence.
// This algorithm is useful for smooth integrands.
RombergIntegrator romberg = new RombergIntegrator();
result = romberg.Integrate(Math.Sin, 0, 2);
Console.WriteLine("Romberg integration:");
Console.WriteLine($"  Value: {romberg.Result}");
Console.WriteLine($"  Status: {romberg.Status}");
Console.WriteLine($"  Estimated error: {romberg.EstimatedError}");
Console.WriteLine($"  Iterations: {romberg.IterationsNeeded}");
Console.WriteLine($"  Function evaluations: {romberg.EvaluationsNeeded}");

// However, it breaks down if the integration
// algorithm contains singularities or
// discontinuities.
//
// The AdaptiveIntegrator can handle this type
// of integrand, and many other difficult cases.
// See the AdvancedIntegration QuickStart sample
// for details.
result = romberg.Integrate(x => x <= 0.0 ? 0.0 : Math.Pow(x,-0.9) * Math.Log(1/x),
    0.0, 1.0);
Console.WriteLine("Romberg on hard integrand:");
Console.WriteLine($"  Value: {romberg.Result}");
Console.WriteLine("  Actual value: 100");
Console.WriteLine($"  Status: {romberg.Status}");
Console.WriteLine($"  Estimated error: {romberg.EstimatedError}");
Console.WriteLine($"  Iterations: {romberg.IterationsNeeded}");
Console.WriteLine($"  Function evaluations: {romberg.EvaluationsNeeded}");


/// <summary>
/// Function that will cause difficulties to the
/// simplistic integration algorithms.
/// </summary>
static double HardIntegrand(double x)
{
    // This is put in because some integration rules
    // evaluate the function at x=0.
    if (x <= 0)
        return 0;
    return Math.Pow(x,-0.9) * Math.Log(1/x);
}
