//=====================================================================
//
//  File: generalized-linear-models.cs
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
using Numerics.NET.DataAnalysis;
using Numerics.NET;
using Numerics.NET.Statistics;


// Illustrates building generalized linear models using
// the GeneralizedLinearModel class in the
// Numerics.NET.Statistics namespace of Numerics.NET.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// Generalized linear models can be computed using
// the GeneralizedLinearModel class.

//
// Poisson regression
//

// This QuickStart sample uses data about the attendance of 316 students
// from two urban high schools. The fields are as follows:
//   daysabs: The number of days the student was absent.
//   male:    A binary indicator of gender.
//   math:    The student's standardized math score.
//   langarts:The student's standardized language arts score.
//
// We want to investigate the relationship between these variables.
//
// See http://www.ats.ucla.edu/stat/stata/dae/poissonreg.htm

// First, read the data from a file into a VariableCollection.
// The ReadAttendanceData method is defined later in this file.
var data = ReadAttendanceData();

// Now create the regression model. Parameters are the name
// of the dependent variable, a string array containing
// the names of the independent variables, and the VariableCollection
// containing all variables.
var model = new GeneralizedLinearModel(data,
    "daysabs", new string[] { "math", "langarts", "male" });

model = new GeneralizedLinearModel(data,
    "daysabs ~ math + langarts + male");

// The ModelFamily specifies the distribution of the dependent variable.
// Since we're dealing with count data, we use a Poisson model:
model.ModelFamily = ModelFamily.Poisson;

// The LinkFunction specifies the relationship between the dependent variable
// and the linear predictor of independent variables. In this case,
// we use the canonical link function, which is the default.
model.LinkFunction = ModelFamily.Poisson.CanonicalLinkFunction;

// The Fit method performs the actual regression analysis.
model.Fit();

// The Parameters collection contains information about the regression
// parameters.
Console.WriteLine("Variable              Value    Std.Error    z     p-Value");
foreach (var parameter in model.Parameters)
{
    // Parameter objects have the following properties:
    Console.WriteLine("{0,-20}{1,10:F6}{2,10:F6}{3,8:F2} {4,7:F5}",
        // Name, usually the name of the variable:
        parameter.Name,
        // Estimated value of the parameter:
        parameter.Value,
        // Standard error:
        parameter.StandardError,
        // The value of the z score for the hypothesis that the parameter
        // is zero.
        parameter.Statistic,
        // Probability corresponding to the t statistic.
        parameter.PValue);
}
Console.WriteLine();

// In addition to these properties, Parameter objects have a GetConfidenceInterval
// method that returns a confidence interval at a specified confidence level.
// Notice that individual parameters can be accessed using their numeric index.
// Parameter 0 is the intercept, if it was included.
Interval confidenceInterval = model.Parameters[0].GetConfidenceInterval(0.95);
Console.WriteLine("95% confidence interval for math score: {0:F4} - {1:F4}",
    confidenceInterval.LowerBound, confidenceInterval.UpperBound);

// Parameters can also be accessed by name:
confidenceInterval = model.Parameters.Get("math").GetConfidenceInterval(0.95);
Console.WriteLine("95% confidence interval for math score: {0:F4} - {1:F4}",
    confidenceInterval.LowerBound, confidenceInterval.UpperBound);
Console.WriteLine();

// There is also a wealth of information about the analysis available
// through various properties of the GeneralizedLinearModel object:
Console.WriteLine($"Log likelihood:         {model.LogLikelihood:F4}");
Console.WriteLine($"Kernel log likelihood:  {model.GetKernelLogLikelihood():F4}");

// Note that some statistical applications (notably stata) use
// a different definition of some of these "information criteria":
Console.WriteLine("\"Information Criteria\"");
Console.WriteLine($"Akaike (AIC):           {model.GetAkaikeInformationCriterion():F3}");
Console.WriteLine($"Corrected AIC:          {model.GetCorrectedAkaikeInformationCriterion():F3}");
Console.WriteLine($"Bayesian (BIC):         {model.GetBayesianInformationCriterion():F3}");
Console.WriteLine($"Chi Square:             {model.GetChiSquare():F3}");
Console.WriteLine();

//
// Probit regression
//

// In a second example, we investigate the relationship between whether a student
// graduates, and the student's GRE scores,grade point averages, the level
// of the school from a "top notch" school. The fields are as follows:
//   admit:    Dependent variable
//   gre:      The student's GRE score.
//   topnotch: A binary indicator of the type of school
//   gpa:      The student's Grade Point Average.
//
// The data was generated.
// See http://www.ats.ucla.edu/stat/stata/dae/probit.htm

// First, read the data from a file into a VariableCollection.
// The ReadGraduateData method is defined later in this file.
data = ReadGraduateData();

// Now create the regression model. Parameters are the name
// of the dependent variable, a string array containing
// the names of the independent variables, and the VariableCollection
// containing all variables.
model = new GeneralizedLinearModel(data,
    "admit", new string[] { "gre", "topnotch", "gpa" });

// The ModelFamily specifies the distribution of the dependent variable.
// Since we're dealing with binary data, we use a Binomial model:
model.ModelFamily = ModelFamily.Binomial;

// We use the probit link function.
model.LinkFunction = LinkFunction.Probit;

// The Fit method performs the actual regression analysis.
model.Fit();

// The Parameters collection contains information about the regression
// parameters.
Console.WriteLine("Variable              Value    Std.Error    z     p-Value");
foreach (var parameter in model.Parameters)
{
    Console.WriteLine("{0,-20}{1,10:F6}{2,10:F6}{3,8:F2} {4,7:F5}",
        parameter.Name,
        parameter.Value,
        parameter.StandardError,
        parameter.Statistic,
        parameter.PValue);
}
Console.WriteLine();

// There is also a wealth of information about the analysis available
// through various properties of the GeneralizedLinearModel object:
Console.WriteLine($"Log likelihood:         {model.LogLikelihood:F4}");
Console.WriteLine($"Kernel log likelihood:  {model.GetKernelLogLikelihood():F4}");

// Note that some statistical applications (notably stata) use
// a different definition of some of these "information criteria":
Console.WriteLine("\"Information Criteria\"");
Console.WriteLine($"Akaike (AIC):           {model.GetAkaikeInformationCriterion():F3}");
Console.WriteLine($"Corrected AIC:          {model.GetCorrectedAkaikeInformationCriterion():F3}");
Console.WriteLine($"Bayesian (BIC):         {model.GetBayesianInformationCriterion():F3}");
Console.WriteLine($"Chi Square:             {model.GetChiSquare():F3}");
Console.WriteLine();


static DataFrame<long, string> ReadAttendanceData()
{
    return DelimitedTextFile.ReadDataFrame(@"..\..\..\..\..\data\PoissonReg.csv");
}

static DataFrame<long, string> ReadGraduateData()
{
    var df = FixedWidthTextFile.ReadDataFrame(@"..\..\..\..\..\data\probit.dat",
        new FixedWidthTextOptions(new int[] { 9, 18, 27 }, columnHeaders:false));
    var columnNames = new string[] { "admit", "gre", "topnotch", "gpa" };
    return df.WithColumnIndex(columnNames);
}
