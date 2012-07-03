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

namespace Accord.Tests.Statistics
{
    using Accord.Statistics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Accord.Math;
    using Accord;
    using System.Data;

    [TestClass()]
    public class CircularTest
    {

        private TestContext testContextInstance;

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


        private double[] angles = 
        {
            0.003898633, 5.956808760, 0.318487983,
            5.887227832, 0.641802182, 4.640345741,
            0.931996171, 0.426819547, 0.624824460,
            0.247553652, 6.282827901, 0.313780766,
            0.093206440, 0.392279489, 0.601228848
        };


        [TestMethod()]
        public void WeightedKappaTest()
        {
    DataTable table = new DataTable();

    // Add multiple columns at once:
    table.Columns.Add("columnName1", "columnName2");

            double[] angles = { 0.1242, 1.2425, 0.6712 };
            double[] weights = { 3, 1, 1 };

            weights = weights.Divide(weights.Sum());

            double expectedMean = 0.4436528;
            double expectedKappa = 5.497313;

            double actualMean = Circular.WeightedMean(angles, weights);
            Assert.AreEqual(expectedMean, actualMean, 1e-6);

            double actualKappa = Circular.WeightedConcentration(angles, weights);
            Assert.AreEqual(expectedKappa, actualKappa, 1e-6);

            actualKappa = Circular.WeightedConcentration(angles, weights, actualMean);
            Assert.AreEqual(expectedKappa, actualKappa, 1e-6);
        }

        [TestMethod()]
        public void KappaTest1()
        {
            double expected = 3.721646;
            double actual = Circular.Concentration(angles);

            Assert.AreEqual(expected, actual, 1e-6);
        }

        [TestMethod()]
        public void KappaTest()
        {
            double mean = Circular.Mean(angles);

            double expected = 3.721646;
            double actual = Circular.Concentration(angles, mean);

            Assert.AreEqual(expected, actual, 1e-6);
        }

        [TestMethod()]
        public void MeanTest()
        {
            double expected = 0.2051961;
            double actual = Circular.Mean(angles);

            Assert.AreEqual(expected, actual, 1e-6);
        }

        [TestMethod()]
        public void VarianceTest()
        {
            double expected = 0.1466856; 
            double actual = Circular.Variance(angles);

            Assert.AreEqual(expected, actual, 1e-6);
        }

    }
}
