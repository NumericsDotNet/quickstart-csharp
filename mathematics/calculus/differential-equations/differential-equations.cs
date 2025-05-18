//=====================================================================
//
//  File: differential-equations.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

using Numerics.NET;
using Numerics.NET.Calculus.OrdinaryDifferentialEquations;

// Illustrates integrating systems of ordinary differential equations
// (ODE's) using classes in the
// Numerics.NET.Calculus.OrdinaryDifferentialEquations
// namespace.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

// The ClassicRungeKuttaIntegrator class implements the
// well-known 4th order fixed step Runge-Kutta method.
ClassicRungeKuttaIntegrator rk4 = new ClassicRungeKuttaIntegrator();

// The differential equation is expressed in terms of a
// DifferentialFunction delegate. This is a function that
// takes a double (time value) and two Vectors (y value and
// return value)  as arguments.
//
// The Lorentz function below defines the differential function
// for the Lorentz attractor.
rk4.DifferentialFunction = Lorentz;

// To perform the computations, we need to set the initial time...
rk4.InitialTime = 0.0;
// and the initial value.
rk4.InitialValue = Vector.Create(1.0, 0.0, 0.0);
// The Runge-Kutta integrator also requires a step size:
rk4.InitialStepsize = 0.1;

Console.WriteLine("Classic 4th order Runge-Kutta");
for (int i = 0; i <= 5; i++) {
    double t = 0.2 * i;
    // The Integrate method performs the integration.
    // It takes as many steps as necessary up to
    // the specified time and returns the result:
    var y = rk4.Integrate(t);
    // The IterationsNeeded always shows the number of steps
    // needed to arrive at the final time.
    Console.WriteLine("{0:F2}: {1,20:F14} ({2} steps)", t, y, rk4.IterationsNeeded);
}

// The RungeKuttaFehlbergIntegrator class implements a variable-step
// Runge-Kutta method. This method chooses the stepsize, and so
// is generally more reliable.
RungeKuttaFehlbergIntegrator rkf45 = new RungeKuttaFehlbergIntegrator();

rkf45.InitialTime = 0.0;
rkf45.InitialValue = Vector.Create(1.0, 0.0, 0.0);
rkf45.DifferentialFunction = Lorentz;
rkf45.AbsoluteTolerance = 1e-8;

Console.WriteLine("Classic 4/5th order Runge-Kutta-Fehlberg");
for (int i = 0; i <= 5; i++) {
    double t = 0.2 * i;
    var y = rkf45.Integrate(t);
    Console.WriteLine("{0:F2}: {1,20:F14} ({2} steps)", t, y, rkf45.IterationsNeeded);
}

// The CVODE integrator, part of the SUNDIALS suite of ODE solvers,
// is the most advanced of the ODE integrators.
CvodeIntegrator cvode = new CvodeIntegrator() {
    InitialTime = 0.0,
    InitialValue = Vector.Create(1.0, 0.0, 0.0),
    DifferentialFunction = Lorentz,
    AbsoluteTolerance = 1e-8
};

Console.WriteLine("CVODE (multistep Adams-Moulton)");
for (int i = 0; i <= 5; i++) {
    double t = 0.2 * i;
    var y = cvode.Integrate(t);
    Console.WriteLine("{0:F2}: {1,20:F14} ({2} steps)", t, y, cvode.IterationsNeeded);
}

//
// Other properties and methods
//

// The IntegrateSingleStep method takes just a single step
// in the direction of the specified final time:
cvode.IntegrateSingleStep(2.0);
// The CurrentTime property returns the corresponding time value.
Console.WriteLine($"t after single step: {cvode.CurrentTime}");
// CurrentValue returns the corresponding value:
Console.WriteLine($"Value at this t: {cvode.CurrentValue:F14}");
// The IntegrateMultipleSteps method performs the integration
// until at least the final time, and returns the last
// value that was computed:
cvode.IntegrateMultipleSteps(2.0);
Console.WriteLine($"t after multiple steps: {cvode.CurrentTime}");

//
// Stiff systems
//

