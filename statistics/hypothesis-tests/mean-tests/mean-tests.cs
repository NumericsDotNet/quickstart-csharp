//=====================================================================
//
//  File: mean-tests.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

using Numerics.NET;
using Numerics.NET.Statistics;
using Numerics.NET.Statistics.Tests;

// Demonstrates how to use hypothesis tests for the mean
// of one or two distributions.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// This QuickStart Sample uses the scores obtained by the students
// in two groups of students on a national test.
//
// We want to know if the scores for these two groups of students
// are significantly different from the national average, and
// from each other.

// The mean and standard deviation of the complete population:
double nationalMean = 79.3;
double nationalStandardDeviation = 7.3;

Console.WriteLine("Tests for group 1");

    // First we create a NumericalVariable that holds the test scores.
var group1Results = Vector.Create(new double[] {
    62, 77, 61, 94, 75, 82, 86, 83, 64, 84,
    68, 82, 72, 71, 85, 66, 61, 79, 81, 73
});

// We can get the mean and standard deviation of the group right away:
Console.WriteLine($"Mean for the group: {group1Results.Mean():F1}");
Console.WriteLine($"Standard deviation: {group1Results.StandardDeviation():F1}");

//
// One Sample z-test
//

Console.WriteLine("\nUsing z-test:");
// We know the population standard deviation, so we can use the z-test,
// implemented by the OneSampleZTest group. We pass the sample variable
// and the population parameters to the constructor.
OneSampleZTest zTest = new OneSampleZTest(group1Results, nationalMean, nationalStandardDeviation);
// We can obtan the value of the test statistic through the Statistic property,
// and the corresponding P-value through the Probability property:
Console.WriteLine($"Test statistic: {zTest.Statistic:F4}");
Console.WriteLine($"P-value:        {zTest.PValue:F4}");

// The significance level is the default value of 0.05:
Console.WriteLine($"Significance level:     {zTest.SignificanceLevel:F2}");
// We can now print the test scores:
Console.WriteLine($"Reject null hypothesis? {(zTest.Reject() ? "yes" : "no")}");
// We can get a confidence interval for the current significance level:
Interval meanInterval = zTest.GetConfidenceInterval();
Console.WriteLine("95% Confidence interval for the mean: {0:F1} - {1:F1}",
    meanInterval.LowerBound, meanInterval.UpperBound);

// We can get the same scores for the 0.01 significance level by explicitly
// passing the significance level as a parameter to these methods:
Console.WriteLine($"Significance level:     {0.01:F2}");
Console.WriteLine($"Reject null hypothesis? {(zTest.Reject(0.01) ? "yes" : "no")}");
// The GetConfidenceInterval method needs the confidence level, which equals
// 1 - the significance level:
meanInterval = zTest.GetConfidenceInterval(0.99);
Console.WriteLine("99% Confidence interval for the mean: {0:F1} - {1:F1}",
    meanInterval.LowerBound, meanInterval.UpperBound);

//
// One sample t-test
//

Console.WriteLine("\nUsing t-test:");
// Suppose we only know the mean of the national scores,
// not the standard deviation. In this case, a t-test is
// the appropriate test to use.
OneSampleTTest tTest = new OneSampleTTest(group1Results, nationalMean);
// We can obtan the value of the test statistic through the Statistic property,
// and the corresponding P-value through the Probability property:
Console.WriteLine($"Test statistic: {tTest.Statistic:F4}");
Console.WriteLine($"P-value:        {tTest.PValue:F4}");

// The significance level is the default value of 0.05:
Console.WriteLine($"Significance level:     {tTest.SignificanceLevel:F2}");
// We can now print the test scores:
Console.WriteLine($"Reject null hypothesis? {(tTest.Reject() ? "yes" : "no")}");
// We can get a confidence interval for the current significance level:
meanInterval = tTest.GetConfidenceInterval();
Console.WriteLine("95% Confidence interval for the mean: {0:F1} - {1:F1}",
    meanInterval.LowerBound, meanInterval.UpperBound);


//
// Two sample t-test
//

Console.WriteLine("\nUsing two-sample t-test:");
// We want to compare the scores of the first group to the scores
// of a second group from the same school. Once again, we start
// by creating a NumericalVariable containing the scores:
var group2Results = Vector.Create(new double[] {
    61, 80, 98, 90, 94, 65, 79, 75, 74, 86,
    76, 85, 78, 72, 76, 79, 65, 92, 76, 80
});

// To compare the means of the two groups, we need the two sample
// t test, implemented by the TwoSampleTTest group:
var tTest2 = new TwoSampleTTest(group1Results, group2Results, SamplePairing.Paired, false);
// We can obtan the value of the test statistic through the Statistic property,
// and the corresponding P-value through the Probability property:
Console.WriteLine($"Test statistic: {tTest2.Statistic:F4}");
Console.WriteLine($"P-value:        {tTest2.PValue:F4}");

// The significance level is the default value of 0.05:
Console.WriteLine($"Significance level:     {tTest2.SignificanceLevel:F2}");
// We can now print the test scores:
Console.WriteLine($"Reject null hypothesis? {(tTest2.Reject() ? "yes" : "no")}");

