//=====================================================================
//
//  File: data-frames.cs
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
using Numerics.NET.Data.Text;
using Numerics.NET.DataAnalysis;
using Index = Numerics.NET.DataAnalysis.Index;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates how to create and manipulate data frames.
    /// </summary>
    class DataFrames
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // Data frames can be constructed in a variety of ways.
            // This example will use mostly static methods of the
            // static DataFrame class.
            // From a dictionary of column keys that map to collections:
            var data = new Dictionary<string, object>() {
                    { "state", new string[] { "Ohio", "Ohio", "Ohio", "Nevada", "Nevada" } },
                    { "year", new int[] { 2000, 2001, 2002, 2001, 2002 } },
                    { "pop", new double[] { 1.5, 1.7, 3.6, 2.4, 2.9 } }
                };
            var df1 = DataFrame.FromColumns(data);
            Console.WriteLine(df1);

            // The data frame has a default index of row numbers.
            // A row index can be specified as well:
            var df2 = DataFrame.FromColumns(new Dictionary<string, object>() {
                    { "first", new double[] { 11, 14, 17, 93, 55 } },
                    { "second", new double[] { 22, 33, 43, 51, 69 } } },
                    Index.CreateDateRange(new DateTime(2015, 4, 1), 5));
            Console.WriteLine(df2);

            // Alternatively, the columns can be a list of collections.
            var rowIndex = Index.Create(new[] { "one", "two", "three", "four", "five" });
            var df3 = DataFrame.FromColumns(data, rowIndex);
            Console.WriteLine(df3);

            // If you supply a column index, only the columns with
            // keys in the index will be retained:
            var columnIndex = Index.Create(new[] { "pop", "year" });
            var df4 = DataFrame.FromColumns(data, rowIndex, columnIndex);
            Console.WriteLine(df4);

            // Yet another way is to use tuples:
            var df5 = DataFrame.FromColumns(
                ("state", new [] { "Ohio", "Ohio", "Ohio", "Nevada", "Nevada" }),
                ("year", new [] { 2000, 2001, 2002, 2001, 2002 }),
                ("pop", new [] { 1.5, 1.7, 3.6, 2.4, 2.9 }));
            Console.WriteLine(df5);

            // Data frames can be created from a sequence of objects.
            // By default, all public properties are included as columns
            // in the resulting data frame:
            var points = new[]
            {
                new { X = 1, Y = 5, Z = 9 },
                new { X = 2, Y = 6, Z = 10 },
                new { X = 3, Y = 7, Z = 11 },
                new { X = 4, Y = 8, Z = 12 }
            };
            var df6 = DataFrame.FromObjects(points);
            Console.WriteLine(df6);

            // It is possible to select the properties:
            var df7 = DataFrame.FromObjects(points, new[] { "Z", "X" });
            Console.WriteLine(df7);

            // Vectors and matrices can be converted to data frames
            // using their ToDataFrame method:
            var m = Matrix.CreateRandom(10, 2);
            var df8 = m.ToDataFrame(Index.Default(10), Index.Create(new[] { "A", "B" }));
            var v = Vector.CreateRandom(3);
            var df9 = v.ToDataFrame(Index.Create(new[] { "a", "b", "c" }), "values");

            //
            // Import / export
            //

            // Several methods exist for importing data frames directly
            // from data sources like text files, R data files, and databases.
            var dt = new System.Data.DataTable();
            dt.Columns.Add("x1", typeof(double));
            dt.Columns.Add("x2", typeof(double));
            dt.Rows.Add(new object[] { 1.0, 2.0 });
            dt.Rows.Add(new object[] { 3.0, 4.0 });
            dt.Rows.Add(new object[] { 5.0, 6.0 });
            var df11 = DataFrame.FromDataTable(dt);
            var df12 = DelimitedTextFile.ReadDataFrame(@"..\..\..\..\data\iris.csv",
                DelimitedTextOptions.CsvWithoutHeader);

            // By default, these methods return a data frame with a default
            // index (row numbers). You can specify the column(s) to use
            // for the index, and the data frame will use that column.
            var df13 = DelimitedTextFile.ReadDataFrame<int>(@"..\..\..\..\data\titanic.csv",
                "PassengerId", options: DelimitedTextOptions.Csv);
            DelimitedTextFile.Write("irisCopy.csv", df12, DelimitedTextOptions.Csv);

            //
            // Setting row and column indexes
            //

            // You can use specific columns as the row index.
            // Here we have a 2 level hierarchical index:
            var df1a = df1.WithRowIndex<string, int>("state", "year");

            /// Column indexes can be changed as well:
            var df2b = df2.WithColumnIndex(new string[] { "A", "B" });
        }
    }
}
