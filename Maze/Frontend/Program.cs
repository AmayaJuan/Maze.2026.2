// Program.cs
using Backend;

// Example 1: small maze, good for tracing the queue by hand.
string[] smallMaze =
{
    "S.#....",
    ".##.##.",
    "....#..",
    "###.#.#",
    "......E",
};

// Example 2: larger maze with loops, dead ends and several routes to the exit.
string[] largeMaze =
{
    "S...#.........#......",
    ".##.#.#######.#.####.",
    ".#..#.#.....#.#.#....",
    ".#.##.#.###.#.#.#.###",
    ".#....#.#...#...#...#",
    ".####.#.#.#####.###.#",
    "......#.........#...#",
    ".#####.#.#######.#.##",
    ".#.....#.....#...#...",
    ".#.#########.#.#####.",
    "...#................E",
};

Solve("Example 1 - small maze", smallMaze);
Solve("Example 2 - large maze", largeMaze);

static void Solve(string title, string[] lines)
{
    Console.WriteLine($"=== {title} ===");

    var maze = new Maze(lines);
    var finder = new BfsPathFinder(maze);

    Console.WriteLine("Original maze:");
    Console.WriteLine(maze.Render());

    SearchResult result = finder.FindShortestPath();

    if (!result.Found)
    {
        Console.WriteLine("There is no path between S and E.");
        Console.WriteLine();
        return;
    }

    Console.WriteLine($"Shortest path found: {result.Steps} steps");
    Console.WriteLine($"Cells explored: {result.CellsExplored} of {maze.Rows * maze.Columns}");
    Console.WriteLine("Sequence: " + string.Join(" -> ", result.Path));
    Console.WriteLine();
    Console.WriteLine("Maze with the path marked (*):");
    Console.WriteLine(maze.Render(result.Path));
}