//=====================================================================
//
//  File: logistic-regression.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

using Numerics.NET.Data.Text;
using Numerics.NET.Statistics;
using Index = Numerics.NET.DataAnalysis.Index;


// Illustrates building logistic regression models using
// the LogisticRegressionModel class in the
// Numerics.NET.Statistics namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");
// Logistic regression can be performed using
// the LogisticRegressionModel class.
//
// This QuickStart sample uses data from a study of factors
// that determine low birth weight at Baystate Medical Center.
// from Belsley, Kuh and Welsch. The fields are as follows:
//   AGE:  Mother's age.
//   LWT:  Mother's weight.
//   RACE: 1=white, 2=black, 3=other.
//   FVT:  Number of physician visits during the 1st trimester.
//   LOW:  Low birth weight indicator.

// First, read the data from a file into an ADO.NET DataTable.
var data = FixedWidthTextFile.ReadDataFrame(
    @"..\..\..\..\..\Data\lowbwt.txt",
    new int[] { 4, 11, 18, 25, 33, 42, 49, 55, 61, 68 });

// Now create the regression model. Parameters are the name
// of the dependent variable, a string array containing
// the names of the independent variables, and the data frame
// containing all variables.

// Categorical variables are automatically expanded into
// indicator variables if they are marked properly:
data.MakeCategorical("RACE", Index.Create(new[] { 1, 2, 3 }));

var model = new LogisticRegressionModel(data, "LOW",
    new string[] { "AGE", "LWT", "RACE", "FTV" });

// Alternatively, we can use a formula to describe the variables
// in the model. The dependent variable goes on the left, the
// independent variables on the right of the ~:
model = new LogisticRegressionModel(data, "LOW ~ AGE + LWT + RACE + FTV");

// The Fit method performs the actual regression analysis.
model.Fit();

// The Parameters collection contains information about the regression
// parameters.
Console.WriteLine("Variable              Value    Std.Error  t-stat  p-Value");
foreach (var parameter in model.Parameters)
    // Parameter objects have the following properties:
    Console.WriteLine("{0,-20}{1,10:F5}{2,10:F5}{3,8:F2} {4,7:F4}",
        // Name, usually the name of the variable:
        parameter.Name,
        // Estimated value of the parameter:
        parameter.Value,
        // Standard error:
        parameter.StandardError,
        // The value of the t statistic for the hypothesis that the parameter
        // is zero.
        parameter.Statistic,
        // Probability corresponding to the t statistic.
        parameter.PValue);

// The log-likelihood of the computed solution is also available:
Console.WriteLine($"Log-likelihood: {model.LogLikelihood:F4}");

// We can test the significance by looking at the results
// of a log-likelihood test, which compares the model to
// a constant-only model:
Numerics.NET.Statistics.Tests.SimpleHypothesisTest lrt = model.GetLikelihoodRatioTest();
Console.WriteLine("Likelihood-ratio test: chi-squared={0:F4}, p={1:F4}", lrt.Statistic, lrt.PValue);
Console.WriteLine();

// We can compute a model with fewer parameters:
var model2 = new LogisticRegressionModel(data, "LOW ~ LWT + RACE");
model2.Fit();

// Print the results...
Console.WriteLine("Variable              Value    Std.Error  t-stat  p-Value");
foreach (var parameter in model2.Parameters)
    Console.WriteLine("{0,-20}{1,10:F5}{2,10:F5}{3,8:F2} {4,7:F4}",
        parameter.Name, parameter.Value, parameter.StandardError, parameter.Statistic, parameter.PValue);
// ...including the log-likelihood:
Console.WriteLine($"Log-likelihood: {model2.LogLikelihood:F4}");

// We can now compare the original model to this one, once again
// using the likelihood ratio test:
lrt = model.GetLikelihoodRatioTest(model2);
Console.WriteLine("Likelihood-ratio test: chi-squared={0:F4}, p={1:F4}", lrt.Statistic, lrt.PValue);
Console.WriteLine();

//
// Multinomial (polytopous) logistic regression
//

// The LogisticRegressionModel class can also be used
// for logistic regression with more than 2 responses.
// The following example is from "Applied Linear Statistical
// Models."

// Load the data into a matrix
string[] columnNames = { "id", "duration", "x2", "x3", "x4",
    "nutritio", "agecat1", "agecat3", "alcohol", "smoking" };
var dataFrame = FixedWidthTextFile.ReadDataFrame(
    @"..\..\..\..\..\Data\mlogit.txt",
    new FixedWidthTextOptions(
        new int[] { 5, 10, 15, 20, 25, 32, 37, 42, 47 },
        columnHeaders: false))
    .WithColumnIndex(columnNames);

// For multinomial regression, the response variable must be
// a categorical variable:
dataFrame.MakeCategorical("duration");

// The constructor takes an extra argument of type
// LogisticRegressionMethod:
var model3 = new LogisticRegressionModel(
    dataFrame, "duration",
    new string[] { "nutritio", "agecat1", "agecat3", "alcohol", "smoking" });
model3.Method = LogisticRegressionMethod.Nominal;

// When using a formula, we can use '.' as a shortcut
// for all unused variables in the data frame.
// Because duration has 3 levels, nominal logistic regression
// is automatically inferred.
model3 = new LogisticRegressionModel(dataFrame,
    "duration ~ nutritio + agecat1 + agecat3 + alcohol + smoking");

// Everything else is the same:
model3.Fit();

// There is a set of parameters for each level of the
// response variable. The highest level is the reference
// level and has no associated parameters.
foreach (var p in model3.Parameters) {
    Console.WriteLine(p.ToString());
}

Console.WriteLine($"Log likelihood: {model3.LogLikelihood:F4}");

// To test the hypothesis that all the slopes are zero,
// use the GetLikelihoodRatioTest method.
lrt = model3.GetLikelihoodRatioTest();
Console.WriteLine("Test that all slopes are zero: chi-squared={0:F4}, p={1:F4}", lrt.Statistic, lrt.PValue);

