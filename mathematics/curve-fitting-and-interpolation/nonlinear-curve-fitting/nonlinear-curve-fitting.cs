//=====================================================================
//
//  File: nonlinear-curve-fitting.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;
// The curve fitting classes reside in the
// Numerics.NET.Curves namespace.
using Numerics.NET.Curves;
// The predefined non-linear curves reside in the
// Numerics.NET.Curves.Nonlinear namespace.
using Numerics.NET.Curves.Nonlinear;
// Vectors reside in the Numerics.NET.Mathemaics.LinearAlgebra
// namespace
using Numerics.NET;

namespace Numerics.NET.QuickStart.CSharp
{
    /// <summary>
    /// Illustrates nonlinear least squares curve fitting using the
    /// NonlinearCurveFitter class in the
    /// Numerics.NET.Curves namespace of Numerics.NET.
    /// </summary>
    class NonlinearCurveFitting
    {
        static void Main(string[] args)
        {
            // The license is verified at runtime. We're using
            // a 30 day trial key here. For more information, see
            //     https://numerics.net/trial-key
            Numerics.NET.License.Verify("your-trial-key-here");

            // Nonlinear least squares fits are calculated using the
            // NonlinearCurveFitter class:
            NonlinearCurveFitter fitter = new NonlinearCurveFitter();

            // In the first example, we fit a dose response curve
            // to a data set that includes error information.

            // The data points must be supplied as vectors:
            var dose = Vector.Create(1.46247, 2.3352,
                4, 7, 12, 18, 23, 30, 40, 60, 90, 160, 290, 490, 860);
            var response = Vector.Create(95.49073, 95.14551, 94.86448,
                92.66762, 85.36377, 74.72183, 62.76747, 51.04137, 38.20257,
                28.01712, 19.40086, 13.18117, 9.87161, 7.64622, 7.21826);
            var error = Vector.Create(4.74322, 4.74322, 4.74322,
                4.63338, 4.26819, 3.73609, 3.13837, 3.55207, 3.91013,
                2.40086, 2.6, 3.65906, 2.49358, 2.38231, 2.36091);

            // You must supply the curve whose parameters will be
            // fit to the data. The curve must inherit from NonlinearCurve.
            // The FourParameterLogistic curve is one of several
            // predefined nonlinear curves:
            FourParameterLogisticCurve doseResponseCurve
                = new FourParameterLogisticCurve();

            // Now we set the curve fitter's Curve property:
            fitter.Curve = doseResponseCurve;
            // and the data values:
            fitter.XValues = dose;
            fitter.YValues = response;
            // The GetInitialFitParameters method returns a set of
            // initial values appropriate for the data:
            fitter.InitialGuess = doseResponseCurve.GetInitialFitParameters(dose, response);

            // The GetWeightVectorFromErrors method of the WeightFunctions
            // class lets us convert the error values to weights:
            fitter.WeightVector = WeightFunctions.GetWeightVectorFromErrors(error);

            // The Fit method performs the actual calculation.
            fitter.Fit();
            // The standard deviations associated with each parameter
            // are available through the GetStandardDeviations method.
            var s = fitter.GetStandardDeviations();

            // We can now print the results:
            Console.WriteLine("Dose response curve");

            Console.WriteLine("Initial value: {0,10:F6} +/- {1:F4}", doseResponseCurve.InitialValue, s[0]);
            Console.WriteLine("Final value:   {0,10:F6} +/- {1:F4}", doseResponseCurve.FinalValue, s[1]);
            Console.WriteLine("Center:        {0,10:F6} +/- {1:F4}", doseResponseCurve.Center, s[2]);
            Console.WriteLine("Hill slope:    {0,10:F6} +/- {1:F4}", doseResponseCurve.HillSlope, s[3]);

            // We can also show some statistics about the calculation:
            Console.WriteLine($"Residual sum of squares: {fitter.Residuals.Norm()}");
            // The Optimizer property returns the MultidimensionalOptimization object
            // used to perform the calculation:
            Console.WriteLine($"# iterations: {fitter.Optimizer.IterationsNeeded}");
            Console.WriteLine($"# function evaluations: {fitter.Optimizer.EvaluationsNeeded}");

            Console.WriteLine();

            //
            // Defining your own nonlinear curve
            //

            // In this example, we use one of the datasets (MGH10)
            // from the National Institute for Statistics and Technology
            // (NIST) Statistical Reference Datasets.
            // See http://www.itl.nist.gov/div898/strd for details

            fitter = new NonlinearCurveFitter();

            // Here, we need to define our own curve.
            // The MyCurve class is defined below.
            fitter.Curve = new MyCurve();

            // You can use Automatic Differentiation to compute
            // the derivative of the function and the partial derivatives
            // with respect to the curve parameters.

            // To do so, call the FromExpression method of the NonlinearCurve
            // class with a lambda expression for the value of the function.
            // The first argument of the lambda is the x value. The remaining
            // arguments correspond to the curve parameters:
            fitter.Curve = NonlinearCurve.FromExpression((x, a, b, c) => a * Math.Exp(b / (x + c)));

            // In this case, we have to specify the initial values
            // for the parameters:
            fitter.InitialGuess = Vector.Create(0.2, 40000, 2500);

            // The data is provided as vectors.
            // X values go into the XValues property...
            fitter.XValues = Vector.Create(new double[]
            {
                5.000000E+01, 5.500000E+01, 6.000000E+01, 6.500000E+01,
                7.000000E+01, 7.500000E+01, 8.000000E+01, 8.500000E+01,
                9.000000E+01, 9.500000E+01, 1.000000E+02, 1.050000E+02,
                1.100000E+02, 1.150000E+02, 1.200000E+02, 1.250000E+02,
            });
            // ...and Y values go into the YValues property:
            fitter.YValues = Vector.Create(new double[]
            {
                3.478000E+04, 2.861000E+04, 2.365000E+04, 1.963000E+04,
                1.637000E+04, 1.372000E+04, 1.154000E+04, 9.744000E+03,
                8.261000E+03, 7.030000E+03, 6.005000E+03, 5.147000E+03,
                4.427000E+03, 3.820000E+03, 3.307000E+03, 2.872000E+03
            });
            fitter.WeightVector = null;
            // The Fit method performs the actual calculation:
            fitter.Fit();

            // A vector containing the parameters of the best fit
            // can be obtained through the BestFitParameters property.
            var solution = fitter.BestFitParameters;
            s = fitter.GetStandardDeviations();

            Console.WriteLine("NIST Reference Data Set");
            Console.WriteLine("Solution:");
            Console.WriteLine($"b1: {solution[0],20} {s[0],20}");
            Console.WriteLine($"b2: {solution[1],20} {s[1],20}");
            Console.WriteLine($"b3: {solution[2],20} {s[2],20}");

            Console.WriteLine("Certified values:");
            Console.WriteLine($"b1: {5.6096364710E-03,20} {1.5687892471E-04,20}");
            Console.WriteLine($"b2: {6.1813463463E+03,20} {2.3309021107E+01,20}");
            Console.WriteLine($"b3: {3.4522363462E+02,20} {7.8486103508E-01,20}");

            // Now let's redo the same operation, but with observations weighted
            // by 1/Y^2. To do this, we set the WeightFunction property.
            // The WeightFunctions class defines a set of ready-to-use weight functions.
            fitter.WeightFunction = WeightFunctions.OneOverX;
            // Refit the curve:
            fitter.Fit();
            solution = fitter.BestFitParameters;
            s = fitter.GetStandardDeviations();

            // The solution is slightly different:
            Console.WriteLine("Solution (weighted observations):");
            Console.WriteLine($"b1: {solution[0],20} {s[0],20}");
            Console.WriteLine($"b2: {solution[1],20} {s[1],20}");
            Console.WriteLine($"b3: {solution[2],20} {s[2],20}");

        }
    }

