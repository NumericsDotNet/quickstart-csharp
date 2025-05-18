//=====================================================================
//
//  File: elementary-functions.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// We use many classes from the Numerics.NET namespace.
using Numerics.NET;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates the use of the elementary functions implemented
    /// by the Elementary class in the Numerics.NET.Curve namespace of Numerics.NET.
    /// </summary>
    class ElementaryFunctionsSample
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // This QuickStart sample deals with elementary
            // functions, implemented in the Elementary class.

            //
            // Elementary functions
            //

            // Evaluating Log(1+x) directly causes significant
            // round-off error when x is close to 0. The
            // Log1PlusX function allows high precision evaluation
            // of this expression for values of x close to 0:
            Console.WriteLine("Logarithm of 1+1e-12");
            Console.WriteLine("  Math.Log: {0}",
                Math.Log(1+1e-12));
            Console.WriteLine("  Log1PlusX: {0}",
                Elementary.Log1PlusX(1e-12));

            // In a similar way, Exp(x) - 1 has a variant,
            // ExpXMinus1, for values of x close to 0:
            Console.WriteLine("Exponential of 1e-12 minus 1.");
            Console.WriteLine("  Math.Exp: {0}",
                Math.Exp(1e-12) - 1);
            Console.WriteLine("  ExpMinus1: {0}",
                Elementary.ExpMinus1(1e-12));

            // The hypotenuse of two numbers that are very large
            // may cause an overflow when not evaluated properly:
            Console.WriteLine("Hypotenuse:");
            double a = 3e200;
            double b = 4e200;
            Console.Write("  Simple method: ");
            try
            {
                double sumOfSquares = a*a + b*b;
                Console.WriteLine(Math.Sqrt(sumOfSquares));
            }
            catch (OverflowException)
            {
                Console.WriteLine("Overflow!");
            }
            Console.WriteLine("  Elementary.Hypot: {0}",
                Elementary.Hypot(a, b));

            // Raising numbers to integer powers is much faster
            // than raising numbers to real numbers. The
            // overloaded Pow method implements this:
            Console.WriteLine("2.5^19 = {0}", Elementary.Pow(2.5, 19));
            // You can raise numbers to negative integer powers
            // as well:
            Console.WriteLine("2.5^-19 = {0}", Elementary.Pow(2.5,-19));

        }
    }
}
