//=====================================================================
//
//  File: accessing-tensor-elements.cs
//
//---------------------------------------------------------------------
//
//  This file is part of the Numerics.NET Code Samples.
//
//  Copyright (c) 2004-2025 ExoAnalytics Inc. All rights reserved.
//
//=====================================================================

using System;

// Tensor classes reside in the Numerics.NET.Tensors
// namespace.
using Numerics.NET.Tensors;
// Prefer Numerics.NET Range over System.Range
using Range = Numerics.NET.Range;

// Illustrates different ways of getting and setting
// elements of a tensor.

// The license is verified at runtime. We're using
// a 30 day trial key here. For more information, see
//     https://numerics.net/trial-key
Numerics.NET.License.Verify("your-trial-key-here");

//
// Accessing tensor elements
//

// Let's create a few tensors to work with:
var t = Tensor.CreateFromFunction((3, 4), (i, j) => 11 + 10 * i + j);
// t -> [ [ 11, 12, 13, 14 ],
//        [ 21, 22, 23, 24 ],
//        [ 31, 32, 33, 34 ] ]

// Actually, let's use something a little bigger:
t = Tensor.CreateFromFunction((3, 4, 5), (i, j, k) => 100 * i + 10 * j + k);

// Tensors have indexer properties which get or set all or part
// of a tensor, including individual values.

// Important: All indexers return Tensor<T> objects,
// even if it contains just a single element!
var t123 = t[1, 2, 3];
// t123 -> [ 123 ]
Console.WriteLine($"Type of t[1, 2, 3] -> {t123.GetType()}");

// As of C# 8.0, you can use the caret (^) operator to
// specify that the index should be counted from the end
// of the dimension:
var t_2_2_2 = t[^2, ^2, ^2];
// t_2_2_2 -> [ 123 ]

// Single values can be set using the indexer, but you
// have to assign a scalar tensor:
t[1, 2, 3] = Tensor.CreateScalar(999);
t123 = t[1, 2, 3];
// t123 -> [ 999 ]

// To get an element's value, and not a scalar tensor,
// use the GetValue method:
var tValue = t.GetValue(1, 2, 3);
// tValue -> [ 999 ]
// A corresponding SetValue method lets you set the value:
t.SetValue(99, 1, 2, 3);
tValue = t.GetValue(1, 2, 3);
// tValue -> [ 99 ]

// When you leave out dimensions, the entire dimensions
// are returned:
var t12x = t[1, 2];
// t12x -> [ 120, 121, 122, 999, 124 ]

// You can use ranges and slices to get or set sub-tensors.
// You can use either Range or System.Range:
var r12 = new Range(1, 2);
var trrr = t[r12, r12, r12];
// trrr -> [[[ 111, 112], [121, 122]], [211, 212], [221, 222]]]
trrr = t[1..3, 1..3, 1..3];

// You can mix and match:
var s = Tensor.CreateFromFunction((3, 3), (i, j) => 11 + 10 * i + j);
// s -> [[ 11 12 13 ]
//       [ 21 22 23 ]
//       [ 31 32 33 ]]
var row1 = s[0, ..];
// row1 -> [ 11 12 13 ]
var column1 = s[.., ^2];
// column1 -> [ 12 22 32 ]
var row2 = s[1, 1..];
// row2 -> [ 22 23 ]

// C#'s ranges do not support strides. For that, you have to use
// either Range or Numerics.NET.Slice:
var row3 = s[1, new Range(0, 2, 2)];
// row3 -> [ 21 23 ]
row3 = s[1, new Numerics.NET.Slice(2, 0, 2)];
// row3 -> [ 21 23 ]

// You can even have ranges with negative strides:
var x = Tensor.CreateRange(3);
// x -> [ 0 1 2 ]
var reverse = x[new Range(2, 0, -1)];
// reverse -> [ 2 1 0 ]
reverse = x[new Numerics.NET.Slice(2, 2, -1)];
// reverse -> [ 2 1 0 ]

// You can set values using ranges and slices:
s[1, 1..3] = Tensor.CreateFromArray(new[] { 88, 99 });
// s -> [[ 11 12 13 ]
//       [ 21 88 99 ]
//       [ 31 32 33 ]]
s[..^1, ^2] = Tensor.CreateFromArray(new[] { 1, 2 });
// s -> [[ 11  1 13 ]
//       [ 21  2 99 ]
//       [ 31 32 33 ]]
s[1, new Range(0, 2, 2)] = Tensor.CreateFromArray(new[] { 77, 66 });
// s -> [[ 11  1 13 ]
//       [ 77  2 66 ]
//       [ 31 32 33 ]]

//
// Advanced indexes:
//

// You can use sets of integers to specify only those elements:
int[] indexes = { 0, 3 };
var t1 = t[1, indexes, 3..5];
// t1 -> [[ 103 133 ]
//        [ 104 134 ]]

// You can also use a mask, an array of booleans, that are true
// for the elements you want to select:
bool[] mask = { true, false, false, true };
var t2 = t[1, mask, 3..5];
// t2 -> [[ 103 133 ]
//        [ 104 134 ]]
//
// Copying and cloning tensors
//

// A shallow copy of a tensor constructs a tensor
// that shares the component storage with the original.
// This is done using an indexer:
Console.WriteLine("Shallow copy vs. clone:");
var t10 = t2[TensorIndex.All];
// The Copy method creates a full copy.
var t11 = t2.Copy();
// When we change t2, t10 changes, but t11 is left
// unchanged:
Console.WriteLine($"t2[1,1] = {t2[1, 1]}");
t2.SetValue(-2, 1, 1);
Console.WriteLine($"t10[1,1] = {t10[1, 1]}");
Console.WriteLine($"t11[1,1] = {t11[1, 1]}");

