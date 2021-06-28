using System;
using System.Collections.Generic;
using System.Linq;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            return Enumerable.Range(0, documents.Count)
                .SelectMany(i => Enumerable.Range(i + 1, documents.Count - i - 1)
                    .Select(j => GetComparisonResult(documents[i], documents[j])))
                .ToList();
        }

        private static ComparisonResult GetComparisonResult(DocumentTokens document1, DocumentTokens document2)
        {
            var matrix = new double[document1.Count + 1, document2.Count + 1];
            for (var i = 1; i <= document1.Count; i++)
                matrix[i, 0] = i;
            for (var i = 1; i <= document2.Count; i++)
                matrix[0, i] = i;

            for (var i = 1; i <= document1.Count; i++)
            for (var j = 1; j <= document2.Count; j++)
            {
                matrix[i, j] = Math.Min(matrix[i - 1, j], matrix[i, j - 1]);
                matrix[i, j] = Math.Min(matrix[i, j] + 1,
                    TokenDistanceCalculator.GetTokenDistance(document1[i - 1], document2[j - 1]) +
                    matrix[i - 1, j - 1]);
            }

            return new ComparisonResult(document1, document2, matrix[document1.Count, document2.Count]);
        }
    }
}