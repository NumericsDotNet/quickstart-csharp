//=====================================================================
//
//  File: anova-one-way.cs
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

// Illustrates the use of the OneWayAnovaModel class for performing
// a one-way analysis of variance.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// This QuickStart Sample investigates the effect of the color of packages
// on the sales of the product. The data comes from 12 stores.
// Packages can be either red, green or blue.

// Set up the data as anonymous records:
var values = new[] {
    new { Store = 1, Color = "Blue", Shape = "Square", Sales = 6 },
    new { Store = 2, Color = "Blue", Shape = "Square", Sales = 14 },
    new { Store = 3, Color = "Blue", Shape = "Rectangle", Sales = 19 },
    new { Store = 4, Color = "Blue", Shape = "Rectangle", Sales = 17 },

    new { Store = 5, Color = "Red", Shape = "Square", Sales = 18 },
    new { Store = 6, Color = "Red", Shape = "Square", Sales = 11 },
    new { Store = 7, Color = "Red", Shape = "Rectangle", Sales = 20 },
    new { Store = 8, Color = "Red", Shape = "Rectangle", Sales = 23 },

    new { Store = 9, Color = "Green", Shape = "Square", Sales = 7 },
    new { Store = 10, Color = "Green", Shape = "Square", Sales = 11 },
    new { Store = 11, Color = "Green", Shape = "Rectangle", Sales = 18 },
    new { Store = 12, Color = "Green", Shape = "Rectangle", Sales = 10 },
};
var dataFrame = DataFrame.FromObjects(values);

// Construct the OneWayAnovaModel object.
var anova = new OneWayAnovaModel(dataFrame, "Sales", "Color");
// Alternatively, you can use a formula to specify the variables:
anova = new OneWayAnovaModel(dataFrame, "Sales ~ Color");
// Perform the calculation.
anova.Fit();

// Verify that the design is balanced:
if (!anova.IsBalanced)
    Console.WriteLine("The design is not balanced.");

// The AnovaTable property gives us a classic ANOVA table.
// We can write the table directly to the console:
Console.WriteLine(anova.AnovaTable.ToString());
Console.WriteLine();

// A Cell object represents the data in a cell of the model,
// i.e. the data related to one level of the factor.
// We can use it to access the group means of our color groups.

// First we get the index so we can easily iterate
// through the levels:
var colorFactor = anova.GetFactor<string>(0);
foreach(string level in colorFactor)
    Console.WriteLine("Mean for group '{0}': {1:F4}",
        level, anova.Cells.Get(level).Mean);

// We could have accessed the cells directly as well:
Console.WriteLine("Variance for blue packages: {0}",
    anova.Cells.Get("Blue").Variance);
Console.WriteLine();

// We can get the summary data for the entire model
// from the TotalCell property:
Cell totalSummary = anova.TotalCell;
Console.WriteLine("Summary data:");
Console.WriteLine($"# observations: {totalSummary.Count}");
Console.WriteLine($"Grand mean:     {totalSummary.Mean:F4}");