    // This is our nonlinear curve implementation. For details, see
    // http://www.itl.nist.gov/div898/strd/nls/data/mgh10.shtml
    // You must inherit from NonlinearCurve:
    public class MyCurve : NonlinearCurve
    {
        // Call the base constructor with the number of
        // parameters.
        public MyCurve() : base(3)
        {
            // It is convenient to set common starting values
            // for the curve parameters in the constructor:
            this.Parameters[0] = 0.2;
            this.Parameters[1] = 40000;
            this.Parameters[2] = 2500;
        }

        // The ValueAt method evaluates the function:
        override public double ValueAt(double x)
        {
            return Parameters[0] * Math.Exp(Parameters[1] / (x + Parameters[2]));
        }

        // The SlopeAt method evaluates the derivative:
        override public double SlopeAt(double x)
        {
            return Parameters[0] * Parameters[1] * Math.Exp(Parameters[1] / (x + Parameters[2]))
                / Math.Pow(x + Parameters[2], 2);
        }

        // The FillPartialDerivatives evaluates the partial derivatives
        // with respect to the curve parameters, and returns
        // the result in a vector. If you don't supply this method,
        // a numerical approximation is used.
        override public void FillPartialDerivatives(double x, Numerics.NET.LinearAlgebra.DenseVector<double> f)
        {
            double exp = Math.Exp(Parameters[1] / (x + Parameters[2]));
            f[0] = exp;
            f[1] = Parameters[0] * exp / (x + Parameters[2]);
            f[2] = -Parameters[0] * Parameters[1] * exp / Math.Pow(x + Parameters[2], 2);
        }
    }
}
