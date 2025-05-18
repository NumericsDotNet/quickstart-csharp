//=====================================================================
//
//  File: anova-repeated-measures.cs
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
using Numerics.NET.Statistics;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates the use of the OneWayRAnovaModel class for performing
    /// a one-way analysis of variance with repeated measures.
    /// </summary>
    class AnovaRepeatedMeasures
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // This QuickStart Sample investigates the effect of the color of packages
            // on the sales of the product. The data comes from 12 stores.
            // Packages can be either red, green or blue.

            // Set up the data as anonymous records:
            var data = new[] {
                new { Person = 1, Drug = 1, Score = 30 },
                new { Person = 1, Drug = 2, Score = 28 },
                new { Person = 1, Drug = 3, Score = 16 },
                new { Person = 1, Drug = 4, Score = 34 },
                new { Person = 2, Drug = 1, Score = 14 },
                new { Person = 2, Drug = 2, Score = 18 },
                new { Person = 2, Drug = 3, Score = 10 },
                new { Person = 2, Drug = 4, Score = 22 },
                new { Person = 3, Drug = 1, Score = 24 },
                new { Person = 3, Drug = 2, Score = 20 },
                new { Person = 3, Drug = 3, Score = 18 },
                new { Person = 3, Drug = 4, Score = 30 },
                new { Person = 4, Drug = 1, Score = 38 },
                new { Person = 4, Drug = 2, Score = 34 },
                new { Person = 4, Drug = 3, Score = 20 },
                new { Person = 4, Drug = 4, Score = 44 },
                new { Person = 5, Drug = 1, Score = 26 },
                new { Person = 5, Drug = 2, Score = 28 },
                new { Person = 5, Drug = 3, Score = 14 },
                new { Person = 5, Drug = 4, Score = 30 }
            };
            var dataFrame = DataFrame.FromObjects(data);

            // Construct the OneWayAnova object.
            OneWayRAnovaModel anova = new OneWayRAnovaModel(dataFrame, "Score", "Drug", "Person");
            // Alternatively, we can use a formula to specify the variables
            // in the model:
            anova = new OneWayRAnovaModel(dataFrame, "Score ~ Drug + Person");
            // Perform the calculation.
            anova.Fit();

            // Verify that the design is balanced:
            if (!anova.IsBalanced)
                Console.WriteLine("The design is not balanced.");

            // The AnovaTable property gives us a classic anova table.
            // We can write the table directly to the console:
            Console.WriteLine(anova.AnovaTable.ToString());
            Console.WriteLine();

            // A Cell object represents the data in a cell of the model,
            // i.e. the data related to one level of the factor.
            // We can use it to access the group means for each drug.

            // We need two indices here: the second index corresponds
            // to the person factor.

            // First we get the index so we can easily iterate
            // through the levels:
            var drugFactor = (Index<int>)anova.TreatmentFactor;
            foreach(int level in drugFactor)
                Console.WriteLine("Mean for group '{0}': {1:F4}",
                    level, anova.SubjectTotals.Get(level).Mean);

            // We could have accessed the cells directly as well:
            Console.WriteLine("Variance for second drug: {0}",
                anova.TreatmentTotals.Get(2).Variance);
            Console.WriteLine();

            // We can get the summary data for the entire model
            // from the TotalCell property:
            Cell totalSummary = anova.TotalCell;
            Console.WriteLine("Summary data:");
            Console.WriteLine($"# observations: {totalSummary.Count}");
            Console.WriteLine($"Grand mean:     {totalSummary.Mean:F4}");

        }
    }
}
