//=====================================================================
//
//  File: grouping-and-aggregation.cs
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
using Index = Numerics.NET.DataAnalysis.Index;

namespace Numerics.NET.QuickStart.CSharp {

    /// <summary>
    /// Illustrates how to group data and how to compute aggregates
    /// over groups and entire datasets.
    /// </summary>
    class GroupingAndAggregation {

        static void Main(string[] args) {

            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // We work with the Titanic dataset
            var titanic = DelimitedTextFile.ReadDataFrame(@"..\..\..\..\data\titanic.csv");
            // We'll use these columns often:
            var age = titanic.GetColumn("Age");
            var survived = titanic["Survived"].As<bool>();
            // We want to group by the passenger class,
            // so we make this a categorical vector.
            var pclass = titanic["Pclass"].AsCategorical();

            //
            // Aggregators and Aggregation
            //

            // The Aggregators class defines all common aggregator functions.
            // Here we compute the mean and do the computations using the double
            // type. The Aggregate method applies the aggregator
            // to every column in the data frame:
            var means = titanic.Aggregate(Aggregators.Mean.As<double>());
            Console.WriteLine(means.Summarize());

            // We can create custom aggregators. Here we compute
            // the fraction of true values of a boolean vector:
            var trueFraction = Aggregators.Create(
                (Vector<bool> b) => (double)b.CountTrue() / b.Count);
            var pctSurvived = survived.Aggregate(trueFraction);

            // Using a pivot grouping, we can compute the fractions
            // that survivied in each class:
            var survivedByClass = new Pivot(pclass, survived.AsCategorical())
                .CountsMatrix()
                .NormalizeRowsInPlace(VectorNorm.OneNorm);
            survivedByClass.ColumnIndex = Index.Create(new[] { "Died", "Survived" });
            Console.WriteLine(survivedByClass.Summarize());

            // We can also compute more than one aggregate:
            var descriptives = titanic.Aggregate(
                Aggregators.Count,
                Aggregators.Mean.As<double>(),
                Aggregators.StandardDeviation.As<double>());
            Console.WriteLine(descriptives.Summarize());

            // Aggregations can be applied to individual vectors:
            var meanAge = age.Aggregate(Aggregators.Mean);

            // Or to rows or columns of a matrix:
            var m = Matrix.CreateRandom(5, 8);
            var meanByRow = m.AggregateRows(Aggregators.Mean);
            var meanByColumn = m.AggregateColumns(Aggregators.Mean);

            //
            // Groupings
            //

            // By defining a grouping, we can compute the aggregate
            // for each group.

            // The simplest grouping is by value, similar to
            // GROUP BY clauses in database queries.

            // Let's get the average age by class:
            var ageByClass = age.AggregateBy(pclass, Aggregators.Mean);

            // Grouping by quantile means we sort the values
            // and divide the result into groups of the same size.
            var byQuantile = Grouping.ByQuantile(age, 5);
            var survivedByAgeGroup = survived.AggregateBy(byQuantile, trueFraction);
            Console.WriteLine("Survival rate by age group:");
            Console.WriteLine(survivedByAgeGroup.Summarize());

            // For the remainder we will use a vector with a DateTime index:
            var x = Vector.CreateRandom(200);
            var dates = Index.CreateDateRange(new DateTime(2016, 1, 1), x.Length);
            x.Index = dates;

            // A partition is a straight division of the data into equal groups:
            var partition = Grouping.Partition(dates, 10,
                alignToEnd: true, skipIncomplete: true);
            var partitionAvg = x.AggregateBy(partition, Aggregators.Mean);
            Console.WriteLine("Avg. by partition:");
            Console.WriteLine(partitionAvg);

            //
            // Moving and expanding windows
            //

            // Moving or rolling averages and related statistics
            // can be computed efficiently by using moving windows:
            var window = Grouping.Window(dates, 20);
            var ma20 = x.AggregateBy(window, Aggregators.Mean);
            Console.WriteLine("ma20:");
            Console.WriteLine(ma20.GetSlice(0, 20));
            // Moving standard deviation is just as simple:
            var mstd20 = x.AggregateBy(window, Aggregators.StandardDeviation);
            Console.WriteLine("mstd20:");
            Console.WriteLine(mstd20.GetSlice(0, 20));

            // Moving windows can have a fixed number of elements, as above,
            // or a fixed maximum width:
            var window2 = Grouping.RangeWindow(dates, TimeSpan.FromDays(20));
            var ma20_2= x.AggregateBy(window2, Aggregators.Mean);

            // Expanding windows keep the starting point and move the end point
            // forward in time:
            var expanding = Grouping.ExpandingWindow(dates);
            var expAvg = x.AggregateBy(expanding, Aggregators.Mean);
            Console.WriteLine("expAvg:");
            Console.WriteLine(expAvg.GetSlice(0, 10));

            //
            // Resampling
            //

            // Resampling means computing values for a series
            // with longer periods by aggregating over the values
            // for shorter periods.

            // We start by creating an index with the boundaries,
            // in this case the 10th of each month.
            var months = Index.CreateDateRange(new DateTime(2016, 1, 10),
                12, Recurrence.Monthly);
            // We then create the resampling grouping from this:
            // Giving the Direction argument as Backward means that
            // the last value in the time period is used as the key
            // for the group.
            var resampling1 = Grouping.Resample(dates, months, Direction.Backward);
            // We can also obtain this grouping in one step:
            var resampling2 = Grouping.Resample(dates,
                Recurrence.Monthly.Day(10), Direction.Backward);
            var resampled = x.AggregateBy(resampling2, Aggregators.Mean);

            //
            // Pivot tables
            //

            // A pivot table is a 2-dimensional grouping on two key columns.
            // For this, we go back to the Titanic dataset, and we compute
            // the survival rate per class in a different way. We group
            // by class and by whether the passenger survived:
            var pivot = Grouping.Pivot(
                titanic["Pclass"].As<int>(),
                titanic["Survived"].As<bool>());
            // We can then get the # of elements in each group
            // as a matrix, with rows indexed by class and columns
            // indexed by survived:
            var counts = pivot.CountsMatrix();
            // Scaling by the row sums gives us the fraction
            // of survived/did not survive for each class.
            // Notice that the rows and columns of the matrix
            // are labeled by the class and survival status:
            var fractions = counts.UnscaleRowsInPlace(counts.GetRowSums());
            Console.WriteLine(fractions.Summarize());

        }
    }
}
