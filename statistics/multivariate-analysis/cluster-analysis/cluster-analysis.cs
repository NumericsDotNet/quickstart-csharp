//=====================================================================
//
//  File: cluster-analysis.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
using System.Linq;

using Numerics.NET.Data.Stata;
using Numerics.NET;
using Numerics.NET.Statistics;
using Numerics.NET.Statistics.Multivariate;

namespace Numerics.NET.Quickstart.CSharp
{
    /// <summary>
    /// Demonstrates how to use classes that implement
    /// hierarchical and K-means clustering.
    /// </summary>
    class ClusterAnalysis
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // This QuickStart Sample demonstrates how to run two
            // common multivariate analysis techniques:
            // hierarchical cluster analysis and K-means cluster analysis.
            //
            // The classes used in this sample reside in the
            // Numerics.NET.Statistics.Multivariate namespace..

            // First, our dataset, which is from
            //     Computer-Aided Multivariate Analysis, 4th Edition
            //     by A. A. Afifi, V. Clark and S. May, chapter 16
            //     See http://www.ats.ucla.edu/stat/Stata/examples/cama4/default.htm
            var frame = StataFile.ReadDataFrame(@"..\..\..\..\..\Data\companies.dta");

            //
            // Hierarchical cluster analysis
            //

            Console.WriteLine("Hierarchical clustering");

            // Create the model:
            var columns = new string[] { "ror5", "de", "salesgr5", "eps5", "npm1", "pe", "payoutr1" };
            var hc = new HierarchicalClusterAnalysis(frame, columns);
            // Alternatively, we could use a formula to specify the variables:
            string formula = "ror5 + de + salesgr5 + eps5 + npm1 + pe + payoutr1";
            hc = new HierarchicalClusterAnalysis(frame, formula);

            // Rescale the variables to their Z-scores before doing the analysis:
            hc.Standardize = true;
            // The linkage method defaults to Centroid:
            hc.LinkageMethod = LinkageMethod.Centroid;
            // We could set the distance measure. We use the default:
            hc.DistanceMeasure = DistanceMeasures.SquaredEuclideanDistance;

            // Compute the model:
            hc.Fit();

            // We can partition the cases into clusters:
            HierarchicalClusterCollection partition = hc.GetClusterPartition(5);
            // Individual clusters are accessed through an index, or through enumeration.
            foreach(HierarchicalCluster cluster in partition)
                Console.WriteLine("Cluster {0} has {1} members.", cluster.Index, cluster.Size);

            var count3 = partition[3].Size;
            Console.WriteLine($"Number of items in cluster 3: {count3}");

            // Get a variable that shows memberships:
            var memberships = partition.GetMemberships();
            for (int i = 15; i < memberships.Length; i++)
                Console.WriteLine("Observation {0} belongs to cluster {1}", i, memberships[i].Index);

            // A dendrogram is a graphical representation of the clustering in the form of a tree.
            // You can get all the information you need to draw a dendrogram starting from
            // the root node of the dendrogram:
            DendrogramNode root = hc.DendrogramRoot;
            // Position and DistanceMeasure give the x and y coordinates:
            Console.WriteLine("Root position: ({0:F4}, {1:F4}", root.Position, root.DistanceMeasure);
            // The left and right children:
            Console.WriteLine($"Position of left child: {root.LeftChild.Position:F4}");
            Console.WriteLine($"Position of right child: {root.RightChild.Position:F4}");

            // You can also get a permutation that orders the observations
            // in a way suitable for drawing the dendrogram:
            var sortOrder = hc.GetDendrogramOrder();
            Console.WriteLine();

            //
            // K-Means Clustering
            //

            Console.WriteLine("K-means clustering");

            // Create the model:
            KMeansClusterAnalysis kmc = new KMeansClusterAnalysis(frame, columns, 3);
            // Rescale the variables to their Z-scores before doing the analysis:
            kmc.Standardize = true;

            // Compute the model:
            kmc.Fit();

            // The Predictions property is a categorical vector that contains
            // the cluster assignments:
            var predictions = kmc.Predictions;
            // The GetDistancesToCenters method returns a vector containing
            // the distance of each observations to its center.
            var distances = kmc.GetDistancesToCenters();

            // For example:
            for (int i = 18; i < predictions.Length; i++)
                Console.WriteLine("Observation {0} belongs to cluster {1}, distance: {2:F4}.",
                    i, predictions[i], distances[i]);

            // You can use this to compute several statistics:
            var descriptives = distances.SplitBy(predictions)
                .Map(x => new Descriptives<double>(x));

            // Individual clusters are accessed through an index, or through enumeration.
            for (int i = 0; i < descriptives.Length; i++)
            {
                Console.WriteLine("Cluster {0} has {1} members. Sum of squares: {2:F4}",
                    i, descriptives[i].Count, descriptives[i].SumOfSquares);
                Console.WriteLine($"Center: {kmc.Clusters[i].Center:F4}");
            }

            // The distances between clusters are also available:
            Console.WriteLine(kmc.GetClusterDistances().ToString("F4"));

            // You can get a filter for the observations in a single cluster.
            // This uses the GetIndexes method of categorical vectors.
            var level1Indexes = kmc.Predictions.GetIndexes(1).ToArray();
            Console.WriteLine($"Number of items in cluster 1: {level1Indexes.Length}");

        }
    }
}
