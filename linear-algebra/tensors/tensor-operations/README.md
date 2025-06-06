# Tensor Operations

This sample illustrates how to perform operations on tensors using Numerics.NET.

## Overview

This QuickStart sample demonstrates how to perform common operations on tensors using Numerics.NET.

The sample covers fundamental tensor operations including arithmetic operations (addition, subtraction,
multiplication, division), broadcasting operations between tensors of different shapes, mathematical
functions applied to tensors, conditional operations using masks, reduction operations like sum and
mean, and working with tensor views. It shows how to:

- Create tensors and perform basic arithmetic operations
- Use broadcasting to operate on tensors with different shapes
- Apply mathematical functions to tensors
- Use conditional operations with masks to selectively modify tensor elements
- Perform reduction operations along specified axes
- Work with tensor views that share underlying storage
- Manipulate complex-valued tensors using real and imaginary parts

The sample includes detailed examples with output showing the results of each operation, making it
ideal for developers new to tensor operations or those wanting to understand how to efficiently
work with multi-dimensional arrays in Numerics.NET.


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
[Visual Basic](https://github.com/NumericsDotNet/quickstart-visualbasic/tree/net8.0/linear-algebra/tensors/tensor-operations), [F#](https://github.com/NumericsDotNet/quickstart-fsharp/tree/net8.0/linear-algebra/tensors/tensor-operations).

You can find out more about the methods used in this sample from the Numerics.NET documentation.

### Numerics.NET User's Guide

- [Tensor Basics](https://numerics.net/documentation/latest/vector-and-matrix/tensors/tensor-basics)
- [Creating Tensors](https://numerics.net/documentation/latest/vector-and-matrix/tensors/creating-tensors)
- [Mathematical Operations](https://numerics.net/documentation/latest/vector-and-matrix/tensors/mathematical-operations)
- [Accessing Tensor Elements](https://numerics.net/documentation/latest/vector-and-matrix/tensors/accessing-tensor-elements)
- [Manipulating Tensors](https://numerics.net/documentation/latest/vector-and-matrix/tensors/manipulating-tensors)

### Numerics.NET API Reference

- [Tensor class](https://numerics.net/documentation/latest/reference/numerics.net.tensors.tensor)
- [Tensor&lt;T&gt; class](https://numerics.net/documentation/latest/reference/numerics.net.tensors.tensor-1)
- [Numerics.NET.Tensors namespace](https://numerics.net/documentation/latest/reference/numerics.net.tensors)


## Troubleshooting

### Common Issues

- **Missing dependencies**: Make sure to run `dotnet restore` before building
- **License verification failed**: Ensure you have a valid trial key from [https://numerics.net/trial-key](https://numerics.net/trial-key)
- **Runtime errors**: Check that you're targeting the correct .NET version (.NET 6 or .NET 8)

### Need Help?

- Check the [Numerics.NET documentation](https://numerics.net/documentation/)
- Contact support at [support@numerics.net](mailto:support@numerics.net?subject=TensorOperations%20QuickStart%20Sample%20%28C%23%29)

## About Numerics.NET

Numerics.NET is a powerful numerical library for .NET that provides advanced mathematical 
functions and algorithms for scientific computing, data analysis, and machine learning.
See [numerics.net](https://numerics.net) for details.

---

_Last updated on 2025-05-06 3:03:10 PM (version 9.1.3)._
