using System.Threading.Tasks;
using MultiThreading.Task3.MatrixMultiplier.Matrices;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);

            Parallel.For(0, resultMatrix.RowCount,
                (i) =>
                {
                    for (int j = 0; j < m1.ColCount; j++)
                    {
                        long currentItem = 0;
                        for (int l = 0; l < m1.ColCount; l++)
                        {
                            currentItem += m1.GetElement(i, l) * m2.GetElement(l, j);
                        }

                        resultMatrix.SetElement(i, j, currentItem);
                    }
                });

            return resultMatrix;
        }
    }
}
