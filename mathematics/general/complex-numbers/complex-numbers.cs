//=====================================================================
//
//  File: complex-numbers.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// The Complex<T> class resides in the Numerics.NET namespace.
using Numerics.NET;

// Illustrates the use of the Complex<double> class in
// Numerics.NET.


// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

//
// Complex constants:
//

Console.WriteLine($"Complex<double>.Zero = {Complex<double>.Zero}");
Console.WriteLine($"Complex<double>.One = {Complex<double>.One}");
// The imaginary unit is given by Complex<double>.I:
Console.WriteLine($"Complex<double>.I = {Complex<double>.I}");
Console.WriteLine();

//
// Construct some complex numbers
//
// Real and imaginary parts:
//   a = 2 + 4i
Complex<double> a = new Complex<double>(2, 4);
Console.WriteLine($"a = {a}");
//   b = 1 - 3i
Complex<double> b = new Complex<double>(1, -3);
Console.WriteLine($"b = {b}");
// From a real number:
//   c = -3 + 0i
Complex<double> c = new Complex<double>(-3);
Console.WriteLine($"c = {c}");
// Polar form:
//   d = 2 (cos(Pi/3) + i sin(Pi/3))
Complex<double> d = Complex<double>.FromPolar(2, Constants.Pi/3);
// To print this number, use the overloaded ToString
// method and specify the format string for the real
// and imaginary parts:
Console.WriteLine($"d = {d:F3}");
// There is an implicit conversion from a tuple of 2 numbers.
// This gives a compact and convenient notation:
d = (1.0, Constants.Sqrt3);
Console.WriteLine($"d = {d:F3}");

Console.WriteLine();

//
// Parts of complex numbers
//
Console.WriteLine($"Parts of a = {a}:");
Console.WriteLine($"Real part of a = {a.Re}");
Console.WriteLine($"Imaginary part of a = {a.Im}");
Console.WriteLine($"Modulus of a = {a.Magnitude}");
Console.WriteLine($"Argument of a = {a.Phase}");
Console.WriteLine();

//
// Basic arithmetic:
//
Console.WriteLine("Basic arithmetic:");
Complex<double> e = -a;
Console.WriteLine($"-a = {e}");
e = a + b;
Console.WriteLine($"a + b = {e}");
e = a - b;
Console.WriteLine($"a - b = {e}");
e = a * b;
Console.WriteLine($"a * b = {e}");
e = a / b;
Console.WriteLine($"a / b = {e}");
// The conjugate of a complex number corresponds to
// the "Conjugate" method:
e = a.Conjugate();
Console.WriteLine($"Conjugate(a) = ~a = {e}");
Console.WriteLine();

//
// Functions of complex numbers
//

// Most of these have corresponding static methods
// in the System.Math class, but are extended to complex
// arguments.
Console.WriteLine("Functions of complex numbers:");

// Exponentials and logarithms
Console.WriteLine($"Exponentials and logarithms:");
e = Complex<double>.Exp(a);
Console.WriteLine($"Exp(a) = {e}");
e = Complex<double>.Log(a);
Console.WriteLine($"Log(a) = {e}");
e = Complex<double>.Log10(a);
Console.WriteLine($"Log10(a) = {e}");
// You can get a point on the unit circle by calling
// the ExpI method:
e = Complex<double>.ExpI(2*Constants.Pi/3);
Console.WriteLine($"ExpI(2*Pi/3) = {e}");
// The RootOfUnity method also returns points on the
// unit circle. The above is equivalent to the second
// root of z^6 = 1:
e = Complex<double>.RootOfUnity(6, 2);
Console.WriteLine($"RootOfUnity(6, 2) = {e}");

// The Pow method is overloaded for integer, double,
// and complex argument:
e = Complex<double>.Pow(a, 3);
Console.WriteLine($"Pow(a,3) = {e}");
e = Complex<double>.Pow(a, 1.5);
Console.WriteLine($"Pow(a,1.5) = {e}");
e = Complex<double>.Pow(a, b);
Console.WriteLine($"Pow(a,b) = {e}");

// Square root
e = Complex<double>.Sqrt(a);
Console.WriteLine($"Sqrt(a) = {e}");
// The Sqrt method is overloaded. Here's the square
// root of a negative double:
e = Complex<double>.Sqrt(-4);
Console.WriteLine($"Sqrt(-4) = {e}");
Console.WriteLine();

//
// Trigonometric functions:
//
Console.WriteLine("Trigonometric function:");
e = Complex<double>.Sin(a);
Console.WriteLine($"Sin(a) = {e}");
e = Complex<double>.Cos(a);
Console.WriteLine($"Cos(a) = {e}");
e = Complex<double>.Tan(a);
Console.WriteLine($"Tan(a) = {e}");

// Inverse Trigonometric functions:
e = Complex<double>.Asin(a);
Console.WriteLine($"Asin(a) = {e}");
e = Complex<double>.Acos(a);
Console.WriteLine($"Acos(a) = {e}");
e = Complex<double>.Atan(a);
Console.WriteLine($"Atan(a) = {e}");

// Asin and Acos have overloads with real argument
// not restricted to [-1,1]:
e = Complex<double>.Asin(2);
Console.WriteLine($"Asin(2) = {e}");
e = Complex<double>.Acos(2);
Console.WriteLine($"Acos(2) = {e}");
Console.WriteLine();

//
// Hyperbolic and inverse hyperbolic functions:
//
Console.WriteLine("Hyperbolic function:");
e = Complex<double>.Sinh(a);
Console.WriteLine($"Sinh(a) = {e}");
e = Complex<double>.Cosh(a);
Console.WriteLine($"Cosh(a) = {e}");
e = Complex<double>.Tanh(a);
Console.WriteLine($"Tanh(a) = {e}");
e = Complex<double>.Asinh(a);
Console.WriteLine($"Asinh(a) = {e}");
e = Complex<double>.Acosh(a);
Console.WriteLine($"Acosh(a) = {e}");
e = Complex<double>.Atanh(a);
Console.WriteLine($"Atanh(a) = {e}");
Console.WriteLine();

//
// Other numeric types
//
var q = new Complex<Quad>(0, 1);
var q2 = Complex<Quad>.Sqrt(q);
Console.WriteLine($"Sqrt(i) in quad precison = {q2}");

