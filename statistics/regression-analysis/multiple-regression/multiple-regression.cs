//=====================================================================
//
//  File: multiple-regression.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

using Numerics.NET.DataAnalysis;
using Numerics.NET;
using Numerics.NET.Statistics;
using Numerics.NET.Data.Text;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates building multiple linear regression models using
    /// the LinearRegressionModel class in the
    /// Numerics.NET.Statistics namespace of Numerics.NET.
    /// </summary>
    class MultipleRegression
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // Multiple linear regression can be performed using
            // the LinearRegressionModel class.
            //
            //

            // This QuickStart sample uses data test scores of 200 high school
            // students, including science, math, and reading.

            // First, read the data from a file into a data frame.
            var data = DelimitedTextFile.ReadDataFrame(@"..\..\..\..\..\Data\hsb2.csv");

            // Now create the regression model. Parameters are the data frame,
            // the name of the dependent variable, and a string array containing
            // the names of the independent variables.
            var model = new LinearRegressionModel(data,
                "science", new string[] {"math", "female", "socst", "read"});

            // Alternatively, we can use a formula to describe the variables
            // in the model. The dependent variable goes on the left, the
            // independent variables on the right of the ~:
            var model2 = new LinearRegressionModel(data,
                "science ~ math + female + socst + read");

            // We can set model options now, such as whether to exclude
            // the constant term:
            // model.NoIntercept = false;

            // The Fit method performs the actual regression analysis.
            model.Fit();

            // The Parameters collection contains information about the regression
            // parameters.
            Console.WriteLine("Variable              Value    Std.Error  t-stat  p-Value");
            foreach(var parameter in model.Parameters)
            {
                // Parameter objects have the following properties:
                Console.WriteLine("{0,-20}{1,10:F6}{2,10:F6}{3,8:F2} {4,7:F5}",
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
            }
            Console.WriteLine();

            // In addition to these properties, Parameter objects have
            // a GetConfidenceInterval method that returns
            // a confidence interval at a specified confidence level.
            // Notice that individual parameters can be accessed
            // using their numeric index. Parameter 0 is the intercept,
            // if it was included.
            Interval confidenceInterval = model.Parameters[0].GetConfidenceInterval(0.95);
            Console.WriteLine("95% confidence interval for intercept: {0:F4} - {1:F4}",
                confidenceInterval.LowerBound, confidenceInterval.UpperBound);

            // Parameters can also be accessed by name:
            confidenceInterval = model.Parameters.Get("math").GetConfidenceInterval(0.95);
            Console.WriteLine("95% confidence interval for 'math': {0:F4} - {1:F4}",
                confidenceInterval.LowerBound, confidenceInterval.UpperBound);
            Console.WriteLine();

            // There is also a wealth of information about the analysis available
            // through various properties of the LinearRegressionModel object:
            Console.WriteLine($"Residual standard error: {model.StandardError:F3}");
            Console.WriteLine($"R-Squared:               {model.RSquared:F4}");
            Console.WriteLine($"Adjusted R-Squared:      {model.AdjustedRSquared:F4}");
            Console.WriteLine($"F-statistic:             {model.FStatistic:F4}");
            Console.WriteLine($"Corresponding p-value:   {model.PValue:F5}");
            Console.WriteLine();

            // Much of this data can be summarized in the form of an ANOVA table:
            Console.WriteLine(model.AnovaTable.ToString());

            // All this information can be printed using the Summarize method.
            // You will also see summaries using the library in C# interactive.
            Console.WriteLine(model.Summarize());

        }
    }
}
