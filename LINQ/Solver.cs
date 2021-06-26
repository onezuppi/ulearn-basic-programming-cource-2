using System;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] matrix, double[] freeMembers)
        {
            var size = Math.Max(matrix.Length, matrix[0].Length);
            (matrix, freeMembers) = (ExtendMatrix(matrix, size), ExtendMatrix(freeMembers, size));
            for (var column = 0; column < size; column++)
            {
                var currentRow = -1;
                for (var r = column; r < size; r++)
                    if (currentRow == -1 || Math.Abs(matrix[r][column]) >= Math.Abs(matrix[currentRow][column]))
                        currentRow = r;
                if (currentRow != column)
                    SwapRows(matrix, freeMembers, currentRow, column);
                DivideRow(matrix, freeMembers, column, matrix[column][column] == 0 ? 1 : matrix[column][column]);
                for (var row = column + 1; row < size; row++)
                    CombineRows(matrix, freeMembers, row, column, -matrix[row][column]);
            }

            var result = new double[size];
            for (var i = size - 1; i >= 0; i--)
                result[i] = freeMembers[i] - result.Skip(i + 1).Select((t, ind) => matrix[i][i + ind + 1] * t).Sum();

            if (IsSolution(result, matrix, freeMembers))
                return result;
            throw new NoSolutionException("");
        }

        private T[][] ExtendMatrix<T>(T[][] matrix, int size) => Enumerable.Range(0, size).Select(i =>
            i < matrix.Length ? ExtendMatrix(matrix[i], size) : new T[size]).ToArray();

        private T[] ExtendMatrix<T>(T[] matrix, int size) =>
            Enumerable.Range(0, size).Select(x => x < matrix.Length ? matrix[x] : default).ToArray();


        private void SwapRows(double[][] matrix, double[] freeMembers, int row1, int row2)
        {
            (matrix[row1], matrix[row2]) = (matrix[row2], matrix[row1]);
            (freeMembers[row1], freeMembers[row2]) = (freeMembers[row2], freeMembers[row1]);
        }

        private void DivideRow(double[][] matrix, double[] freeMembers, int row, double divider)
        {
            matrix[row] = matrix[row].Select(x => x / divider).ToArray();
            freeMembers[row] /= divider;
        }


        private void CombineRows(double[][] matrix, double[] freeMembers, int row, int sourceRow, double weight)
        {
            matrix[row] = matrix[row].Select((x, i) => x + matrix[sourceRow][i] * weight).ToArray();
            freeMembers[row] += freeMembers[sourceRow] * weight;
        }

        private bool IsSolution(double[] actual, double[][] matrix, double[] freeMembers, double accuracy = 1e-3) =>
            matrix.Select((x, i) => freeMembers[i] - x.Select((y, j) => y * actual[j]).Sum())
                .All(x => Math.Abs(x) < accuracy);
    }
}