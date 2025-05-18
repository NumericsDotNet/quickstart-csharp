//=====================================================================
//
//  File: generic-algorithms.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// We'll also need the big number types.
using Numerics.NET;

// Illustrates writing generic algorithms that can be
// applied to different operand types using the types in the
// Numerics.NET.Generic namespace.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// To use generic linear algebra, we need a reference
// to the Numerics.NET.Generic package. We then need
// to register the generic provider as follows:
NumericsConfiguration.Providers.RegisterGenericProvider();

// We can create a 10x10 Hilbert matrix
var A = Matrix.CreateFromFunction(10, 10, (i, j) => 1 / (1m + i + j));
// and compute its inverse:
var invA = A.GetInverse();
Console.WriteLine($"{A * invA:F14}");

// We will implement a simple Newton-Raphson solver class.
// The code for the solver is below.

// Here we will call the generic solver with three
// different operand types: BigFloat, BigRational and Double.

// First, let's compute pi to 100 digits
// by solving the equation sin(x) == 0 with
// an initual guess of 3.
Console.WriteLine("Computing pi by solving sin(x) == 0 with x0 = 3 using BigFloat:");
// Create the solver object.
Solver<BigFloat> bigFloatSolver = new Solver<BigFloat>();
// Set the function to solve, and its derivative.
bigFloatSolver.TargetFunction = delegate(BigFloat x) { return BigFloat.Sin(x); };
bigFloatSolver.DerivativeOfTargetFunction = delegate(BigFloat x) { return BigFloat.Cos(x); };
// Now solve to within a tolerance of 10^-100.
BigFloat pi = bigFloatSolver.Solve(3, BigFloat.Pow(10, -100));
// Print the results...
Console.WriteLine($"Computed value: {pi:F100}");
// and verify:
Console.WriteLine("Known value:    {0:F100}",
    BigFloat.GetPi(AccuracyGoal.Absolute(100)));
Console.WriteLine();

// Next, we will use rational numbers to compute
// an approximation to the square root of 2.
Console.WriteLine("Computing sqrt(2) by solving x^2 == 2 using BigRational:");
// Create the solver...
Solver<BigRational> bigRationalSolver = new Solver<BigRational>();
// Set properties...
bigRationalSolver.TargetFunction = delegate(BigRational x) { return x * x - 2; };
bigRationalSolver.DerivativeOfTargetFunction = delegate(BigRational x) { return 2 * x; };
// Compute the solution...
BigRational sqrt2 = bigRationalSolver.Solve(1, BigRational.Pow(10, -100));
// And print the result.
Console.WriteLine($"Rational approximation: {sqrt2}");
// To verify, we convert the BigRational to a BigFloat:
Console.WriteLine("As real number: {0:F100}",
    new BigFloat(sqrt2, AccuracyGoal.Absolute(100), RoundingMode.TowardsNearest));
Console.WriteLine("Known value:    {0:F100}",
    BigFloat.Sqrt(2, AccuracyGoal.Absolute(100), RoundingMode.TowardsNearest));
Console.WriteLine();

// Now, we compute the Lambert W function at x = 3.
Console.WriteLine("Computing Lambert's W at x = 3 by solving x*exp(x) == 3 using double solver:");
// Create the solver...
Solver<double> doubleSolver = new Solver<double>();
// Set properties...
doubleSolver.TargetFunction = delegate(double x) { return x * Math.Exp(x) - 3; };
doubleSolver.DerivativeOfTargetFunction = delegate(double x) { return Math.Exp(x) * (1 + x); };
// Compute the solution...
double W3 = doubleSolver.Solve(1.0, 1e-15);
// And print the result.
Console.WriteLine($"Solution:    {W3}");
Console.WriteLine("Known value: {0}",
    Numerics.NET.Elementary.LambertW(3.0));

// Finally, we use generic functions:
Console.WriteLine("Using generic function delegates:");
// The delegates are slightly more complicated.
doubleSolver.TargetFunction = fGeneric<double>;
doubleSolver.DerivativeOfTargetFunction = dfGeneric<double>;
double genericW3 = doubleSolver.Solve(1, 1e-15);
Console.WriteLine($"Double:      {genericW3}");
bigFloatSolver.TargetFunction = fGeneric<BigFloat>;
bigFloatSolver.DerivativeOfTargetFunction = dfGeneric<BigFloat>;
BigFloat bigW3 = bigFloatSolver.Solve(1, BigFloat.Pow(10, -100));
Console.WriteLine($"BigFloat:    {bigW3:F100}");


// Generic versions of the above
static T fGeneric<T>(T x)
{
    return Operations<T>.Subtract(
        Operations<T>.Multiply(x, Operations<T>.Exp(x)),
        Operations<T>.FromInt32(3));
}
static T dfGeneric<T>(T x)
{
    return Operations<T>.Multiply(
        Operations<T>.Exp(x),
        Operations<T>.Add(x, Operations<T>.One));
}

// Class that contains the generic Newton-Raphson algorithm.
// <typeparam name="T">The operand type.</typeparam>
class Solver<T>
{
    // Member fields:
    Func<T,T> f, df;
    int maxIterations = 100;

    // The function to solve:
    public Func<T,T> TargetFunction
    {
        get { return f; }
        set { f = value; }
    }
    // The derivative of the function to solve.
    public Func<T,T> DerivativeOfTargetFunction
    {
        get { return df; }
        set { df = value; }
    }
    // The maximum number of iterations.
    public int MaxIterations
    {
        get { return maxIterations; }
        set { maxIterations = value; }
    }

    // The core algorithm.
    // Arithmetic operations are replaced by calls to
    // methods on the arithmetic object (ops).
    public T Solve(T initialGuess, T tolerance)
    {
        int iterations = 0;

        T x = initialGuess;
        T dx = Operations<T>.Zero;
        do
        {
            iterations++;
            // Compute the denominator of the correction term.
            T dfx = df(x);
            // Relational operators map to the Compare method.
            // We also use the value of zero for the operand type.
            // if (dfx == 0)
            if (Operations<T>.IsZero(dfx))
            {
                // Change value by 2x tolerance.
                // When multiplying by a power of two, it's more efficient
                // to use the ScaleByPowerOfTwo method.
                dx = Operations<T>.ScaleByPowerOfTwo(tolerance, 1);
            }
            else
            {
                // dx = f(x) / df(x)
                dx = Operations<T>.Divide(f(x), dfx);
            }
            // x -= dx;
            x = Operations<T>.Subtract(x, dx);

            // if |dx|^2<tolerance
            // Convergence is quadratic (in most cases), so we should be good here:
            if (Operations<T>.LessThan(Operations<T>.Multiply(dx,dx), tolerance))
                return x;
        }
        while (iterations < MaxIterations);
        return x;
    }
}
