using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var chests = new HashSet<Point>(map.Chests);
            var pathToExit = BfsTask.FindPaths(map, map.InitialPosition, new[] {map.Exit}).FirstOrDefault();
            if (pathToExit != null && pathToExit.Any(point => chests.Contains(point)))
                return MakeDirectionFromPath(pathToExit);

            var pathsFromStartToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests)
                .ToDictionary(path => path.Value, path => path.Previous);
            var pathsFromExitToChests = BfsTask.FindPaths(map, map.Exit, map.Chests)
                .ToDictionary(path => path.Value, path => path);
            
            var paths = pathsFromExitToChests
                .Where(path => pathsFromStartToChests.ContainsKey(path.Key))
                .Select(path =>
                    path.Value.Aggregate(pathsFromStartToChests[path.Key],
                        (previous, value) => new SinglyLinkedList<Point>(value, previous)));
            
            return MakeDirectionFromPath(paths.OrderBy(path => path.Length).FirstOrDefault() ?? pathToExit);
        }

        private static MoveDirection[] MakeDirectionFromPath(SinglyLinkedList<Point> path)
        {
            if (path == null)
                return new MoveDirection[0];
            var turns = new MoveDirection[path.Length - 1];
            for (var i = path.Length - 2; i >= 0; i--, path = path.Previous)
                turns[i] = Walker.ConvertOffsetToDirection(new Size(path.Value) - new Size(path.Previous.Value));
            return turns;
        }
    }
}