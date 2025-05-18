//=====================================================================
//
//  File: advanced-polynomials.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// The Complex<T> structure resides in the Numerics.NET namespace.
using Numerics.NET;
// The Polynomial class resides in the Numerics.NET.Curves namespace.
using Numerics.NET.Curves;

// Illustrates the more advanced uses of the Polynomial class
// in the Numerics.NET.Curve namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// Basic operations on polynomials are covered in the
// BasicPolynomials QuickStart Sample. This QuickStart
// Sample focuses on more advanced topics, including
// finding complex roots, calculating least-squares
// polynomials, and polynomial arithmetic.

// Index variable.
int index;

//
// Complex numbers and polynomials
//

Polynomial polynomial = new Polynomial(new Double[] {-2, 0, 1, 1});

// The Polynomial class supports complex numbers
// as arguments for polynomials. It does not support
// polynomials with complex coefficients.
//
// For more about complex numbers, see the
// ComplexNumbers QuickStart Sample.
Complex<double> z1 = new Complex<double>(1, 2);

// Polynomial provides variants of ValueAt and
// SlopeAt for complex arguments:
Console.WriteLine("polynomial.ComplexValueAt({0}) = {1}",
    z1, polynomial.ComplexValueAt(z1));
Console.WriteLine("polynomial.ComplexSlopeAt({0}) = {1}",
    z1, polynomial.ComplexSlopeAt(z1));

//
// Real and complex roots
//
// Our polynomial has only one real root:
double[] roots = polynomial.FindRoots();
Console.WriteLine("Number of roots of polynomial1: {0}",
    roots.Length);
Console.WriteLine($"Value of root 1 = {roots[0]}");
// The FindComplexRoots method returns all three
// roots, two of which are complex:
Complex<double>[] complexRoots = polynomial.FindComplexRoots();
Console.WriteLine("Number of complex roots: {0}",
    complexRoots.Length);
Console.WriteLine("Value of root 1 = {0}",
    complexRoots[0]);
Console.WriteLine("Value of root 2 = {0}",
    complexRoots[1]);
Console.WriteLine("Value of root 3 = {0}",
    complexRoots[2]);

//
// Least squares polynomials
//

// Let's approximate 7 points on the unit circle
// by a fourth degree polynomial in the least squares
// sense.
// First, we create two arrays containing the x and
// y values of our data points:
double[] xValues = new double[7];
double[] yValues = new double[7];
double angle = 0;
for(index = 0; index < 7; index++)
{
    xValues[index] = Math.Cos(angle);
    yValues[index] = -Math.Sin(angle);
    angle = angle + Constants.Pi / 6;
}
// Now we can find the least squares polynomial
// by calling the ststic LeastSquaresFit method.
// The last parameter is the degree of the desired
// polynomial.
Polynomial lsqPolynomial =
    Polynomial.LeastSquaresFit(xValues, yValues, 4);
// Note that, as expected, the odd coefficients
// are close to zero.
Console.WriteLine("Least squares fit: {0}",
    lsqPolynomial.ToString());

//
// Polynomial arithmetic
//

// We can add, subtract, multiply and divide
// polynomials using overloaded operators:
Polynomial a = new Polynomial(new Double[] {4, -2, 4});
Polynomial b = new Polynomial(new Double[] {-3, 1});

Console.WriteLine($"a = {a.ToString()}");
Console.WriteLine($"b = {b.ToString()}");
Polynomial c = a + b;
Console.WriteLine($"a + b = {c.ToString()}");
c = a - b;
Console.WriteLine($"a - b = {c.ToString()}");
c = a * b;
Console.WriteLine($"a * b = {c.ToString()}");
c = a / b;
Console.WriteLine($"a / b = {c.ToString()}");
c = a % b;
Console.WriteLine($"a % b = {c.ToString()}");
// You can also calculate quotient and remainder
// at the same time by calling the overloaded Divide
// method:
Polynomial d;
c = Polynomial.Divide(a, b, out d);
Console.WriteLine("Using Divide method:");
Console.WriteLine($"  a / b = {c.ToString()}");
Console.WriteLine($"  a % b = {d.ToString()}");

