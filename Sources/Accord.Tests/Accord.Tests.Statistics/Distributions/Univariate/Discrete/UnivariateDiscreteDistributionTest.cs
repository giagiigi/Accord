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
using Accord.Statistics.Distributions;
using System;
using Accord.Statistics.Distributions.Fitting;

namespace Accord.Tests.Statistics
{


    /// <summary>
    ///This is a test class for UnivariateDiscreteDistributionTest and is intended
    ///to contain all UnivariateDiscreteDistributionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UnivariateDiscreteDistributionTest
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


        internal virtual UnivariateDiscreteDistribution CreateUnivariateDiscreteDistribution()
        {
            double mean = 0.42;
            return new BernoulliDistribution(mean);
        }

        [TestMethod()]
        public void VarianceTest()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();
            double actual = target.Variance;
            double expected = 0.42 * (1.0 - 0.42);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void StandardDeviationTest()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();
            double actual = target.StandardDeviation;
            double expected = System.Math.Sqrt(0.42 * (1.0 - 0.42));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MeanTest()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();
            double actual = target.Mean;
            double expected = 0.42;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void EntropyTest()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();

            double q = 0.42;
            double p = 1 - q;

            double actual = target.Entropy;
            double expected = -q * System.Math.Log(q) - p * System.Math.Log(p);

            Assert.AreEqual(expected, actual);


            target.Fit(new double[] { 0, 1, 0, 0, 1, 0 });

            q = target.Mean;
            p = 1 - q;

            actual = target.Entropy;
            expected = -q * System.Math.Log(q) - p * System.Math.Log(p);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ProbabilityMassFunctionTest()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();

            double p = 0.42;
            double q = 1 - p;

            Assert.AreEqual(q, target.ProbabilityMassFunction(0));
            Assert.AreEqual(p, target.ProbabilityMassFunction(1));

            double[] observations = { 0, 1, 0, 0, 1, 0 };

            target.Fit(observations);

            p = target.Mean;
            q = 1 - p;

            Assert.AreEqual(q, target.ProbabilityMassFunction(0));
            Assert.AreEqual(p, target.ProbabilityMassFunction(1));
        }

        [TestMethod()]
        public void LogProbabilityMassFunctionTest()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();

            double p = 0.42;
            double q = 1 - p;

            double lnp = System.Math.Log(p);
            double lnq = System.Math.Log(q);

            Assert.AreEqual(lnq, target.LogProbabilityMassFunction(0));
            Assert.AreEqual(lnp, target.LogProbabilityMassFunction(1));
        }

        [TestMethod()]
        public void FitTest7()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();
            double[] observations = { 0, 1, 1, 1, 1 };
            double[] weights = { 0.125, 0.125, 0.25, 0.25, 0.25 };
            target.Fit(observations, weights);

            double mean = Accord.Statistics.Tools.WeightedMean(observations, weights);

            Assert.AreEqual(mean, target.Mean);
        }

        [TestMethod()]
        public void FitTest6()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();
            double[] observations = { 0, 1, 1, 1, 1 };
            target.Fit(observations);

            double mean = Accord.Statistics.Tools.Mean(observations);

            Assert.AreEqual(mean, target.Mean);
        }

        [TestMethod()]
        public void FitTest5()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();
            double[] observations = { 0, 1, 1, 1, 1 };
            double[] weights = { 0.125, 0.125, 0.25, 0.25, 0.25 };
            IFittingOptions options = null;

            target.Fit(observations, weights, options);

            double mean = Accord.Statistics.Tools.WeightedMean(observations, weights);

            Assert.AreEqual(mean, target.Mean);
        }

        [TestMethod()]
        public void FitTest4()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();
            double[] observations = { 0, 1, 1, 1, 1 };
            IFittingOptions options = null;

            target.Fit(observations, options);

            double mean = Accord.Statistics.Tools.Mean(observations);

            Assert.AreEqual(mean, target.Mean);
        }

        [TestMethod()]
        public void DistributionFunctionTest1()
        {
            UnivariateDiscreteDistribution target = CreateUnivariateDiscreteDistribution();

            double q = 1.0 - 0.42;

            Assert.AreEqual(0, target.DistributionFunction(-1));
            Assert.AreEqual(q, target.DistributionFunction(+0));
            Assert.AreEqual(1, target.DistributionFunction(+1));
            Assert.AreEqual(1, target.DistributionFunction(+2));
        }



        [TestMethod()]
        [DeploymentItem("Accord.Statistics.dll")]
        public void ProbabilityFunctionTest()
        {
            IDistribution target = CreateUnivariateDiscreteDistribution();

            double p = 0.42;
            double q = 1 - p;

            Assert.AreEqual(q, target.ProbabilityFunction(0));
            Assert.AreEqual(p, target.ProbabilityFunction(1));


            double[] observations = { 0, 1, 0, 0, 1, 0 };

            target.Fit(observations);

            p = Accord.Statistics.Tools.Mean(observations);
            q = 1 - p;

            Assert.AreEqual(q, target.ProbabilityFunction(0));
            Assert.AreEqual(p, target.ProbabilityFunction(1));
        }

        [TestMethod()]
        [DeploymentItem("Accord.Statistics.dll")]
        public void FitTest3()
        {
            IDistribution target = CreateUnivariateDiscreteDistribution();
            double[] observations = { 0, 1, 1, 1, 1 };
            target.Fit(observations);

            double mean = Accord.Statistics.Tools.Mean(observations);

            Assert.AreEqual(mean, (target as BernoulliDistribution).Mean);
        }

        [TestMethod()]
        [DeploymentItem("Accord.Statistics.dll")]
        public void FitTest2()
        {
            IDistribution target = CreateUnivariateDiscreteDistribution();
            double[] observations = { 0, 1, 1, 1, 1 };
            target.Fit(observations);

            double mean = Accord.Statistics.Tools.Mean(observations);

            Assert.AreEqual(mean, (target as BernoulliDistribution).Mean);
        }

        [TestMethod()]
        [DeploymentItem("Accord.Statistics.dll")]
        public void FitTest1()
        {
            IDistribution target = CreateUnivariateDiscreteDistribution();
            double[] observations = { 0, 1, 1, 1, 1 };
            double[] weights = { 0.125, 0.125, 0.25, 0.25, 0.25 };
            IFittingOptions options = null;

            target.Fit(observations, weights, options);

            double mean = Accord.Statistics.Tools.WeightedMean(observations, weights);

            Assert.AreEqual(mean, (target as BernoulliDistribution).Mean);
        }

        [TestMethod()]
        [DeploymentItem("Accord.Statistics.dll")]
        public void FitTest()
        {
            IDistribution target = CreateUnivariateDiscreteDistribution();
            double[] observations = { 0, 1, 1, 1, 1 };
            IFittingOptions options = null;

            target.Fit(observations, options);

            double mean = Accord.Statistics.Tools.Mean(observations);

            Assert.AreEqual(mean, (target as BernoulliDistribution).Mean);
        }

        [TestMethod()]
        [DeploymentItem("Accord.Statistics.dll")]
        public void DistributionFunctionTest()
        {
            IDistribution target = CreateUnivariateDiscreteDistribution();

            double q = 1.0 - 0.42;

            Assert.AreEqual(0, target.DistributionFunction(-1.0));
            Assert.AreEqual(q, target.DistributionFunction(+0.0));
            Assert.AreEqual(q, target.DistributionFunction(+0.5));
            Assert.AreEqual(1, target.DistributionFunction(+1.0));
            Assert.AreEqual(1, target.DistributionFunction(+1.1));
        }
    }
}
