// SearchResult.cs
namespace Backend;

public class SearchResult
{
    public bool Found { get; }
    public IReadOnlyList<Position> Path { get; }
    public int CellsExplored { get; }

    // Number of moves = cells on the path minus the starting one.
    public int Steps => Found ? Path.Count - 1 : -1;

    private SearchResult(bool found, IReadOnlyList<Position> path, int cellsExplored)
    {
        Found = found;
        Path = path;
        CellsExplored = cellsExplored;
    }

    public static SearchResult Success(IReadOnlyList<Position> path, int cellsExplored)
        => new(true, path, cellsExplored);

    public static SearchResult NoPath(int cellsExplored)
        => new(false, Array.Empty<Position>(), cellsExplored);
}