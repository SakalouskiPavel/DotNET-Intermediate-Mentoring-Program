using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            var stopWatch = new Stopwatch();

            // Test time for 3x3 matrices.
            stopWatch.Start();
            TestMatrix3On3(new MatricesMultiplier());
            stopWatch.Stop();

            var firstTime = stopWatch.ElapsedTicks;

            stopWatch.Reset();

            stopWatch.Start();
            TestMatrix3On3(new MatricesMultiplierParallel());
            stopWatch.Stop();

            var secondTime = stopWatch.ElapsedTicks;

            // Not-parallelized method should be faster for small matrices. 
            Assert.IsTrue(firstTime < secondTime);

            stopWatch.Reset();

            // Test time for 200x200 matrices.
            stopWatch.Start();
            TestMatrix200On200(new MatricesMultiplier());
            stopWatch.Stop();

            firstTime = stopWatch.ElapsedTicks;

            stopWatch.Reset();

            stopWatch.Start();
            TestMatrix200On200(new MatricesMultiplierParallel());
            stopWatch.Stop();

            secondTime = stopWatch.ElapsedTicks;

            // Parallelized method should be faster for large matrices. 
            Assert.IsTrue(firstTime > secondTime);
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        void TestMatrix200On200(IMatricesMultiplier matrixMultiplier)
        {
            var m1 = new Matrix(200, 200);
            var m2 = new Matrix(200, 200);
            var rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    m1.SetElement(i, j, rand.Next(1, 100));
                    m2.SetElement(i, j, rand.Next(1, 100));
                }
            }

            var multiplied = matrixMultiplier.Multiply(m1, m2);
        }

        #endregion
    }
}
