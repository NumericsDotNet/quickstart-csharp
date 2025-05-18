//=====================================================================
//
//  File: indexes-and-labels.cs
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

using Index = Numerics.NET.DataAnalysis.Index;

namespace IndexesAndLabels {

    /// <summary>
    /// Illustrates how to use indexes to label the elements
    /// of a vector, or the rows and columns of a matrix.
    /// </summary>
    class IndexesAndLabels {

        static void Main(string[] args) {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            //
            // Indexes
            //

            // An index is a set of keys that can be used
            // to label one or more dimensions of a vector,
            // matrix, or data frame.

            //
            // Construction
            //

            // The simplest way to create an index is from an array:
            var index = Index.Create(new[] { "a", "b", "c", "d" });
            // We can then assign this to the Index property of a vector:
            var v = Vector.Create(new double[] { 1.0, 2.0, 3.0, 4.0 });
            v.Index = index;
            Console.WriteLine(v);

            // An index by position is very common,
            // and can be created efficiently using the
            // Default method:
            var numbers = Index.Default(10); // 0, 1, ..., 9
            var numbers2 = Index.Default(10, 20); // 10, 11, ..., 19

            // Various options exist to create indexes over date ranges,
            // for example:
            var dateIndex = Index.CreateDateRange(new DateTime(2015, 4, 25), 10);
            // 2015/4/25, 2015/4/26, ..., 2015/5/4

            // Finally, for some purposes it may be useful to create
            // an index of intervals, for example when you want to
            // categorize people into age groups:
            int[] ages = { 0, 18, 35, 65 };
            var ageGroups = Index.CreateBins(ages, SpecialBins.AboveMaximum);

            //
            // Properties
            //

            // Indexes have a length
            Console.WriteLine($"# of keys in index: {index.Length}");
            // Indexes usually have unique elements.
            Console.WriteLine($"Keys are unique? {index.IsUnique}");
            // The elements may be sorted or not.
            Console.WriteLine($"Keys are sorted? {index.IsSorted}");
            Console.WriteLine($"Sort order: {index.SortOrder}");

            //
            // Lookup
            //

            // Once created, you can look up the position of a key:
            var position = index.Lookup("c"); // = 2
            if (index.TryLookup("e", out position))
                Console.WriteLine("We shouldn't be here.");

            // You can also look up the nearest date.
            var dates = Index.CreateDateRange(DateTime.Today.AddDays(-5), 10);
            var now = DateTime.Now;
            // An exact lookup fails in this case:
            if (!dates.TryLookup(now, out position))
                Console.WriteLine("Exact lookup failed.");
            // But looking for the nearest key works fine:
            position = dates.LookupNearest(now, Direction.Backward); // = 5
            position = dates.LookupNearest(now, Direction.Forward); // = 6

            //
            // Automatic alignment
            //

            // One of the useful features of indexes is that
            // values are aligned on key values automatically.
            // For example, given two vectors:
            var a = Vector.Create(
                new[] { 1.0, 2.0, 3.0, 4.0 },
                new[] { "a", "b", "c", "d" });
            var b = Vector.Create(
                new[] { 10.0, 30.0, 40.0, 50.0 },
                new[] { "a", "c", "d", "e" });
            // We can compute their sum:
            Console.WriteLine(a + b);
            // and we find that elements are added
            // when they have the same key,
            // not when they have the same position.

            // Indexes also propagate through calculations:
            Console.WriteLine($"Exp(a) = \n{Vector.Exp(a)}");
            Console.WriteLine($"a[a % 2 == 0] =\n{a[x => x % 2 == 0]}");

            // Matrices can have a row and/or a column index:
            var c = Matrix.CreateRandom(100, 4);
            c.ColumnIndex = Index.Create(new[] { "a", "b", "c", "d" });
            var cTc = c.Transpose() * c;
            Console.WriteLine($"C^T*C = \n{cTc.Summarize()}");

        }
    }
}
