//=====================================================================
//
//  File: simple-time-series.cs
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

using Numerics.NET.Data.Text;
using Numerics.NET.DataAnalysis;
using Numerics.NET.Statistics;

// Illustrates the use of the TimeSeriesCollection class to represent
// and manipulate time series data.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// Time series data frames can be created in a variety of ways.
// Here we read from a CSV file and specify the column to use as the index:
var timeSeries = DelimitedTextFile.ReadDataFrame<DateTime>(
    @"..\..\..\..\..\Data\MicrosoftStock.csv", "Date");

// The RowCount property returns the number of
// observations:
Console.WriteLine($"# observations: {timeSeries.RowCount}");

//
// Accessing variables
//

// Variables are accessed by name or numeric index.
// They need to be cast to the appropriate specialized
// type using the As() method:
var close = timeSeries["Close"].As<double>();
Console.WriteLine($"Average close price: ${close.Mean():F2}");

// Variables can also be accessed by numeric index:
Console.WriteLine($"3rd variable: {timeSeries[2].Name}");

// The GetRows method returns the data from the specified range.
DateTime y2004 = new DateTime(2004, 1, 1);
DateTime y2005 = new DateTime(2005, 1, 1);
var series2004 = timeSeries.GetRows(y2004, y2005);
Console.WriteLine("Opening price on the first trading day of 2004: {0}",
    series2004["Open"].GetValue(0));

//
// Transforming the Frequency
//

// The first step is to define the aggregator function
// for each variable. This function specifies how each
// observation in the new time series is calculated
// from the observations in the original series.

// The Aggregators class has a number of
// pre-defined aggregator functions.

// We create a dictionary that maps column names
// to aggregators:
var aggregators = new Dictionary<string, AggregatorGroup>()
{
    { "Open", Aggregators.First },
    { "Close", Aggregators.Last },
    { "High", Aggregators.Max },
    { "Low", Aggregators.Min },
    { "Volume", Aggregators.Sum }
};

// We can then resample the data frame in accordance with
// a recurrence pattern we specify, in this case monthly:
var monthlySeries = timeSeries.Resample(Recurrence.Monthly, aggregators);

// We can specify a subset of the series by selecting it
// from the data frame first:
monthlySeries = timeSeries.GetRows(y2004, y2005)
    .Resample(Recurrence.Monthly, aggregators);

// We can now print the results:
Console.WriteLine("Monthly statistics for Microsoft Corp. (MSFT)");
Console.WriteLine(monthlySeries.ToString());

