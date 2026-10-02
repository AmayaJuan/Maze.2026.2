// Maze.cs
namespace Backend;

public class Maze
{
    public const char Wall = '#';
    public const char Open = '.';
    public const char Start = 'S';
    public const char Exit = 'E';

    private readonly char[,] _cells;

    public int Rows { get; }
    public int Columns { get; }
    public Position StartPosition { get; }
    public Position ExitPosition { get; }

    // The four allowed moves: up, down, left, right.
    private static readonly (int RowOffset, int ColumnOffset)[] Directions =
    {
        (-1, 0), (1, 0), (0, -1), (0, 1)
    };

    public Maze(string[] lines)
    {
        if (lines is null || lines.Length == 0)
            throw new ArgumentException("The maze cannot be empty.");

        Rows = lines.Length;
        Columns = lines[0].Length;
        _cells = new char[Rows, Columns];

        Position? start = null;
        Position? exit = null;

        for (int r = 0; r < Rows; r++)
        {
            if (lines[r].Length != Columns)
                throw new ArgumentException($"Row {r} does not have {Columns} columns.");

            for (int c = 0; c < Columns; c++)
            {
                char ch = lines[r][c];
                _cells[r, c] = ch;

                if (ch == Start) start = new Position(r, c);
                if (ch == Exit) exit = new Position(r, c);
            }
        }

        StartPosition = start ?? throw new ArgumentException("Missing start cell 'S'.");
        ExitPosition = exit ?? throw new ArgumentException("Missing exit cell 'E'.");
    }

    public bool IsInside(Position p)
        => p.Row >= 0 && p.Row < Rows && p.Column >= 0 && p.Column < Columns;

    public bool IsWalkable(Position p)
        => IsInside(p) && _cells[p.Row, p.Column] != Wall;

    public char CellAt(Position p) => _cells[p.Row, p.Column];

    // Yields only the neighbours the player can actually move to.
    public IEnumerable<Position> GetNeighbours(Position p)
    {
        foreach (var (rowOffset, columnOffset) in Directions)
        {
            var neighbour = p.Move(rowOffset, columnOffset);
            if (IsWalkable(neighbour))
                yield return neighbour;
        }
    }

    // Renders the maze, marking the cells of the given path with '*'.
    public string Render(IReadOnlyCollection<Position>? path = null)
    {
        var onPath = path is null ? new HashSet<Position>() : new HashSet<Position>(path);
        var sb = new System.Text.StringBuilder();

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                var p = new Position(r, c);
                char ch = _cells[r, c];

                bool isPathCell = onPath.Contains(p) && ch == Open;
                sb.Append(isPathCell ? '*' : ch);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}