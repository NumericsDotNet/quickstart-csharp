//=====================================================================
//
//  File: factor-analysis.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

using Numerics.NET.Data.Stata;
using Numerics.NET;
using Numerics.NET.Statistics.Multivariate;

namespace Numerics.NET.Quickstart.CSharp
{
    /// <summary>
    /// Demonstrates how to use classes that implement
    /// Factor Analysis.
    /// </summary>
    class FactorAnalysisExample
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // This QuickStart Sample demonstrates how to perform
            // a factor analysis on a set of data.
            //
            // The classes used in this sample reside in the
            // Numerics.NET.Statistics.Multivariate namespace.

            // First, our dataset, 'm255.dta', from Professor James Sidanius.
            //     See http://www.ats.ucla.edu/stat/sas/output/factor.htm

            // Note: tolerances used to test for convergence in factor analysis
            // algorithms are usually set very low (around 0.001). As a result,
            // when comparing results from different programs, usually only
            // about the first 3 digits will be equal.

            // The data is in Stata format. Use a matrix reader to load it into a matrix.
            var frame = StataFile.ReadDataFrame(@"..\..\..\..\..\Data\m255.dta");
            // We'll use only these columns:
            string[] names = { "item13", "item14", "item15", "item16",
                    "item17", "item18", "item19", "item20", "item21",
                    "item22", "item23", "item24" };
            // First, filter out any rows with missing values:
            frame = frame.RemoveRowsWithMissingValues(names);

            //
            // Factor analysis
            //

            // We can construct FA objects in many ways. Since we have the data in a matrix,
            // we use the constructor that takes a data matrix as input.
            var fa = new FactorAnalysis(frame, names);

            // Alternatively, we could have used a formula to specify
            // the variables we want in the model:
            fa = new FactorAnalysis(frame, string.Join(" + ", names));

            // We set the number of factors:
            fa.NumberOfFactors = 3;
            // and immediately perform the analysis:
            fa.Fit();

            // We can get the unrotated factors:
            var unrotatedFactors = fa.GetUnrotatedFactors();
            // We can get the contributions of each factor:
            Console.WriteLine(" #    Eigenvalue Difference Contribution Contrib. %");
            foreach (Factor factor in unrotatedFactors)
            {
                // and write out its properties
                Console.WriteLine("{0,2}{1,12:F4}{2,11:F4}{3,14:F3}{4,10:F3}",
                    factor.Index, factor.Eigenvalue, factor.EigenvalueDifference,
                    factor.ProportionOfVariance,
                    factor.CumulativeProportionOfVariance);
            }

            Console.WriteLine("\nVarimax rotation");

