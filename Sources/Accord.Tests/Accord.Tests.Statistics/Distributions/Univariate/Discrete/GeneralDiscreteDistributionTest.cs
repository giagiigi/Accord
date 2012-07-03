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

using Accord.Statistics.Distributions.Univariate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accord.Math;
namespace Accord.Tests.Statistics
{


    /// <summary>
    ///This is a test class for DiscreteDistributionTest and is intended
    ///to contain all DiscreteDistributionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DiscreteDistributionTest
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
        public void FitTest()
        {
            GeneralDiscreteDistribution target = new GeneralDiscreteDistribution(4);
            double[] values = { 0.00, 1.00, 2.00, 3.00 };
            double[] weights = { 0.25, 0.25, 0.25, 0.25 };

            target.Fit(values, weights);

            double[] expected = { 0.25, 0.25, 0.25, 0.25 };
            double[] actual = target.Frequencies;

            Assert.IsTrue(Matrix.IsEqual(expected, actual));
        }

        [TestMethod()]
        public void FitTest2()
        {
            double[] expected = { 0.50, 0.00, 0.25, 0.25 };

            GeneralDiscreteDistribution target;

            double[] values = { 0.00, 2.00, 3.00 };
            double[] weights = { 0.50, 0.25, 0.25 };
            target = new GeneralDiscreteDistribution(4);
            target.Fit(values, weights);
            double[] actual = target.Frequencies;

            Assert.IsTrue(Matrix.IsEqual(expected, actual));

            // --

            double[] values2 = { 0.00, 0.00, 2.00, 3.00 };
            double[] weights2 = { 0.25, 0.25, 0.25, 0.25 };
            target = new GeneralDiscreteDistribution(4);
            target.Fit(values2, weights2);
            double[] actual2 = target.Frequencies;
            Assert.IsTrue(Matrix.IsEqual(expected, actual2));
        }


        [TestMethod()]
        public void DistributionFunctionTest()
        {
            var target = new GeneralDiscreteDistribution(0.1, 0.4, 0.5);

            double actual;

            actual = target.DistributionFunction(0);
            Assert.AreEqual(0.1, actual, 1e-6);

            actual = target.DistributionFunction(1);
            Assert.AreEqual(0.5, actual, 1e-6);

            actual = target.DistributionFunction(2);
            Assert.AreEqual(1.0, actual, 1e-6);

            actual = target.DistributionFunction(3);
            Assert.AreEqual(1.0, actual, 1e-6);


            Assert.AreEqual(1.3999999, target.Mean, 1e-6);
        }

        [TestMethod()]
        public void MeanTest()
        {
            var target = new GeneralDiscreteDistribution(0.1, 0.4, 0.5);
            double expected = 0 * 0.1 + 1 * 0.4 + 2 * 0.5;
            double actual = target.Mean;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MeanTest2()
        {
            var target = new GeneralDiscreteDistribution(42, 0.1, 0.4, 0.5);
            double expected = 42 * 0.1 + 43 * 0.4 + 44 * 0.5;
            double actual = target.Mean;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MeanTest3()
        {
            var target = new GeneralDiscreteDistribution(2, 0.5, 0.5);
            double expected = (2.0 + 3.0) / 2.0;
            double actual = target.Mean;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void VarianceTest()
        {
            var target = new GeneralDiscreteDistribution(42, 0.1, 0.4, 0.5);
            double mean = target.Mean;
            double expected = ((42 - mean) * (42 - mean) * 0.1
                             + (43 - mean) * (43 - mean) * 0.4
                             + (44 - mean) * (44 - mean) * 0.5);

            Assert.AreEqual(expected, target.Variance);
        }

        [TestMethod()]
        public void EntropyTest()
        {
            var target = new GeneralDiscreteDistribution(42, 0.1, 0.4, 0.5);
            double expected = -0.1 * System.Math.Log(0.1) +
                              -0.4 * System.Math.Log(0.4) +
                              -0.5 * System.Math.Log(0.5);

            Assert.AreEqual(expected, target.Entropy);
        }

        [TestMethod()]
        public void UniformTest()
        {
            int a = 2;
            int b = 5;
            int n = b - a + 1;

            // Wikipedia definitions
            double expectedMean = (a + b) / 2.0;
            double expectedVar = (System.Math.Pow(b - a + 1, 2) - 1) / 12.0;
            double p = 1.0 / n;


            GeneralDiscreteDistribution dist = GeneralDiscreteDistribution.Uniform(a, b);

            Assert.AreEqual(expectedMean, dist.Mean); ;
            Assert.AreEqual(expectedVar, dist.Variance);
            Assert.AreEqual(n, dist.Frequencies.Length);

            
            Assert.AreEqual(0, dist.ProbabilityMassFunction(0));
            Assert.AreEqual(0, dist.ProbabilityMassFunction(1));
            Assert.AreEqual(p, dist.ProbabilityMassFunction(2));
            Assert.AreEqual(p, dist.ProbabilityMassFunction(3));
            Assert.AreEqual(p, dist.ProbabilityMassFunction(4));
            Assert.AreEqual(p, dist.ProbabilityMassFunction(5));
            Assert.AreEqual(0, dist.ProbabilityMassFunction(6));
            Assert.AreEqual(0, dist.ProbabilityMassFunction(7));
        }

        [TestMethod()]
        public void ProbabilityMassFunctionTest()
        {
            GeneralDiscreteDistribution dist = GeneralDiscreteDistribution.Uniform(2, 5);
            double p = 0.25; 
            Assert.AreEqual(0, dist.ProbabilityMassFunction(0));
            Assert.AreEqual(0, dist.ProbabilityMassFunction(1));
            Assert.AreEqual(p, dist.ProbabilityMassFunction(2));
            Assert.AreEqual(p, dist.ProbabilityMassFunction(3));
            Assert.AreEqual(p, dist.ProbabilityMassFunction(4));
            Assert.AreEqual(p, dist.ProbabilityMassFunction(5));
            Assert.AreEqual(0, dist.ProbabilityMassFunction(6));
            Assert.AreEqual(0, dist.ProbabilityMassFunction(7));
        }

        [TestMethod()]
        public void LogProbabilityMassFunctionTest()
        {
            GeneralDiscreteDistribution dist = GeneralDiscreteDistribution.Uniform(2, 5);
            
            double p = System.Math.Log(0.25);
            double l = System.Math.Log(0);

            Assert.AreEqual(l, dist.LogProbabilityMassFunction(0));
            Assert.AreEqual(l, dist.LogProbabilityMassFunction(1));
            Assert.AreEqual(p, dist.LogProbabilityMassFunction(2));
            Assert.AreEqual(p, dist.LogProbabilityMassFunction(3));
            Assert.AreEqual(p, dist.LogProbabilityMassFunction(4));
            Assert.AreEqual(p, dist.LogProbabilityMassFunction(5));
            Assert.AreEqual(l, dist.LogProbabilityMassFunction(6));
            Assert.AreEqual(l, dist.LogProbabilityMassFunction(7));
        }
    }
}
