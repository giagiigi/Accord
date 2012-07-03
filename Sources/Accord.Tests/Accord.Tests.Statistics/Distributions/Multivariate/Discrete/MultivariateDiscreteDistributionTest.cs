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
namespace Accord.Tests.Statistics
{
    
    
    /// <summary>
    ///This is a test class for MultivariateDiscreteDistributionTest and is intended
    ///to contain all MultivariateDiscreteDistributionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MultivariateDiscreteDistributionTest
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
        public void ProbabilityMassFunctionTest()
        {
            MultivariateDiscreteDistribution target = new MultinomialDistribution(5, 0.25, 0.25, 0.25, 0.25);
            
            int[] observation = { 1, 1, 1, 2 };

            double expected = 0.05859375;
            double actual = target.ProbabilityMassFunction(observation);

            Assert.AreEqual(expected, actual, 1e-6);
        }

        [TestMethod()]
        public void LogProbabilityMassFunctionTest()
        {
            MultivariateDiscreteDistribution target = new MultinomialDistribution(5, 0.25, 0.25, 0.25, 0.25);

            int[] observation = { 1, 1, 1, 2 };

            double expected = System.Math.Log(target.ProbabilityMassFunction(observation));
            double actual = target.LogProbabilityMassFunction(observation);

            Assert.AreEqual(expected, actual, 1e-6);
        }

    }
}
