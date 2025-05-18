//=====================================================================
//
//  File: sorting-and-filtering.cs
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
using Index = Numerics.NET.DataAnalysis.Index;
using Range = Numerics.NET.Range;

// Illustrates sorting and filtering of data frames.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// We load the data into a data frame with a DateTime row index:
var timeSeries = DelimitedTextFile.ReadDataFrame<DateTime>(
    @"..\..\..\..\Data\MicrosoftStock.csv", "Date");
var date = timeSeries.RowIndex;

// The following are all equivalent ways of getting
// a strongly typed vector from a data frame:
var open = timeSeries["Open"].As<double>();
var close = timeSeries.GetColumn("Close");
var high = timeSeries.GetColumn<double>("High");
var low = (Vector<double>)timeSeries["Low"];

var volume = timeSeries["Volume"].As<double>();

// Let's print some basic statistics for the full data set:
Console.WriteLine($"Total # observations: {timeSeries.RowCount}");
Console.WriteLine($"Average volume: {volume.Mean():F0}");
Console.WriteLine($"Total volume: {volume.Sum():F0}");

//
// Filtering
//

// Use the GetRows method to select subsets of rows.

// You can use a sequence of keys:
var subset = timeSeries.GetRows(new[] {
        new DateTime(2000,3,1), new DateTime(2000,3,2) });

// When the index is sorted, you can use a range:
subset = timeSeries.GetRows(
    new DateTime(2000, 1, 1), new DateTime(2010, 1, 1));

// Another option is to use a boolean mask. Here we select
// observations where the close price was greater
// than the open price:
var filter = Vector.GreaterThan(close, open);
// Then we can use the GetRows method:
subset = timeSeries.GetRows(filter);
// Data is now filtered:
Console.WriteLine($"Filtered # observations: {subset.RowCount}");

// Masks can be combined using logical operations:
var volumeFilter = volume.Map(x => 200e+6 <= x && x < 300e+6);
Console.WriteLine($"Volume filtered #: {volumeFilter.CountTrue()}");
var intersection = Vector.And(volumeFilter, filter);
var union = Vector.Or(volumeFilter, filter);
var negation = Vector.Not(filter);

Console.WriteLine($"Combined filtered #: {intersection.CountTrue()}");
subset = timeSeries.GetRows(intersection);

// When the row index is ordered, it is possible
// to get the rows with the key nearest to the
// supplied keys:
var startDate = new DateTime(2001, 1, 1, 3, 0, 0);
var offsetDates = Index.CreateDateRange(startDate,
    100, Recurrence.Daily);
subset = timeSeries.GetNearestRows(offsetDates, Direction.Forward);

//
// Sorting
//

// The simplest way to sort data is calling the Sort method
// with the name of the variable to sort on:
var sortedSeries = timeSeries.SortBy("High", SortOrder.Descending);
var sortedHigh = sortedSeries.GetColumn("High")[new Range(0, 4)];
Console.WriteLine("Largest 'High' values:");
Console.WriteLine(sortedHigh.ToString("F2"));

// If you just want the largest few items in a series,
// you can use the Top Or Bottom method
Console.WriteLine(high.Top(5).ToString("F2"));

