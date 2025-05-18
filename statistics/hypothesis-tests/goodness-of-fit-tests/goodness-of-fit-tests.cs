//=====================================================================
//
//  File: goodness-of-fit-tests.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
using Numerics.NET.Statistics;
using Numerics.NET.Statistics.Distributions;
using Numerics.NET.Statistics.Tests;
using Numerics.NET;

// Illustrates the Chi Square, Kolmogorov-Smirnov and Anderson-Darling
// tests for goodness-of-fit.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// This QuickStart Sample illustrates the wide variety of goodness-of-fit
// tests available.

//
// Chi-square Test
//

Console.WriteLine("Chi-square test.");

// The Chi-square test is the simplest of the goodness-of-fit tests.
// The results follow a binomial distribution with 3 trials (rolls of the dice):
BinomialDistribution sixesDistribution = new BinomialDistribution(3, 1/6.0);

// First, create a histogram with the expected results.
var expected = sixesDistribution.GetExpectedHistogram(100);

// And a histogram with the actual results
var actual = Vector.Create(new double[] {51, 35, 12, 2});
var chiSquare = new ChiSquareGoodnessOfFitTest(actual, expected);
chiSquare.SignificanceLevel = 0.01;

// We can obtan the value of the test statistic through the Statistic property,
// and the corresponding P-value through the Probability property:
Console.WriteLine($"Test statistic: {chiSquare.Statistic:F4}");
Console.WriteLine($"P-value:        {chiSquare.PValue:F4}");

// We can now print the test results:
Console.WriteLine("Reject null hypothesis? {0}",
    chiSquare.Reject() ? "yes" : "no");

//
// One-sample Kolmogorov-Smirnov Test
//

Console.WriteLine("\nOne-sample Kolmogorov-Smirnov Test");

// We will investigate a sample of 25 random numbers from a lognormal distribution
// and investigate how well it matches a similar looking Weibull distribution.

// We first create the two distributions:
LognormalDistribution logNormal = new LognormalDistribution(0, 1);
WeibullDistribution weibull = new WeibullDistribution(2, 1);

// Then we generate the samples from the lognormal distribution:
var logNormalSample = logNormal.Sample(25);

// Finally, we construct the Kolmogorov-Smirnov test:
var ksTest = new OneSampleKolmogorovSmirnovTest(logNormalSample, weibull);

// We can obtan the value of the test statistic through the Statistic property,
// and the corresponding P-value through the Probability property:
Console.WriteLine($"Test statistic: {ksTest.Statistic:F4}");
Console.WriteLine($"P-value:        {ksTest.PValue:F4}");

// We can now print the test results:
Console.WriteLine("Reject null hypothesis? {0}",
    ksTest.Reject() ? "yes" : "no");

//
// Two-sample Kolmogorov-Smirnov Test
//

Console.WriteLine("\nTwo-sample Kolmogorov-Smirnov Test");

// We once again investigate the similarity between a lognormal and
// a Weibull distribution. However, this time, we use 25 random
// samples from each distribution.

// We already have the lognormal samples.
// Generate the samples from the Weibull distribution:
var weibullSample = weibull.Sample(25);

// Finally, we construct the Kolmogorov-Smirnov test:
var ksTest2 = new TwoSampleKolmogorovSmirnovTest(logNormalSample, weibullSample);

// We can obtan the value of the test statistic through the Statistic property,
// and the corresponding P-value through the Probability property:
Console.WriteLine($"Test statistic: {ksTest2.Statistic:F4}");
Console.WriteLine($"P-value:        {ksTest2.PValue:F4}");

// We can now print the test results:
Console.WriteLine("Reject null hypothesis? {0}",
    ksTest2.Reject() ? "yes" : "no");

//
// Anderson-Darling Test
//

Console.WriteLine("\nAnderson-Darling Test");

// The Anderson-Darling is defined for a small number of
// distributions. Currently, only the normal distribution
// is supported.

// We will investigate the distribution of the strength
// of polished airplane windows. The data comes from
// Fuller, e.al. (NIST, 1993) and represents the pressure
// (in psi).

// First, create a numerical variable:
var strength = Vector.Create(new double[]
    {18.830, 20.800, 21.657, 23.030, 23.230, 24.050,
        24.321, 25.500, 25.520, 25.800, 26.690, 26.770,
        26.780, 27.050, 27.670, 29.900, 31.110, 33.200,
        33.730, 33.760, 33.890, 34.760, 35.750, 35.910,
        36.980, 37.080, 37.090, 39.580, 44.045, 45.290,
        45.381});

// Let's print some summary statistics:
Console.WriteLine($"Number of observations: {strength.Length}");
Console.WriteLine($"Mean:                   {strength.Mean():F3}");
Console.WriteLine($"Standard deviation:     {strength.StandardDeviation():F3}");

// The most refined test of normality is the Anderson-Darling test.
AndersonDarlingTest adTest = new AndersonDarlingTest(strength);

// We can obtan the value of the test statistic through the Statistic property,
// and the corresponding P-value through the Probability property:
Console.WriteLine($"Test statistic: {adTest.Statistic:F4}");
Console.WriteLine($"P-value:        {adTest.PValue:F4}");

// We can now print the test results:
Console.WriteLine("Reject null hypothesis? {0}",
    adTest.Reject() ? "yes" : "no");