Console.WriteLine("\nStiff systems");

// The CVODE integrator is the only ODE integrator that can
// handle stiff problems well. The following example uses
// an equation for the size of a flame. The smaller
// the initial size, the more stiff the equation is.

// We compare the performance of the variable step Runge-Kutta method
// and the CVODE integrator:

double delta = 0.0001;
double tFinal = 2 / delta;

rkf45 = new RungeKuttaFehlbergIntegrator();
rkf45.InitialTime = 0.0;
rkf45.InitialValue = Vector.Create(delta);
rkf45.DifferentialFunction = Flame;

Console.WriteLine("Classic 4/5th order Runge-Kutta-Fehlberg");
for (int i = 0; i <= 10; i++) {
    double t = i * 0.1 * tFinal;
    var y = rkf45.Integrate(t);
    Console.WriteLine("{0:F2}: {1,20:F14} ({2} steps)", t, y, rkf45.IterationsNeeded);
}

// The CVODE integrator will use a special (implicit) method
// for stiff problems. To select this method, pass
// OdeKind.Stiff as an argument to the constructor.
cvode = new CvodeIntegrator(OdeKind.Stiff);
cvode.InitialTime = 0.0;
cvode.InitialValue = Vector.Create(delta);
cvode.DifferentialFunction = Flame;
// When solving stiff problems, a Jacobian for the system
// must be supplied. See below for the definition.
cvode.SetDenseJacobian(FlameJacobian);

// Notice how the CVODE integrator takes a lot fewer steps,
// and is also more accurate than the variable-step
// Runge-Kutta method.
Console.WriteLine("CVODE (Stiff - Backward Differentiation Formula)");
for (int i = 0; i <= 10; i++) {
    double t = i * 0.1 * tFinal;
    var y = cvode.Integrate(t);
    Console.WriteLine("{0:F2}: {1,20:F14} ({2} steps)", t, y, cvode.IterationsNeeded);
}


/// <summary>
/// Represents the differential function for the Lorentz attractor.
/// </summary>
/// <param name="t">The time value.</param>
/// <param name="y">The current value.</param>
/// <param name="dy">On output, the first derivatives.</param>
/// <returns>A reference to <paramref name="dy"/>.</returns>
/// <remarks><paramref name="dy"/> may be <see langword="null"/>
/// on input.</remarks>
static Vector<double> Lorentz(double t, Vector<double> y, Vector<double> dy)
{
    if (dy == null)
        dy = Vector.Create<double>(3);

    double sigma = 10.0;
    double beta = 8.0 / 3.0;
    double rho = 28.0;

    dy[0] = sigma * (y[1] - y[0]);
    dy[1] = y[0] * (rho - y[2]) - y[1];
    dy[2] = y[0] * y[1] - beta * y[2];

    return dy;
}

/// <summary>
/// Represents the differential function for the flame expansion
/// problem.
/// </summary>
static Vector<double> Flame(double t, Vector<double> y, Vector<double> dy)
{
    if (dy == null)
        dy = Vector.Create<double>(1);

    dy[0] = y[0] * y[0] * (1 - y[0]);

    return dy;
}

/// <summary>
/// Represents the Jacobian of the differential function
/// for the flame expansion problem.
/// </summary>
/// <param name="t">The time value.</param>
/// <param name="y">The current value.</param>
/// <param name="dy">The current value of the first derivatives.</param>
/// <param name="J">A Matrix that, on output, contains the value
/// of the Jacobian.</param>
/// <returns>A reference to <paramref name="J"/>.</returns>
/// <remarks>The Jacobian is the matrix of partial derivatives of each
/// equation in the system with respect to each variable in the system.
/// <paramref name="J"/> may be <see langword="null"/>
/// on input.</remarks>
static Matrix<double> FlameJacobian(double t, Vector<double> y, Vector<double> dy, Matrix<double> J) {

    if (J == null)
        J = Matrix.Create<double>(1, 1);

    J[0, 0] = (2 - 3 * y[0]) * y[0];

    return J;
}
