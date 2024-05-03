// Create a C# console application that accepts a matrix of values 0 and 1. The application should
// output only one value into the console – number of areas formed of number 1.
// The matrix is presented as a string value where ‘,’ is used as a separator for columns, ‘;’ is used as
// a separator for rows. For instance, “1,0,1;0,1,0” string value should be converted to the matrix
// [[1,0,1], [0,1,0]].
// The maximum size of the matrix is 100x100.

using System.Text.RegularExpressions;

while (true)
{
    Console.WriteLine("Enter a matrix in the format '1,0,1;0,1,0', or 'exit' to quit:");
    var input = Console.ReadLine();

    if (input.ToLower() == "exit")
    {
        Console.WriteLine("Exiting the program.");
        break;
    }

    try
    {
        // Convert the input string into a 2D array
        var matrix = ParseMatrix(input);
        PrintMatrix(matrix);

        // Count the number of connected areas
        var algorithmType = GetAlgorithmType();
        int areaCount = CountAreas(matrix, algorithmType);


        // Output the result
        Console.WriteLine($"Number of connected areas: {areaCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Invalid input: {ex.Message}");
        Console.WriteLine();
    }
}

return;

static AlgorithmType GetAlgorithmType()
{
    while (true)
    {
        Console.WriteLine("Choose algorithm (1 - BFS; 2 - DFS):");
        var input = Console.ReadLine();

        try
        {
            if (Enum.TryParse<AlgorithmType>(input, out var algorithmType))
            {
                return algorithmType;
            }

            throw new ArgumentException($"{input} not supported");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid algorithm selection: {ex.Message}");
            Console.WriteLine();
        }
    }
}

static bool[,] ParseMatrix(string input)
{
    if (!ValidStringRegex().IsMatch(input))
    {
        throw new ArgumentException("Invalid input. It should consist only of [01,;]. Example: '1,0,1;0,1,0'");
    }
    
    var rows = input.Split(';');
    var matrix = rows.Select(x => x.Split(',')).ToArray();

    if (matrix.Any(row => row.Length != matrix[0].Length))
    {
        throw new ArgumentException("Invalid input. All rows must be of equal size.");
    }

    var allowedCharacters = new[] { "1", "0" };
    if (matrix.Any(row => row.Any(ch => !allowedCharacters.Contains(ch))))
    {
        throw new ArgumentException("Invalid input. Values can consist only of \"1\" or \"0\"");
    }

    var result = new bool[matrix.Length, matrix[0].Length];
    for (var i = 0; i < result.GetLength(0); i++)
    for (var j = 0; j < result.GetLength(1); j++)
    {
        result[i, j] = matrix[i][j] == "1";
    }

    return result;
}

static void PrintMatrix(bool[,] matrix)
{
    var n = matrix.GetLength(0);
    var m = matrix.GetLength(1);

    for (var i = 0; i < n; i++)
    {
        Console.Write("[");
            
        for (int j = 0; j < m; j++)
        {
            Console.Write(matrix[i, j] ? "1" : "0");

            if (j < m - 1)
            {
                Console.Write(",");
            }
        }
            
        Console.WriteLine("]");
    }
}

static int CountAreas(bool[,] matrix, AlgorithmType algorithmType)
{
    Action<bool[,], bool[,], int, int> action = algorithmType switch
    {
        AlgorithmType.Bfs => Bfs,
        AlgorithmType.Dfs => Dfs
    };
    
    var m = matrix.GetLength(0);
    var n = matrix.GetLength(1);
    
    var visited = new bool[m, n];
    var c = 0;
    for(var i = 0; i < m; i++) {
        for(var j = 0; j < n; j++) {
            if (matrix[i, j] && !visited[i, j]) {
                action(matrix, visited, i, j);
                c++;
            }
        }
    }
    
    return c;
}

static void Dfs(bool[,] matrix, bool[,] visited, int i, int j) {
    if (visited[i, j] || !matrix[i, j]) return;

    visited[i,j] = true;

    if (i + 1 < matrix.GetLength(0)) Dfs(matrix, visited, i+1, j);
    if (j + 1 < matrix.GetLength(1)) Dfs(matrix, visited, i, j+1);
    if (i - 1 >= 0) Dfs(matrix, visited, i-1, j);
    if (j - 1 >= 0) Dfs(matrix, visited, i, j-1);
}

static void Bfs(bool[,] matrix, bool[,] visited, int i, int j) {
    var m = matrix.GetLength(0);
    var n = matrix.GetLength(1);
    var queue = new Queue<Point>();
    queue.Enqueue(new Point(i, j));
    while (queue.Count > 0) {
        var cur = queue.Dequeue();

        if (matrix[cur.X, cur.Y] && !visited[cur.X, cur.Y]) {
            visited[cur.X, cur.Y] = true;
            if (cur.X + 1 < m)
                queue.Enqueue(cur with { X = cur.X + 1 });
            if (cur.Y + 1 < n)
                queue.Enqueue(cur with { Y = cur.Y + 1 });
            if (cur.X - 1 >= 0)
                queue.Enqueue(cur with { X = cur.X - 1 });
            if (cur.Y - 1 >= 0)
                queue.Enqueue(cur with { Y = cur.Y - 1 });
        }
    }
}

internal record Point(int X, int Y);

enum AlgorithmType
{
    Bfs = 1,
    Dfs = 2
}

partial class Program
{
    [GeneratedRegex("^[01,;]+$")]
    private static partial Regex ValidStringRegex();
}