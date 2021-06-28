using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class NotGreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var pathsBetweenChests = GetAllPathsBetweenChests(state);
            var bestPath = new List<Point>();

            for (var i = state.Chests.Count; i > 0; i--)
            {
                bestPath = GetPermutations(state.Chests, i, state.Position, pathsBetweenChests, state.Energy)
                    .FirstOrDefault()
                    ?.ToList();

                if (bestPath != null && bestPath.Count > 0)
                    break;
            }

            return GetTotalPathByPermutation(state.Position, bestPath, pathsBetweenChests);
        }

        private Dictionary<Point, Dictionary<Point, PathWithCost>> GetAllPathsBetweenChests(State state)
        {
            var pathFinder = new DijkstraPathFinder();
            var pathsBetweenChests = new Dictionary<Point, Dictionary<Point, PathWithCost>>
            {
                [state.Position] = pathFinder.GetPathsByDijkstra(state, state.Position, state.Chests)
                    .ToDictionary(path => path.End, path => path)
            };

            foreach (var chest in state.Chests)
                pathsBetweenChests[chest] = pathFinder
                    .GetPathsByDijkstra(state, chest, state.Chests.Where(c => c != chest))
                    .ToDictionary(path => path.End, path => path);

            return pathsBetweenChests;
        }
        
        private IEnumerable<IEnumerable<Point>> GetPermutations(List<Point> list, int length, Point start,
            Dictionary<Point, Dictionary<Point, PathWithCost>> pathsBetweenChests, int maxEnergy)
        {
            if (length == 1) 
                return list.Select(t => new[] {t});
            return GetPermutations(list, length - 1, start, pathsBetweenChests, maxEnergy)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new[] {t2}))
                .Where(perm => GetPathCost(start, perm, pathsBetweenChests) <= maxEnergy);
        }

        private int GetPathCost(Point pointFrom, IEnumerable<Point> path,
            IReadOnlyDictionary<Point, Dictionary<Point, PathWithCost>> pathsBetweenChests)
        {
            var cost = 0;
            foreach (var pointTo in path)
                (cost, pointFrom) = (cost + pathsBetweenChests[pointFrom][pointTo].Cost, pointTo);
            return cost;
        }

        private List<Point> GetTotalPathByPermutation(Point pointFrom, List<Point> pointsPermutation,
            Dictionary<Point, Dictionary<Point, PathWithCost>> pathsBetweenChests)
        {
            var result = new List<Point>();
            foreach (var pointTo in pointsPermutation)
            {
                result.AddRange(pathsBetweenChests[pointFrom][pointTo].Path.Skip(1));
                pointFrom = pointTo;
            }

            return result;
        }
    }
}