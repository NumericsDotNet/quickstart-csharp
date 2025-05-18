//=====================================================================
//
//  File: non-uniform-random-numbers.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

using Numerics.NET.Statistics.Distributions;
using Numerics.NET.Random;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates generating non-uniform random numbers
    /// using the classes in the Numerics.NET.Statistics.Random
    /// namespace.
    /// </summary>
    class NonUniformRandomNumbers
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // Random number generators and the generation
            // of uniform pseudo-random numbers are illustrated
            // in the UniformRandomNumbers QuickStart Sample.

            // In this sample, we will generate numbers from
            // an exponential distribution, and compare summary
            // results to what would be expected from
            // the corresponding Poisson distribution.

            double meanTimeBetweenEvents = 0.42;

            // We will use the exponential distribution to generate the time
            // between events. The number of events per unit time follows
            // a Poisson distribution.

            // The parameter of the exponential distribution is the time between events.
            var exponential = new ExponentialDistribution(meanTimeBetweenEvents);
            // The parameter of the Poisson distribution is the mean number of events
            // per unit time, which is the reciprocal of the time between events:
            var poisson = new PoissonDistribution(1 / meanTimeBetweenEvents);

            // We use a MersenneTwister to generate the random numbers:
            var random = new MersenneTwister();

            // The totals array will track the number of events per time unit.
            int[] totals = new int[15];

            double currentTime = 0;
            double endOfCurrentTimeUnit = 1;
            int eventsInUnit = 0;

            double totalTime = 0;
            int count = 0;

            while (currentTime < 100000)
            {
                double timeBetween = exponential.Sample(random);
                totalTime += timeBetween; count++;
                // Alternatively, we could have written
                //   timeBetween = random.NextDouble(exponential);
                // which would give an identical result.
                currentTime += timeBetween;
                while (currentTime > endOfCurrentTimeUnit)
                {
                    if (eventsInUnit >= totals.Length)
                        eventsInUnit = totals.Length-1;
                    totals[eventsInUnit]++;
                    eventsInUnit = 0;
                    endOfCurrentTimeUnit++;
                }
                eventsInUnit++;
            }

            Console.WriteLine($"{totalTime / count}");
            // Now print the totals
            Console.WriteLine("# Events    Actual  Expected");
            for(int i = 0; i < totals.Length; i++)
            {
                int expected = (int)(100000 * poisson.Probability(i));
                Console.WriteLine("{0,8}  {1,8}  {2,8}", i, totals[i], expected);
            }

        }
    }
}