            // Here are the loadings for each of the variables:
            Console.WriteLine("\nUnrotated loadings:");
            Console.WriteLine("Variable        1          2          3      Uniqueness");
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine("  {0,8}{1,10:F5} {2,10:F5} {3,10:F5}{4,10:F5}",
                    names[i],
                    unrotatedFactors[0].Loadings[i],
                    unrotatedFactors[1].Loadings[i],
                    unrotatedFactors[2].Loadings[i],
                    fa.Uniqueness[i]);
            }

            // Now we'll look at the rotated factors:
            var rotatedFactors = fa.GetRotatedFactors();
            Console.WriteLine(" #    Variance   Difference Proportion   Cumulative");
            foreach (Factor factor in rotatedFactors)
            {
                Console.WriteLine("{0,2}{1,12:F4}{2,11:F4}{3,13:F4}{4,11:F4}",
                    factor.Index, factor.VarianceExplained, "-",
                    factor.ProportionOfVariance,
                    factor.CumulativeProportionOfVariance);
            }

            // Here are the rotated loadings for each of the variables:
            Console.WriteLine("\nRotated loadings (Varimax):");
            Console.WriteLine("Variable        1          2          3      Uniqueness");
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine("  {0,8}{1,10:F5} {2,10:F5} {3,10:F5}{4,10:F5}",
                    names[i],
                    rotatedFactors[0].Loadings[i],
                    rotatedFactors[1].Loadings[i],
                    rotatedFactors[2].Loadings[i],
                    fa.Uniqueness[i]);
            }

            // And the matrix that rotates the factors
            Console.WriteLine("Factor transformation matrix:\n{0:F4}",
                fa.FactorTransformationMatrix);

            Console.WriteLine("\nPromax rotation (power = 3)");

            // Now let's use an (oblique) Promax rotation:
            fa.RotationMethod = FactorRotationMethod.Promax;
            fa.PromaxPower = 3;
            fa.Fit();

            // Now we'll look at the rotated factors:
            Console.WriteLine("\nRotated factor variance explained:");
            rotatedFactors = fa.GetRotatedFactors();
            Console.WriteLine(" #    Variance");
            foreach (Factor factor in rotatedFactors)
            {
                Console.WriteLine("{0,2}{1,12:F4}",
                    factor.Index, factor.VarianceExplained);
            }

            // Here are the rotated loadings for each of the variables:
            Console.WriteLine("\nRotated loadings/pattern (Promax):");
            Console.WriteLine("Variable        1          2          3   Communality Uniqueness");
            for (int i = 0; i < names.Length; i++)
            {
                // and write out its properties
                Console.WriteLine("  {0,8}{1,10:F5}{2,10:F5}{3,10:F5}{4,10:F5} {5,10:F5}",
                    names[i],
                    rotatedFactors[0].Loadings[i],
                    rotatedFactors[1].Loadings[i],
                    rotatedFactors[2].Loadings[i],
                    fa.Communalities[i],
                    fa.Uniqueness[i]);
            }

            // Here are the rotated loadings for each of the variables:
            Console.WriteLine("\nRotated factor structure:");
            Console.WriteLine("Variable        1          2          3");
            for (int i = 0; i < names.Length; i++)
            {
                // and write out its properties
                Console.WriteLine("  {0,8}{1,10:F5} {2,10:F5} {3,10:F5}",
                    names[i],
                    rotatedFactors[0].Structure[i],
                    rotatedFactors[1].Structure[i],
                    rotatedFactors[2].Structure[i]);
            }

            // For oblique rotations, the factors are usually correlated:
            Console.WriteLine("Factor correlation matrix:\n{0:F4}",
                fa.FactorCorrelationMatrix);

            //
            // Factor analysis on a correlation matrix
            //

            Console.WriteLine("\nUsing a correlation matrix");

            // This example is from Exploratory Factor Analysis
            // http://www.oup.com/us/companion.websites/9780199734177/supplementary/example/
            double[] values = {
                1.000, 0.666, 0.150, 0.617, 0.541, 0.653, 0.473, 0.549, 0.566,
                0.666, 1.000, 0.247, 0.576, 0.510, 0.642, 0.425, 0.544, 0.488,
                0.150, 0.247, 1.000, 0.222, 0.081, 0.164, 0.091, 0.181, 0.120,
                0.617, 0.576, 0.222, 1.000, 0.409, 0.560, 0.338, 0.448, 0.349,
                0.541, 0.510, 0.081, 0.409, 1.000, 0.667, 0.734, 0.465, 0.754,
                0.653, 0.642, 0.164, 0.560, 0.667, 1.000, 0.596, 0.540, 0.672,
                0.473, 0.425, 0.091, 0.338, 0.734, 0.596, 1.000, 0.432, 0.718,
                0.549, 0.544, 0.181, 0.448, 0.465, 0.540, 0.432, 1.000, 0.412,
                0.566, 0.488, 0.120, 0.349, 0.754, 0.672, 0.718, 0.412, 1.000
            };
            var R = Matrix.CreateSymmetric(9, values, MatrixTriangle.Upper,
                MatrixElementOrder.ColumnMajor, true);
            fa = new FactorAnalysis(R, FactorMethod.Correlation);
            fa.NumberOfFactors = 2;
            fa.ExtractionMethod = FactorExtractionMethod.MaximumLikelihood;
            fa.RotationMethod = FactorRotationMethod.Varimax;
            fa.Fit();

            names = new string[] { "Hugs", "Comps", "PerAd", "SocAd", "ProAd",
                "ComSt", "PhyHlp", "Encour", "Tutor" };

            // Here are the initial:
            Console.WriteLine("\nRotated factor loadings:");
            Console.WriteLine("Variable     Initial    Extracted");
            for (int i = 0; i < names.Length; i++)
            {
                // and write out its properties
                Console.WriteLine("  {0,8}{1,10:F5} {2,10:F5}",
                    names[i],
                    fa.InitialCommunalities[i],
                    fa.Communalities[i]);
            }

            // Here are the rotated loadings for each of the variables:
            // Note that in the SPSS output, the ordering of the variables
            // is different.
            unrotatedFactors = fa.GetUnrotatedFactors();
            Console.WriteLine("\nUnrotated factor loadings:");
            Console.WriteLine("Variable        1          2");
            for (int i = 0; i < names.Length; i++)
            {
                // and write out its properties
                Console.WriteLine("  {0,8}{1,10:F5} {2,10:F5}",
                    names[i],
                    unrotatedFactors[0].Loadings[i],
                    unrotatedFactors[1].Loadings[i]);
            }

            // Here are the rotated loadings for each of the variables:
            rotatedFactors = fa.GetRotatedFactors();
            Console.WriteLine("\nRotated factor loadings:");
            Console.WriteLine("Variable        1          2");
            for (int i = 0; i < names.Length; i++)
            {
                // and write out its properties
                Console.WriteLine("  {0,8}{1,10:F5} {2,10:F5}",
                    names[i],
                    rotatedFactors[0].Loadings[i],
                    rotatedFactors[1].Loadings[i]);
            }

        }
    }
}
