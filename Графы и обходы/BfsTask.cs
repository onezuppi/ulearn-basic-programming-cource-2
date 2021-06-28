using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var visitedPoints = new HashSet<Point>(){start};
            var hashChests = chests.ToHashSet();
            var queue = new Queue<SinglyLinkedList<Point>>(new[] {new SinglyLinkedList<Point>(start)});

            while (queue.Count != 0)
            {
                var path = queue.Dequeue();
                foreach (var nextPoint in GetNextPoints(map, path.Value, visitedPoints))
                {
                    var newPath = new SinglyLinkedList<Point>(nextPoint, path);
                    visitedPoints.Add(nextPoint);
                    queue.Enqueue(newPath);
                    if (hashChests.Contains(nextPoint))
                        yield return newPath;
                }
            }
        }

        private static IEnumerable<Point> GetNextPoints(Map map, Point current, HashSet<Point> visitedPoints) =>
            Walker.PossibleDirections
                .Select(delta => current + delta)
                .Where(map.InBounds)
                .Where(point => map.Dungeon[point.X, point.Y] == MapCell.Empty)
                .Where(point => !visitedPoints.Contains(point));
    }
}