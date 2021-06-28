using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        private static readonly DijkstraPathFinder PathFinder = new DijkstraPathFinder();

        public List<Point> FindPathToCompleteGoal(State state)
        {
            if (state.Chests.Count < state.Goal)
                return new List<Point>();

            var resultPath = new List<Point>();
            var chests = new HashSet<Point>(state.Chests);
            var (energy, position, goal) = (state.InitialEnergy, state.Position, state.Goal);

            for (; goal > 0; goal--)
            {
                var pathWithCost = PathFinder.GetPathsByDijkstra(state, position, chests).FirstOrDefault();
                if (pathWithCost == null || energy < pathWithCost.Cost)
                    break;

                pathWithCost.Path.RemoveAt(0);
                resultPath.AddRange(pathWithCost.Path);
                energy -= pathWithCost.Cost;
                position = resultPath.Count != 0 ? resultPath.Last() : position;
                chests.Remove(position);
            }

            return resultPath;
        }
    }
}