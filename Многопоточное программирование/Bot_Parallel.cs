using System;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            var moves = new Tuple<Turn, double>[threadsCount];
            Parallel.For(0, threadsCount,
                index =>
                {
                    moves[index] = SearchBestMove(rocket, new Random(random.Next()), iterationsCount / threadsCount);
                });
            return rocket.Move(moves.OrderBy(x => x.Item2).Last().Item1, level);
        }
    }
}