//=====================================================================
//
//  File: manipulating-columns.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
using System.Collections.Generic;

using Numerics.NET;
using Numerics.NET.DataAnalysis;

using Index = Numerics.NET.DataAnalysis.Index;

// Illustrates how to transform and manipulate the columns
// of a data frame.


// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// Let's start with a data frame with a DateTime index:
int rowCount = 1000;
var dates = Index.CreateDateRange(new DateTime(2016, 01, 17), rowCount, Recurrence.Daily);
var frame = DataFrame.FromColumns(new Dictionary<string, object>() {
        { "values1", Vector.CreateRandom(rowCount) },
        { "values2", Vector.CreateRandom(rowCount) },
        }, dates);
Console.WriteLine(frame.Head());

// The columns of a data frame are immutable,
// but the collection of columns is not.

// We can add columns:
frame.AddColumn("vzlues3", Vector.CreateRandom(rowCount));
frame.AddColumn("values4", Vector.CreateRandom(rowCount));
frame.AddColumn("values6", Vector.CreateRandom(rowCount));
Console.WriteLine(frame.Head());
// Rename columns:
frame.RenameColumn("values4", "vzlues5");
frame.RenameColumns(s => s.StartsWith("vzlues"), s => "values" + s.Substring(6));
Console.WriteLine(frame.Head());
// And remove columns:
frame.RemoveColumn("values5");
frame.RemoveColumnAt(2);
Console.WriteLine(frame.Head());

// You can transform a column and add the result
// in various places:
// As the last column:
frame.MapAndAppend<double>("values1", x => Vector.Cos(x), "cosValues1");
// After a specific column:
frame.MapAndInsertAfter<double>("values1", x => Vector.Sin(x), "sinValues1");
// Replacing the column
frame.MapAndReplace<double>("values6", x => Vector.Exp(x), "expValues6");
Console.WriteLine(frame.Head());

// The same operations can be performed on multiple columns
// at once:
var columns = new[] { "values1", "values2" };
// We can supply the keys for the new columns explicitly:
var negColumns = new[] { "-values1", "-values2" };
frame.MapAndAppend<double>(columns, x => -x, negColumns);
// or as a function of the original key:
frame.MapAndInsertAfter<double>(columns, x => 2.0 * x, s => "2*" + s);
Console.WriteLine(frame.Head());

// A more complex example: replace missing values
// with the mean of a group.

// We create a categorical variable with 5 categories
// so we will have 5 group means.
var group = frame.GetColumn("values1").Bin(5);
// and a variable that has some missing values:
var withNAs = frame.GetColumn("values2").Clone()
    .SetValues(double.NaN, x => x < 0.15);
// Note that, since columns are immutable, we have to
// make a clone before we can set values.
Console.WriteLine(withNAs.GetSlice(0, 12));

// Now for the actual calculation, which has 3 steps:
// First, we compute the means for each group:
var meansPerGroup = withNAs.AggregateBy(group, Aggregators.Mean);
Console.WriteLine(meansPerGroup);

// Next, create a vector with the means of the group
// that each element belongs to:
var means = group.WithCategories(meansPerGroup);
// Next, we replace the missing values with the corresponding
// elements from that vector.
var withNAsReplaced = withNAs.ReplaceMissingValues(means);
Console.WriteLine(withNAsReplaced.GetSlice(0,12));

//
// Row-based operations
//

// Data frames are column-based data structures.
// Even though it is not recommended, it is possible
// to perform operations on rows:

frame.AddColumn("values3", Vector.CreateRandom(rowCount));
var avg1 = Vector.Create<double>(frame.RowCount);
int i = 0;
foreach (var row in frame.Rows)
{
    avg1[i] = (row.Get<double>("values1")
            + row.Get<double>("values2")
            + row.Get<double>("values3")) / 3;
    i++;
}
frame.AddColumn("Average", avg1);

// Performing the operation directly on the columns
// is much more efficient:
var avg2 = (frame.GetColumn("values1")
         + frame.GetColumn("values2")
         + frame.GetColumn("values3")) / 3.0;
frame.AddColumn("Average2", avg2);

