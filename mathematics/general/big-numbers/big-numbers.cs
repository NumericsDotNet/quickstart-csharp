//=====================================================================
//
//  File: big-numbers.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// The arbitrary precision types reside in the Numerics.NET
// namespace.
using Numerics.NET;

// Illustrates the use of the arbitrary precision number
// classes in Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// In this QuickStart sample, we'll use 5 different methods to compute
// the value of pi, the ratio of the circumference to the diameter of
// a circle.
BigFloat pi;

// The number of decimal digits.
int digits = 10000;
// The equivalent number of binary digits, to account for round-off error:
int binaryDigits = (int)(8 + digits * Math.Log(10, 2));
// The number of digits in the last correction, if applicable.
double correctionDigits;

// First, create an AccuracyGoal for the number of digits we want.
// We'll add 5 extra digits to account for round-off error.
AccuracyGoal goal = AccuracyGoal.Absolute(digits + 5);
Console.WriteLine($"Calculating {digits} digits of pi:");

// Create a stopwatch so we can time the results.
System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

//
// Method 1: Arctan formula
//
// pi/4 = 88L(172) + 51L(239) + 32L(682) + 44L(5357) + 68L(12943)
// Where L(p) = Arctan(1/p)
// We will use big integer arithmetic for this.
// See the helper function Arctan later in this file.
Console.WriteLine("Arctan formula using integer arithmetic:");
sw.Start();
int[] coefficients = { 88, 51, 32, 44, 68 };
int[] arguments = { 172, 239, 682, 5357, 12943 };
pi = BigFloat.Zero;
for (int k = 0; k < 5; k++)
{
    pi += coefficients[k] * Arctan(arguments[k], binaryDigits);
    Console.WriteLine("Step {0}: ({1:F3} seconds)", k + 1, sw.Elapsed.TotalSeconds);
}
// The ScaleByPowerOfTwo is the fastest way to multiply
// or divide by a power of two:
pi = BigFloat.ScaleByPowerOfTwo(pi, 2);
sw.Stop();
Console.WriteLine("Total time: {0:F3} seconds.", sw.Elapsed.TotalSeconds, pi);
Console.WriteLine();

//
// Method 2: Rational approximation
//
// pi/2 = 1 + 1/3 + (1*2)/(3*5) + (1*2*3)/(3*5*7) + ...
//      = 1 + 1/3 * (1 + 2/5 * (1 + 3/7 * (1 + ...)))
// We gain 1 bit per iteration, so we know where to start.
Console.WriteLine("Rational approximation using rational arithmetic.");
Console.WriteLine("This is very inefficient, so we only do up to 10000 digits.");
sw.Start();
BigRational an = BigRational.Zero;
int n0 = digits <= 10000 ? binaryDigits : (int)(8 + 10000 * Math.Log(10, 2));
for (int n = n0; n > 0; n--)
    an = new BigRational(n, 2 * n + 1) * an + 1;
pi = new BigFloat(2 * an, goal, RoundingMode.TowardsNearest);
sw.Stop();
Console.WriteLine("Total time: {0:F3} seconds.", sw.Elapsed.TotalSeconds, pi);
Console.WriteLine();

//
// Method 3: Arithmetic-Geometric mean
//
// By Salamin & Brent, based on discoveries by C.F.Gauss.
// See http://www.cs.miami.edu/~burt/manuscripts/gaussagm/agmagain-hyperref.pdf
Console.WriteLine("Arithmetic-Geometric Mean:");
sw.Reset();
sw.Start();
BigFloat x1 = BigFloat.Sqrt(2, goal, RoundingMode.TowardsNearest);
BigFloat x2 = BigFloat.One;
BigFloat S = BigFloat.Zero;
BigFloat c = BigFloat.One;
for (int k = 0; k < int.MaxValue; k++)
{
    S += BigFloat.ScaleByPowerOfTwo(c, k - 1);
    BigFloat aMean = BigFloat.ScaleByPowerOfTwo(x1 + x2, -1);
    BigFloat gMean = BigFloat.Sqrt(x1 * x2);
    x1 = aMean;
    x2 = gMean;
    c = (x1 + x2) * (x1 - x2);
    // GetDecimalDigits returns the approximate number of digits in a number.
    // A negative return value means the number is less than 1.
    correctionDigits = -c.GetDecimalDigits();
    Console.WriteLine("Iteration {0}: {1:F1} digits ({2:F3} seconds)", k, correctionDigits, sw.Elapsed.TotalSeconds);
    if (correctionDigits >= digits)
        break;
}
pi = x1 * x1 / (1 - S);
sw.Stop();
Console.WriteLine("Total time: {0:F3} seconds.", sw.Elapsed.TotalSeconds, pi);
Console.WriteLine();

