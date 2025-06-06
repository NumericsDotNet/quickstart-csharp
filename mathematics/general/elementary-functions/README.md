# Elementary Functions

This sample illustrates how to use additional elementary functions using Numerics.NET.

## Overview

This QuickStart sample demonstrates the use of specialized elementary mathematical functions provided by 
Numerics.NET that offer higher precision and better numerical stability than standard methods.

The sample shows how to use functions that handle edge cases and potential numerical issues that can 
arise when working with floating-point arithmetic. It specifically covers:

- Using `Log1PlusX` for accurately computing log(1+x) when x is close to zero
- Using `ExpMinus1` for precise calculation of exp(x)-1 near zero
- Computing hypotenuse values for very large numbers without overflow using `Hypot`
- Efficient integer power calculations using specialized `Pow` methods

These functions are particularly important in scientific computing and numerical analysis where 
maintaining precision and avoiding numerical instability is crucial. The sample includes comparisons 
between standard .NET Math methods and their numerically stable Numerics.NET counterparts.


## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (.NET 6.0 or .NET 8.0)
- A code editor like [Visual Studio](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/), or [JetBrains Rider](https://www.jetbrains.com/rider/)
- Required NuGet packages: Numerics.NET.Core, Version 9.1.0

### Running the Sample

#### In Visual Studio
1. Open the solution file (.sln) in Visual Studio
2. Restore NuGet packages: Right-click on the solution → Restore NuGet Packages
3. Update the trial key in the code:
4. Press F5 to build and run the sample

#### In VS Code

1. Open the project folder in VS Code
2. Make sure you have the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) installed
3. Open a terminal and run: `dotnet restore`
4. Update the trial key in the code 
5. Run the sample with: `dotnet run`

#### From the Command Line

1. Open a terminal and navigate to the project directory
2. Run `dotnet restore` to restore the required NuGet packages
3. Update the trial key in the code
4. Run the sample with `dotnet run`

### Getting a Trial Key

To run this sample, you'll need a trial key for Numerics.NET:

1. Visit [https://numerics.net/trial-key](https://numerics.net/trial-key)
2. Sign in using the provider of your choice to receive your 30-day trial key
3. Replace the placeholder in `License.Verify("your-trial-key-here")` with your actual trial key

## Related Content

This sample is also available in the following languages: 
[Visual Basic](https://github.com/NumericsDotNet/quickstart-visualbasic/tree/net8.0/mathematics/general/elementary-functions), [F#](https://github.com/NumericsDotNet/quickstart-fsharp/tree/net8.0/mathematics/general/elementary-functions), [IronPython](https://github.com/NumericsDotNet/quickstart-ironpython/tree/net8.0/mathematics/general/elementary-functions).

You can find out more about the methods used in this sample from the Numerics.NET documentation.

### Numerics.NET User's Guide

- [Elementary Functions](https://numerics.net/documentation/latest/mathematics/mathematical-functions/elementary-functions)
- [Comparing Floating-Point Numbers](https://numerics.net/documentation/latest/mathematics/general-classes/comparing-floating-point-numbers)

### Numerics.NET API Reference

- [Elementary class](https://numerics.net/documentation/latest/reference/numerics.net.elementary)


## Troubleshooting

### Common Issues

- **Missing dependencies**: Make sure to run `dotnet restore` before building
- **License verification failed**: Ensure you have a valid trial key from [https://numerics.net/trial-key](https://numerics.net/trial-key)
- **Runtime errors**: Check that you're targeting the correct .NET version (.NET 6 or .NET 8)

### Need Help?

- Check the [Numerics.NET documentation](https://numerics.net/documentation/)
- Contact support at [support@numerics.net](mailto:support@numerics.net?subject=ElementaryFunctions%20QuickStart%20Sample%20%28C%23%29)

## About Numerics.NET

Numerics.NET is a powerful numerical library for .NET that provides advanced mathematical 
functions and algorithms for scientific computing, data analysis, and machine learning.
See [numerics.net](https://numerics.net) for details.

---

_Last updated on 2025-05-06 3:03:10 PM (version 9.1.3)._
