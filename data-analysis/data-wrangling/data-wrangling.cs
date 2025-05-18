//=====================================================================
//
//  File: data-wrangling.cs
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

using Numerics.NET.DataAnalysis;
using Numerics.NET;

using Index = Numerics.NET.DataAnalysis.Index;

namespace Numerics.NET.QuickStart.CSharp {
    /// <summary>
    /// Illustrates how to perform basic data wrangling operations
    /// on data frames.
    /// </summary>
    class DataWrangling {

        static void Main(string[] args) {

            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            //
            // Joining and reshaping
            //

            // When data comes from different sources,
            // the Append method lets you join the two
            // data frames:
            var frame = DataFrame.FromColumns(new Dictionary<string, object>() {
                { "A", new string[] { "A0", "A1", "A2", "A3"} },
                { "B", new string[] { "B0", "B1", "B2", "B3"} },
                { "C", new string[] { "C0", "C1", "C2", "C3"} },
                { "D", new string[] { "D0", "D1", "D2", "D3" } } },
                Index.Default(0, 3));
            var df2 = DataFrame.FromColumns(new Dictionary<string, object>() {
                { "A", new string[] { "A4", "A5", "A6", "A7"} },
                { "B", new string[] { "B4", "B5", "B6", "B7"} },
                { "C", new string[] { "C4", "C5", "C6", "C7"} },
                { "D", new string[] { "D4", "D5", "D6", "D7" } } },
                Index.Default(4, 7));
            var df12 = frame.Append(df2);
            // It is possible to join more than 2 data frames:
            var df3 = DataFrame.FromColumns(new Dictionary<string, object>() {
                { "A", new string[] { "A8", "A9", "A10", "A11"} },
                { "B", new string[] { "B8", "B9", "B10", "B11"} },
                { "C", new string[] { "C8", "C9", "C10", "C11"} },
                { "D", new string[] { "D8", "D9", "D10", "D11" } } },
                Index.Default(8, 11));
            var df123 = DataFrame.Append(frame, df2, df3);

            // When the columns don't match, you can specify
            // a join operation which determines which columns
            // to keep in the result. If a column is missing
            // in a data frame and present in the result,
            // missing values are inserted.
            frame = DataFrame.FromColumns(new Dictionary<string, object>() {
                { "A", new string[] { "A0", "A1", "A2", "A3"} },
                { "B", new string[] { "B0", "B1", "B2", "B3"} },
                { "C", new string[] { "C0", "C1", "C2", "C3"} } },
                Index.Default(0, 3));
            df2 = DataFrame.FromColumns(new Dictionary<string, object>() {
                { "A", new string[] { "A4", "A5", "A6", "A7"} },
                { "B", new string[] { "B4", "B5", "B6", "B7"} },
                { "D", new string[] { "D4", "D5", "D6", "D7" } } },
                Index.Default(4, 7));
            var df12outer = frame.Append(df2, JoinType.Outer);
            var df12Inner = frame.Append(df2, JoinType.Inner);
            // Left column join is equivalent to using the left column index:
            var df12Left = frame.Append(df2, JoinType.Left);
            var df12Left2 = frame.Append(df2, frame.ColumnIndex);
            // Again, these are equivalent:
            var df12Right = frame.Append(df2, JoinType.Right);
            var df12Right2 = frame.Append(df2, df2.ColumnIndex);

            // One to one joins match rows on their keys:
            var dates1 = Index.CreateDateRange(new DateTime(2015, 11, 11), 5, Recurrence.Daily);
            var df4 = Vector.CreateRandom(5).ToDataFrame(dates1, "values1");
            var dates2 = Index.CreateDateRange(dates1[2], 5, Recurrence.Daily);
            var df5 = Vector.CreateRandom(5).ToDataFrame(dates2, "values2");
            var df6 = DataFrame.Join(df4, JoinType.Outer, df5);
            Console.WriteLine(df6);

            // One to many joins match one data frame's index to another's
            // column.
            // Create a list of presidents:
            var numbers = Index.Create(new[] { 44, 43, 42, 41, 40 });
            var names = Vector.Create("Barack Obama", "George W. Bush", "Bill Clinton",
                "George H.W. Bush", "Ronald Reagan");
            var homeStates = Vector.Create("IL", "TX", "AR", "TX", "CA");
            var presidents = DataFrame.FromColumns(new Dictionary<string, object>() {
                { "Name", names }, { "Home state", homeStates } }, numbers);
            // And a list of states indexed by their abbreviations:
            var abbreviations = Index.Create(new[] { "AR", "CA", "GA", "MI", "IL", "TX" });
            var stateNames = Vector.Create("Arkansas", "California", "Georgia",
                "Michigan", "Illinois", "Texas");
            var states = DataFrame.FromColumns(new Dictionary<string, object>() {
                    { "Full name", stateNames} }, abbreviations);
            // Now get the full names of states in the list:
            var presidentsWithState = DataFrame.Join(presidents, JoinType.Left, states, key: "Home state");
            Console.WriteLine(presidentsWithState);

            // When the indexes are sorted, it is possible
            // to do an inexact join to the nearest value.
            // This is useful for time series where one series
            // if offset by a few hours relative to the other:
            var dates7 = Index.CreateDateRange(new DateTime(2015, 11, 11), 5, Recurrence.Daily);
            var df7 = Vector.CreateRandom(5).ToDataFrame(dates7, "values1");
            var dates8 = Index.CreateDateRange(dates7[0].AddHours(3), 5, Recurrence.Daily);
            var df8 = Vector.CreateRandom(5).ToDataFrame(dates8, "values2");
            var df9 = df7.JoinOnNearest(df8, Direction.Backward);
            Console.WriteLine(df9);

            //
            // Sorting and filtering
            //

            // Data frames can be sorted by their index or by
            // a column. The sort methods always return a new data frame.
            dates2 = Index.CreateDateRange(new DateTime(2015, 11, 11), 15, Recurrence.Daily);
            var frame2 = DataFrame.FromColumns(new Dictionary<string, object>() {
                { "values1", Vector.CreateRandom(dates2.Length) },
                { "values2", Vector.CreateRandom(dates2.Length) },
                { "values3", Vector.CreateRandom(dates2.Length) } }, dates2);
            var frame3 = frame2.SortByIndex(SortOrder.Descending);
            var frame4 = frame2.SortBy("values1", SortOrder.Ascending);

        }
    }
}
