// BfsPathFinder.cs
namespace Backend;

public class BfsPathFinder
{
    private readonly Maze _maze;

    public BfsPathFinder(Maze maze)
    {
        _maze = maze ?? throw new ArgumentNullException(nameof(maze));
    }

    public SearchResult FindShortestPath()
    {
        var start = _maze.StartPosition;
        var exit = _maze.ExitPosition;

        // 1) The QUEUE: cells waiting to be explored, in discovery order (FIFO).
        var queue = new Queue<Position>();

        // 2) Cells already discovered. Prevents cycles and repeated work.
        var visited = new HashSet<Position>();

        // 3) For each cell, the cell we came from. Lets us rebuild the path at the end.
        var parent = new Dictionary<Position, Position>();

        queue.Enqueue(start);
        visited.Add(start);

        int cellsExplored = 0;

        while (queue.Count > 0)
        {
            Position current = queue.Dequeue();   // FIFO: the oldest cell comes out first
            cellsExplored++;

            if (current == exit)
                return SearchResult.Success(RebuildPath(parent, start, exit), cellsExplored);

            foreach (Position neighbour in _maze.GetNeighbours(current))
            {
                if (visited.Contains(neighbour))
                    continue;

                visited.Add(neighbour);          // mark when ENQUEUED, not when dequeued
                parent[neighbour] = current;
                queue.Enqueue(neighbour);        // discovered -> goes to the back of the queue
            }
        }

        return SearchResult.NoPath(cellsExplored);
    }

    // Walks the parent map backwards from the exit to the start, then reverses it.
    private static IReadOnlyList<Position> RebuildPath(
        Dictionary<Position, Position> parent, Position start, Position exit)
    {
        var path = new List<Position>();
        var current = exit;

        while (current != start)
        {
            path.Add(current);
            current = parent[current];
        }
        path.Add(start);
        path.Reverse();

        return path;
    }
}