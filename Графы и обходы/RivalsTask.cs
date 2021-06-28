using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Rivals
{
    public class RivalsTask
    {
        private static readonly (int X, int Y)[] Delta = {(-1, 0), (0, -1), (0, 1), (1, 0)};

        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var minDistances = Enumerable.Range(0, map.Players.Length).ToDictionary(x => map.Players[x], x => 0);
            var movesQueue = new Queue<(int Number, Point Player)>(Enumerable.Range(0, map.Players.Length)
                .Select(x => (x, map.Players[x])));

            while (movesQueue.Count != 0)
            {
                var (number, player) = movesQueue.Dequeue();

                yield return new OwnedLocation(number, player, minDistances[player]);

                var neighbors = Delta
                    .Select(delta => new Point(delta.X + player.X, delta.Y + player.Y))
                    .Where(map.InBounds)
                    .Where(point => map.Maze[point.X, point.Y] == MapCell.Empty)
                    .Where(point => !minDistances.ContainsKey(point));
                foreach (var neighbor in neighbors)
                {
                    movesQueue.Enqueue((number, neighbor));
                    minDistances[neighbor] = minDistances[player] + 1;
                }
            }
        }
    }
}