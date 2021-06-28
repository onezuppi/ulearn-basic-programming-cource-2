using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;
using System.Drawing;

namespace Greedy
{
    public class DijkstraPathFinder
    {
        private static readonly (int X, int Y)[] Delta = {(-1, 0), (0, -1), (1, 0), (0, 1)};

        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
            var notVisited = new HashSet<Point>(new[] {start});
            var data = new Dictionary<Point, (int Distance, Point CameFrom)> {{start, (0, start)}};
            while (notVisited.Count != 0)
            {
                var bestPoint =
                    notVisited.First(x => data[x].Distance == notVisited.Min(point => data[point].Distance));
                var targetsHashes = targets.ToHashSet();
                notVisited.Remove(bestPoint);
                
                if (targetsHashes.Contains(bestPoint))
                    yield return new PathWithCost(data[bestPoint].Distance, RestorePath(start, bestPoint, data));
                
                foreach (var point in GetPointsWithBestDistances(state, bestPoint, data))
                {
                    data[point] = (data[bestPoint].Distance + state.CellCost[point.X, point.Y], bestPoint);
                    notVisited.Add(point);
                }
            }
        }

        private static IEnumerable<Point> GetPointsWithBestDistances(State state, Point current,
            IReadOnlyDictionary<Point, (int Distance, Point CameFrom)> data) =>
            Delta.Select(delta => new Point(current.X + delta.X, current.Y + delta.Y))
                .Where(state.InsideMap)
                .Where(point => !state.IsWallAt(point))
                .Where(point => !data.TryGetValue(point, out var oldPointInfo)
                                || data[current].Distance + state.CellCost[point.X, point.Y] < oldPointInfo.Distance);


        private static Point[] RestorePath(Point start, Point finish,
            IReadOnlyDictionary<Point, (int Distance, Point CameFrom)> data)
        {
            var path = new List<Point>();
            while (finish != start)
            {
                path.Add(finish);
                finish = data[finish].CameFrom;
            }

            path.Add(start);
            path.Reverse();
            
            return path.ToArray();
        }
    }
}