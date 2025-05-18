//=====================================================================
//
//  File: prime-numbers.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// We use many classes from the Numerics.NET.SpecialFunctions
// namespace.
using Numerics.NET;

// Illustrates working with prime numbers using the
// IntegerMath class in the Numerics.NET.SpecialFunctions
// namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

//
// Factoring numbers
//

// The Factor method returns a sequence of pairs of the prime factors
// and their multiplicity:
int index;

int n = 1001110110;
var factors = IntegerMath.Factor(n);
Console.Write($"{n} = ");
string separator = "";
foreach (var factor in factors)
{
    Console.Write($"{separator}{factor.Item1}");
    if (factor.Item2 > 1)
        Console.Write($"^{factor.Item2}");
    separator = " * ";
}
Console.WriteLine();

n = 256 * 6157413;
factors = IntegerMath.Factor(n);
Console.Write($"{n} = ");
separator = "";
foreach (var factor in factors)
{
    Console.Write($"{separator}{factor.Item1}");
    if (factor.Item2 > 1)
        Console.Write($"^{factor.Item2}");
    separator = " * ";
}
Console.WriteLine();

// The 64bit version can safely factor numbers up to 48 bits long:
long n2 = 1296523 * 1177157L;
var factors2 = IntegerMath.Factor(n2);
Console.Write($"{n2} = ");
separator = "";
foreach (var factor in factors2)
{
    Console.Write($"{separator}{factor.Item1}");
    if (factor.Item2 > 1)
        Console.Write($"^{factor.Item2}");
    separator = " * ";
}
Console.WriteLine();

// Let's try a longer number
n2 = 4292017459171704241;
factors2 = IntegerMath.Factor(n2);
Console.Write($"{n2} = ");
separator = "";
foreach (var factor in factors2)
{
    Console.Write($"{separator}{factor.Item1}");
    if (factor.Item2 > 1)
        Console.Write($"^{factor.Item2}");
    separator = " * ";
}
Console.WriteLine();

//
// Prime numbers
//

// The IsPrime method verifies if a number is prime or not.
n = 801853937;
Console.WriteLine("{0} is prime? {1}!", n, IntegerMath.IsPrime(n));
n = 801853939;
Console.WriteLine("{0} is prime? {1}!", n, IntegerMath.IsPrime(n));

// MextPrime gets the first prime after a specified number.
// You can call it repeatedly to get successive primes.
// Let//s get the 10 smallest primes larger than one billion:
n = 1000000000;
Console.WriteLine("\nFirst 10 primes greater than 1 billion:");
for(index = 0; index < 10; index++)
{
    n = IntegerMath.NextPrime(n);
    Console.Write("{0,16}", n);
}
Console.WriteLine();

// PreviousPrime gets the last prime before a specified number.
n = 1000000000;
Console.WriteLine("Last 10 primes less than 1 billion:");
for(index = 0; index < 10; index++)
{
    n = IntegerMath.PreviousPrime(n);
    Console.Write("{0,16}", n);
}
Console.WriteLine();

