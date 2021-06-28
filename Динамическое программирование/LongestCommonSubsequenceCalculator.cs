using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var opt = new int[first.Count + 1, second.Count + 1];

            for (var i = 1; i <= first.Count; i++)
            for (var j = 1; j <= second.Count; j++)
                opt[i, j] = first[i - 1] == second[j - 1]
                    ? opt[i - 1, j - 1] + 1
                    : Math.Max(opt[i - 1, j], opt[i, j - 1]);

            return opt;
        }

        private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var result = new List<string>();
            var (i, j) = (first.Count, second.Count);
            while (i != 0 && j != 0)
            {
                if (i - 1 >= 0 && opt[i - 1, j] == opt[i, j])
                    i--;
                else if (j - 1 >= 0 && opt[i, j - 1] == opt[i, j])
                    j--;
                else
                {
                    i--;
                    j--;
                    result.Add(first[i]);
                }
            }
            result.Reverse();
            return result;
        }
    }
}