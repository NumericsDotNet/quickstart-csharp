//=====================================================================
//
//  File: vector-operations.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// The var class resides in the Numerics.NET.LinearAlgebra
// namespace.
using Numerics.NET.LinearAlgebra;
// The delegate class resides in the Numerics.NET
// namespace.
using Numerics.NET;

// Illustrates operations on vectors from the Numerics.NET
// namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// For details on the basic workings of vector
// objects, including constructing, copying and
// cloning vectors, see the BasicVectors QuickStart
// Sample.
//
// Let's create some vectors to work with.
var v1 = Vector.Create(new double[] { 1, 2, 3, 4, 5 });
var v2 = Vector.Create(new double[] { 1, -2, 3, -4, 5 });
var v3 = Vector.Create(new double[] { 3, 2, 1, 0, -1 });
// This one will hold results.
Vector<double> v;

//
// var Arithmetic
//
// The var class defines overloaded addition,
// subtraction, and multiplication and division
// operators:
Console.WriteLine($"v1 = {v1:F4}");
Console.WriteLine($"v2 = {v2:F4}");
Console.WriteLine("Basic arithmetic:");
v = -v1;
Console.WriteLine($"-v1 = {v:F4}");
v = v1 + v2;
Console.WriteLine($"v1 + v2 = {v:F4}");
v = v1 - v2;
Console.WriteLine($"v1 - v2 = {v:F4}");
// Vectors can only be multiplied or divided by
// a real number. For dot products, see the
// DotProduct method.
v = 5 * v1;
Console.WriteLine($"5 * v1 = {v:F4}");
v = v1 * 5;
Console.WriteLine($"v1 * 5 = {v:F4}");
v = v1 / 5;
Console.WriteLine($"v1 / 5 = {v:F4}");

// For each operator, there is a corresponding
// static method. For example: v1 + v2 is
// equivalent to:
v = Vector.Add(v1, v2);
// v1 - v2 corresponds to:
v = Vector.Subtract(v1, v2);
// You can also apply these methods to vector objects.
// In this case, they change the first operand.
Console.WriteLine($"v3 = {v3:F4}");
v3.AddInPlace(v1);
// Note that this is different from the += operator!
// The += operator creates a Vector.Create object,
// whereas the Add method above does not.
Console.WriteLine($"v3+v1 -> v3 = {v3:F4}");
// This method is overloaded so you can directly
// add a scaled vector:
v3.AddScaledInPlace(-2, v1);
Console.WriteLine($"v3-2v1 -> v3 = {v3:F4}");
// Corresponding to the * operator, we have the
// scale method:
v3.MultiplyInPlace(3);
Console.WriteLine($"3v3 -> v3 = {v3:F4}");
Console.WriteLine();

//
// Norms, dot products, etc.
//
Console.WriteLine("Norms, dot products, etc.");
// The dot product is calculated in one of two ways:
// Using the static DotProduct method:
double a = Vector.DotProduct(v1, v2);
// Or using the DotProduct method on one of the two
// vectors:
double b = v1.DotProduct(v2);
Console.WriteLine("DotProduct(v1, v2) = {0:F4} = {0:F4}",
    a, b);
// The Norm method returns the standard two norm
// of a Vector:
a = v1.Norm();
Console.WriteLine($"|v1| = {a:F4}");
// .the Norm method is overloaded to allow other norms,
// including the one-norm:
a = v1.Norm(1);
Console.WriteLine($"one norm(v1) = {a:F4}");
// ...the positive infinity norm, which returns the
// absolute value of the largest component:
a = v1.Norm(double.PositiveInfinity);
Console.WriteLine($"+inf norm(v1) = {a:F4}");
// ...the negative infinity norm, which returns the
// absolute value of the smallest component:
a = v1.Norm(double.NegativeInfinity);
Console.WriteLine($"-inf norm(v1) = {a:F4}");
// ...and even the zero norm, which simply returns
// the number of components of the vector:
a = v1.Norm(0);
Console.WriteLine($"zero-norm(v1) = {a:F4}");
// You can get the square of the two norm with the
// NormSquared method.
a = v1.NormSquared();
Console.WriteLine($"|v1|^2 = {a:F4}");
Console.WriteLine();

//
// Largest and smallest elements
//
// The var class defines methods to find the
// largest or smallest element or its index.
Console.WriteLine($"v2 = {v2:F4}");
// The Max method returns the largest element:
Console.WriteLine($"Max(v2) = {v2.Max():F4}");
// The AbsoluteMax method returns the element with
// the largest absolute value.
Console.WriteLine("Absolute max(v2) = {0:F4}",
    v2.AbsoluteMax());
// The Min method returns the smallest element:
Console.WriteLine($"Min(v2) = {v2.Min():F4}");
// The AbsoluteMin method returns the element with
// the smallest absolute value.
Console.WriteLine("Absolute min(v2) = {0:F4}",
    v2.AbsoluteMin());
// Each of these methods has an equivalent method
// that returns the zero-based index of the element
// instead of its value, for example:
Console.WriteLine("Index of Min(v2) = {0:F4}",
    v2.MinIndex());

// Finally, the Map method lets you apply
// an arbitrary function to each element of the
// vector:
v1.MapInPlace(Math.Exp);
Console.WriteLine($"Exp(v1) = {v1:F4}");
// There is also a static method that returns a
// new vector:
v = Vector.Map(Math.Exp, v2);

