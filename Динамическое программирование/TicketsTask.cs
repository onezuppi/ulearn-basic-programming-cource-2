using System;
using System.Linq;
using System.Numerics;

namespace Tickets
{
    internal class TicketsTask
    {
        public static BigInteger Solve(int halfLen, int totalSum) =>
            totalSum % 2 != 0 ? 0 : CountLuckyTickets(halfLen, totalSum / 2 + 1);
        
        private static BigInteger CountLuckyTickets(int halfLen, int totalSum)
        {
            var table = GetLuckyTicketsTable(halfLen, totalSum);

            for (var i = 1; i < halfLen; i++)
            for (var j = 1; j < totalSum; j++)
                table[i, j] = Enumerable.Range(j - Math.Min(j, 9), Math.Min(j + 1, 10))
                    .Aggregate(BigInteger.Zero, (current, index) => table[i - 1, index] + current);

            return table[halfLen - 1, totalSum - 1] * table[halfLen - 1, totalSum - 1];
        }

        private static BigInteger[,] GetLuckyTicketsTable(int halfLen, int totalSum)
        {
            var table = new BigInteger[halfLen, totalSum];
            
            for (var i = 0; i < halfLen; i++)
                table[i, 0] = 1;
            for (var j = 0; j < Math.Min(totalSum, 10); j++)
                table[0, j] = 1;
            
            return table;
        }
    }
}