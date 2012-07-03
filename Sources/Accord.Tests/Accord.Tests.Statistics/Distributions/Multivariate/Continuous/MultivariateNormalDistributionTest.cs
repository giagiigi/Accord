﻿// Accord Unit Tests
// The Accord.NET Framework
// http://accord.googlecode.com
//
// Copyright © César Souza, 2009-2012
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

using Accord.Statistics.Distributions.Multivariate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accord.Math;
using Accord.Statistics.Distributions.Fitting;
namespace Accord.Tests.Statistics
{


    /// <summary>
    ///This is a test class for MultivariateNormalDistributionTest and is intended
    ///to contain all MultivariateNormalDistributionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MultivariateNormalDistributionTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void ProbabilityDensityFunctionTest()
        {
            double[] mean = { 1, -1 };
            double[,] covariance = 
            {
                { 0.9, 0.4 },
                { 0.4, 0.3 },
            };

            var target = new MultivariateNormalDistribution(mean, covariance);

            double[] x = { 1.2, -0.8 };
            double expected = 0.446209421363460;
            double actual = target.ProbabilityDensityFunction(x);

            Assert.AreEqual(expected, actual, 0.00000001);
        }

        [TestMethod()]
        public void LogProbabilityDensityFunctionTest()
        {
            double[] mean = { 1, -1 };
            double[,] covariance = 
            {
                { 0.9, 0.4 },
                { 0.4, 0.3 },
            };

            var target = new MultivariateNormalDistribution(mean, covariance);

            double[] x = { 1.2, -0.8 };
            double expected = System.Math.Log(0.446209421363460);
            double actual = target.LogProbabilityDensityFunction(x);

            Assert.AreEqual(expected, actual, 0.00000001);
        }

        [TestMethod()]
        public void ProbabilityDensityFunctionTest2()
        {
            double[] mean = new double[64];
            double[,] covariance = Accord.Tests.Math.CholeskyDecompositionTest.bigmatrix;

            var target = new MultivariateNormalDistribution(mean, covariance);

            double expected = 1.0;
            double actual = target.ProbabilityDensityFunction(mean);

            Assert.AreEqual(expected, actual, 0.00000001);

            double[] x = Matrix.Diagonal(covariance).Multiply(1.5945e7);

            expected = 4.781042576287362e-12;
            actual = target.ProbabilityDensityFunction(x);

            Assert.AreEqual(expected, actual, 1e-21);
        }

        [TestMethod()]
        public void ProbabilityDensityFunctionTest3()
        {
            double[] mean = new double[3];
            double[,] covariance = Matrix.Identity(3);

            var target = new MultivariateNormalDistribution(mean, covariance);

            double[] x = { 1.2, -0.8 };

            bool thrown = false;
            try
            {
                target.ProbabilityDensityFunction(x);
            }
            catch (DimensionMismatchException)
            {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }


        [TestMethod()]
        public void ConstructorTest()
        {
            double[] mean = { 1, -1 };
            double[,] covariance = 
            {
                { 2, 1 },
                { 1, 3 }
            };

            MultivariateNormalDistribution target = new MultivariateNormalDistribution(mean, covariance);

            Assert.AreEqual(covariance, target.Covariance);
            Assert.AreEqual(mean, target.Mean);
            Assert.AreEqual(2, target.Variance.Length);
            Assert.AreEqual(2, target.Variance[0]);
            Assert.AreEqual(3, target.Variance[1]);
            Assert.AreEqual(2, target.Dimension);
        }

        [TestMethod()]
        public void ConstructorTest2()
        {
            double[] mean = { 1, -1 };
            double[,] covariance = Matrix.Identity(4);

            bool thrown = false;

            try { new MultivariateNormalDistribution(mean, covariance); }
            catch (DimensionMismatchException) { thrown = true; }

            Assert.IsTrue(thrown);
        }

        [TestMethod()]
        public void ConstructorTest3()
        {
            double[] mean = { 0, 0 };
            double[,] covariance = 
            {
                { 0, 1 },
                { 1, 0 }
            };

            bool thrown = false;

            try { new MultivariateNormalDistribution(mean, covariance); }
            catch (NonPositiveDefiniteMatrixException) { thrown = true; }

            Assert.IsTrue(thrown);
        }


        [TestMethod()]
        public void FitTest()
        {
            double[][] observations = 
            {
                new double[] { 0.1000, -0.2000 },
                new double[] { 0.4000,  0.6000 },
                new double[] { 2.0000,  0.2000 },
                new double[] { 2.0000,  0.3000 }
            };

            double[] mean = Accord.Statistics.Tools.Mean(observations);
            double[,] cov = Accord.Statistics.Tools.Covariance(observations);

            var target = new MultivariateNormalDistribution(2);

            double[] weigths = { 0.25, 0.25, 0.25, 0.25 };

            target.Fit(observations, weigths);

            Assert.IsTrue(Matrix.IsEqual(mean, target.Mean));
            Assert.IsTrue(Matrix.IsEqual(cov, target.Covariance));
        }

        [TestMethod()]
        public void FitTest2()
        {
            double[][] observations = 
            {
                new double[] { 1, 2 },
                new double[] { 1, 2 },
                new double[] { 1, 2 },
                new double[] { 1, 2 }
            };


            var target = new MultivariateNormalDistribution(2);

            bool thrown = false;
            try { target.Fit(observations); }
            catch (NonPositiveDefiniteMatrixException) { thrown = true; }

            Assert.IsTrue(thrown);

            NormalOptions options = new NormalOptions() { Regularization = double.Epsilon };

            // No exception thrown
            target.Fit(observations, options);
        }

    }
}
