//=====================================================================
//
//  File: optimization-in-nd.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// The optimization classes reside in the
// Numerics.NET.Optimization namespace.
using Numerics.NET.Optimization;
// Function delegates reside in the Numerics.NET
// namespace.
using Numerics.NET;
// Vectors reside in the Numerics.NET.LinearAlgebra
// namespace.
using Numerics.NET.LinearAlgebra;

// Illustrates unconstrained optimization in multiple dimensions
// using classes in the Numerics.NET.Optimization
// namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

//
// Objective function
//

// The objective function must be supplied as a
// Func<Vector<double>, double> delegate. This is a method
// that takes one var argument and returns a real number.
// See the end of this sample for definitions of the methods
// that are referenced here.
Func<Vector<double>, double> f = fRosenbrock;

// The gradient of the objective function can be supplied either
// as a MultivariateVectorFunction delegate, or a
// MultivariateVectorFunction delegate. The former takes
// one vector argument and returns a vector. The latter
// takes a second vector argument, which is an existing
// vector that is used to return the result.
Func<Vector<double>, Vector<double>, Vector<double>> g = gRosenbrock;

// The initial values are supplied as a vector:
var initialGuess = Vector.Create(-1.2, 1);
// The actual solution is [1, 1].

//
// Quasi-Newton methods: BFGS and DFP
//

// For most purposes, the quasi-Newton methods give
// excellent results. There are two variations: DFP and
// BFGS. The latter gives slightly better results.

// Which method is used, is specified by a constructor
// parameter of type QuasiNewtonMethod:
var bfgs = new QuasiNewtonOptimizer(QuasiNewtonMethod.Bfgs);

bfgs.InitialGuess = initialGuess;
bfgs.ExtremumType = ExtremumType.Minimum;

// Set the ObjectiveFunction:
bfgs.ObjectiveFunction = f;
// Set either the GradientFunction or FastGradientFunction:
bfgs.FastGradientFunction = g;
// The FindExtremum method does all the hard work:
bfgs.FindExtremum();

Console.WriteLine("BFGS Method:");
Console.WriteLine($"  Solution: {bfgs.Extremum}");
Console.WriteLine($"  Estimated error: {bfgs.EstimatedError}");
Console.WriteLine($"  # iterations: {bfgs.IterationsNeeded}");
// Optimizers return the number of function evaluations
// and the number of gradient evaluations needed:
Console.WriteLine($"  # function evaluations: {bfgs.EvaluationsNeeded}");
Console.WriteLine($"  # gradient evaluations: {bfgs.GradientEvaluationsNeeded}");

// You can use Automatic Differentiation to compute the gradient.
// To do so, set the SymbolicObjectiveFunction to a lambda expression
// for the objective function:
bfgs = new QuasiNewtonOptimizer(QuasiNewtonMethod.Bfgs);

bfgs.InitialGuess = initialGuess;
bfgs.ExtremumType = ExtremumType.Minimum;

bfgs.SymbolicObjectiveFunction = x => Math.Pow((1 - x[0]), 2) + 105 * Math.Pow(x[1] - x[0] * x[0], 2);

bfgs.FindExtremum();

Console.WriteLine("BFGS using Automatic Differentiation:");
Console.WriteLine($"  Solution: {bfgs.Extremum}");
Console.WriteLine($"  Estimated error: {bfgs.EstimatedError}");
Console.WriteLine($"  # iterations: {bfgs.IterationsNeeded}");
Console.WriteLine($"  # function evaluations: {bfgs.EvaluationsNeeded}");
Console.WriteLine($"  # gradient evaluations: {bfgs.GradientEvaluationsNeeded}");

//
// Conjugate Gradient methods
//

// Conjugate gradient methods exist in three variants:
// Fletcher-Reeves, Polak-Ribiere, and positive Polak-Ribiere.

// Which method is used, is specified by a constructor
// parameter of type ConjugateGradientMethod:
ConjugateGradientOptimizer cg =
    new ConjugateGradientOptimizer(ConjugateGradientMethod.PositivePolakRibiere);
// Everything else works as before:
cg.ObjectiveFunction = f;
cg.FastGradientFunction = g;
cg.InitialGuess = initialGuess;
cg.FindExtremum();

Console.WriteLine("Conjugate Gradient Method:");
Console.WriteLine($"  Solution: {cg.Extremum}");
Console.WriteLine($"  Estimated error: {cg.EstimatedError}");
Console.WriteLine($"  # iterations: {cg.IterationsNeeded}");
Console.WriteLine($"  # function evaluations: {cg.EvaluationsNeeded}");
Console.WriteLine($"  # gradient evaluations: {cg.GradientEvaluationsNeeded}");

//
// Powell's method
//

// Powell's method is a conjugate gradient method that
// does not require the derivative of the objective function.
// It is implemented by the PowellOptimizer class:
var pw = new PowellOptimizer();
pw.InitialGuess = initialGuess;
// Powell's method does not use derivatives:
pw.ObjectiveFunction = f;
pw.FindExtremum();

Console.WriteLine("Powell's Method:");
Console.WriteLine($"  Solution: {pw.Extremum}");
Console.WriteLine($"  Estimated error: {pw.EstimatedError}");
Console.WriteLine($"  # iterations: {pw.IterationsNeeded}");
Console.WriteLine($"  # function evaluations: {pw.EvaluationsNeeded}");
Console.WriteLine($"  # gradient evaluations: {pw.GradientEvaluationsNeeded}");

//
// Nelder-Mead method
//

// Also called the downhill simplex method, the method of Nelder
// and Mead is useful for functions that are not tractable
// by other methods. For example, other methods
// may fail if the objective function is not continuous.
// Otherwise it is much slower than other methods.

// The method is implemented by the NelderMeadOptimizer class:
var nm = new NelderMeadOptimizer();

// The class has three special properties, that help determine
// the progress of the algorithm. These parameters have
// default values and need not be set explicitly.
nm.ContractionFactor = 0.5;
nm.ExpansionFactor = 2;
nm.ReflectionFactor = -2;

// Everything else is the same.
nm.SolutionTest.AbsoluteTolerance = 1e-15;
nm.InitialGuess = initialGuess;
// The method does not use derivatives:
nm.ObjectiveFunction = f;
nm.FindExtremum();

Console.WriteLine("Nelder-Mead Method:");
Console.WriteLine($"  Solution: {nm.Extremum}");
Console.WriteLine($"  Estimated error: {nm.EstimatedError}");
Console.WriteLine($"  # iterations: {nm.IterationsNeeded}");
Console.WriteLine($"  # function evaluations: {nm.EvaluationsNeeded}");


// The famous Rosenbrock test function.
static double fRosenbrock(Vector<double> x)
{
    double p = (1-x[0]);
    double q = x[1] - x[0]*x[0];
    return p*p + 105 * q*q;
}

// Gradient of the Rosenbrock test function.
static Vector<double> gRosenbrock(Vector<double> x, Vector<double> f)
{
    // Always assume that the second argument may be null:
    if (f == null)
        f = Vector.Create<double>(2);
    double p = (1-x[0]);
    double q = x[1] - x[0]*x[0];
    f[0] = -2*p - 420*x[0]*q;
    f[1] = 210*q;
    return f;
}
