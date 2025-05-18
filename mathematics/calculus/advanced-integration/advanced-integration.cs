//=====================================================================
//
//  File: advanced-integration.cs
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

// Illustrates the more advanced use of the
// AdaptiveGaussKronrodIntegrator numerical integrator class
// classes in the Numerics.NET.Calculus namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// Numerical integration algorithms fall into two
// main categories: adaptive and non-adaptive.
// This QuickStart Sample illustrates the use of
// the adaptive numerical integrator implemented by
// the AdaptiveIntegrator class. This class is the
// most advanced of the numerical integration
// classes.
//
// All numerical integration classes derive from
// NumericalIntegrator. This abstract base class
// defines properties and methods that are shared
// by all numerical integration classes.

//
// The integrand
//

// The function we are integrating must be
// provided as a Func<double, double>. For more
// information about this delegate, see the
// FunctionDelegates QuickStart sample.
//
// Variable to hold the result:
double result;
// Construct an instance of the integrator class:
AdaptiveIntegrator integrator = new AdaptiveIntegrator();

//
// Adaptive integrator basics
//

// All the properties and methods defined by the
// NumericalIntegrator base class are available.
// See the BasicIntegration QuickStart Sample
// for details. The AdaptiveIntegrator class defines
// the following additional properties:
//
// The IntegrationRule property gets or sets the
// integration rule that is to be used for
// integrating subintervals. It can be any
// object derived from IntegrationRule.
//
// For convenience, a series of Gauss-Kronrod
// integration rules of order 15, 21, 31, 41, 51,
// and 61 have been provided.
integrator.IntegrationRule = IntegrationRules.GaussKronrod(15);
// The UseAcceleration property specifies whether
// precautions should be taken for singularities
// in the integration interval.
integrator.UseExtrapolation = false;
// Finally, the Singularities property allows you
// to specify singularities or discontinuities
// inside the integration interval. See the
// sample below for details.

//
// Integration over infinite intervals
//

integrator.AbsoluteTolerance = 1e-8;
integrator.ConvergenceCriterion = ConvergenceCriterion.WithinAbsoluteTolerance;
// The Integrate method performs the actual
// integration. To integrate over an infinite
// interval, simply use either or both of
// double.PositiveInfinity and
// double.NegativeInfinity as bounds:
result = integrator.Integrate(x => Math.Exp(-x - x*x),
    double.NegativeInfinity, double.PositiveInfinity);

Console.WriteLine("Exp(-x^2-x) on [-inf,inf]");
Console.WriteLine($"  Value:       {integrator.Result}");
Console.WriteLine($"  Exact value: {Math.Exp(0.25) * Constants.SqrtPi}");
// To see whether the algorithm ended normally,
// inspect the Status property:
Console.WriteLine($"  Status: {integrator.Status}");
Console.WriteLine($"  Estimated error: {integrator.EstimatedError}");
Console.WriteLine($"  Iterations: {integrator.IterationsNeeded}");
Console.WriteLine($"  Function evaluations: {integrator.EvaluationsNeeded}");

// If you just want the result, you can also call the Integrate
// extension method directly on the integrand:
Func<double,double> integrand = x => Math.Exp(-x - x * x);
result = integrand.Integrate(double.NegativeInfinity, double.PositiveInfinity);
Console.WriteLine($"  Value:       {result}");

//
// Functions with singularities at the end points
// of the integration interval.
//

// Thanks to the adaptive nature of the algorithm,
// special measures can be taken to accelerate
// convergence near singularities. To enable this
// acceleration, set the Singularities property
// to true.
integrator.UseExtrapolation = true;
// We'll use the function that gives the Romberg
// integrator in the BasicIntegration QuickStart
// sample trouble.
result = integrator.Integrate(x => Math.Pow(x,-0.9) * Math.Log(1/x), 0.0, 1.0);
Console.WriteLine("Singularities on boundary:");
Console.WriteLine($"  Value:       {integrator.Result}");
Console.WriteLine("  Exact value: 100");
Console.WriteLine("  Status: {0}",
    integrator.Status);
Console.WriteLine("  Estimated error: {0}",
    integrator.EstimatedError);
// Where Romberg integration failed after 1,000,000
// function evaluations, we find the correct answer
// to within tolerance using only 135 function
// evaluations!
Console.WriteLine("  Iterations: {0}",
    integrator.IterationsNeeded);
Console.WriteLine("  Function evaluations: {0}",
    integrator.EvaluationsNeeded);

//
// Functions with singularities or discontinuities
// inside the interval.
//
integrator.UseExtrapolation = true;
// We will pass an array containing the interior
// singularities to the integrator through the
// Singularities property:
integrator.SetSingularities(1, Math.Sqrt(2));
integrator.Integrate(x => x*x*x * Math.Log(Math.Abs((x*x-1) * (x*x - 2))),
    0.0, 3.0);
Console.WriteLine("Singularities inside the interval:");
Console.WriteLine($"  Value:       {integrator.Result}");
Console.WriteLine("  Exact value: 52.740748383471444998");
Console.WriteLine("  Status: {0}",
    integrator.Status);
Console.WriteLine("  Estimated error: {0}",
    integrator.EstimatedError);
Console.WriteLine("  Iterations: {0}",
    integrator.IterationsNeeded);
Console.WriteLine("  Function evaluations: {0}",
    integrator.EvaluationsNeeded);

