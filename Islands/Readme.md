# Number of connected areas
Create a C# console application that accepts a matrix of values 0 and 1. The application should output only one value into the console – number of areas formed of number 1. The matrix is presented as a string value where ‘,’ is used as a separator for columns, ‘;’ is used as a separator for rows. For instance, “1,0,1;0,1,0” string value should be converted to the matrix [[1,0,1], [0,1,0]].

The maximum size of the matrix is 100x100.

Examples of the input and output:
1. Input: “1,0,1;0,1,0”
   Output: 3
2. Input: “1,0,1;1,1,0”
   Output: 2
3. Input: “1,1,1,1,0;1,1,0,1,0;1,1,0,0,0;0,0,0,0,0”
   Output: 3

# Implementation

2 types of algorithms covered in the app:
- Depth first search
- Breadth first search

There is also 1 algorithm which is not covered, but can easily be added - UnionFind