//
// Method 4: Borweins' quartic formula
//
// This algorithm quadruples the number of correct digits
// in each iteration.
// See http://en.wikipedia.org/wiki/Borwein's_algorithm
Console.WriteLine("Quartic formula:");
sw.Reset();
sw.Start();
BigFloat sqrt2 = BigFloat.Sqrt(2, goal, RoundingMode.TowardsNearest);
BigFloat y = sqrt2 - BigFloat.One;
BigFloat a = new BigFloat(6, goal) - BigFloat.ScaleByPowerOfTwo(sqrt2, 2);
BigFloat y2 = y * y, y3, y4 = y2 * y2;
BigFloat da;
correctionDigits = 0;
for (int k = 1; 4 * correctionDigits < digits; k++)
{
    BigFloat qrt = BigFloat.Root(1 - y4, 4);
    y = BigFloat.Subtract(1, qrt, goal, RoundingMode.TowardsNearest)
        / BigFloat.Add(1, qrt, goal, RoundingMode.TowardsNearest);
    // y = BigFloat.Divide(1 - qrt, 1 + qrt, AccuracyGoal.InheritAbsolute, RoundingMode.TowardsNearest);
    y2 = y * y;
    y3 = y * y2;
    y4 = y2 * y2;
    da = (BigFloat.ScaleByPowerOfTwo(y + y3, 2) + (6 * y2 + y4)) * a
        - BigFloat.ScaleByPowerOfTwo(y + y2 + y3, 2 * k + 1);
    da = da.RestrictPrecision(goal, RoundingMode.TowardsNearest);
    a += da;
    correctionDigits = -da.GetDecimalDigits();
    Console.WriteLine("Iteration {0}: {1:F1} digits ({2:F3} seconds)", k, correctionDigits, sw.Elapsed.TotalSeconds);
}
pi = BigFloat.Inverse(a);
sw.Stop();
Console.WriteLine("Total time: {0:F3} seconds.", sw.Elapsed.TotalSeconds, pi);
Console.WriteLine();

//
// Method 5: The built-in method
//
// The method used to compute pi internally is an order of magnitude
// faster than any of the above.
Console.WriteLine("Built-in function:");
sw.Reset();
sw.Start();
pi = BigFloat.GetPi(goal);
sw.Stop();
Console.WriteLine("Total time: {0:F3} seconds.", sw.Elapsed.TotalSeconds, pi);
// The highest precision value of pi is cached, so
// getting pi to any precision up to that is super fast.
Console.WriteLine("Built-in function (cached):");
sw.Reset();
sw.Start();
pi = BigFloat.GetPi(goal);
sw.Stop();
Console.WriteLine("Total time: {0:F3} seconds.", sw.Elapsed.TotalSeconds, pi);


/// <summary>
/// Helper function to compute Arctan(1/p)
/// </summary>
/// <param name="p">The reciprocal of the argument.</param>
/// <param name="binaryDigits">The number of binary digits in the result.</param>
/// <returns>Arctan(1/<paramref name="p"/>) to <paramref name="binaryDigits"/> binary digits.</returns>
static BigFloat Arctan(int p, int binaryDigits)
{
    // We scale the result by a factor of 2^binaryDigits.
    // The first term is 1/p.
    BigInteger power = BigInteger.Pow(2, binaryDigits) / p;
    // We store the sum in result.
    BigInteger result = power;
    bool subtract = true;
    int k = 0;
    while (!power.IsZero)
    {
        k++;
        // Expressions involving big integers look exactly like any other arithmetic expression:

        // The kth term is (-1)^k 1/(2k+1) 1/p^2k.
        // So the power is 1/p^2 times the previous power.
        power /= (p * p);
        // And we alternately add and subtract
        if (subtract)
            result -= power / (2 * k + 1);
        else
            result += power / (2 * k + 1);
        subtract = !subtract;
    }
    // Scale the result.
    return BigFloat.ScaleByPowerOfTwo(new BigFloat(result), -binaryDigits);
}
