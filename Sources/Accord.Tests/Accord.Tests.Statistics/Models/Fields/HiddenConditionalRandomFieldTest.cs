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

namespace Accord.Tests.Statistics.Models.Fields
{
    using Accord.Statistics.Models.Fields;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Accord.Statistics.Models.Fields.Functions;
    using Accord.Statistics.Models.Markov;
    using Accord.Statistics.Models.Markov.Learning;
    using Accord.Math;
    using System;
    using Accord.Statistics.Models.Markov.Topology;

    /// <summary>
    ///This is a test class for ConditionalRandomFieldTest and is intended
    ///to contain all ConditionalRandomFieldTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HiddenConditionalRandomFieldTest
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
        public void HiddenConditionalRandomFieldConstructorTest()
        {
            HiddenMarkovClassifier hmm = HiddenMarkovClassifierPotentialFunctionTest.CreateModel1();

            var function = new DiscreteMarkovClassifierFunction(hmm);
            var target = new HiddenConditionalRandomField<int>(function);

            Assert.AreEqual(function, target.Function);
            Assert.AreEqual(2, target.Function.Factors[0].States);
        }

        [TestMethod()]
        public void ComputeTest()
        {
            HiddenMarkovClassifier hmm = HiddenMarkovClassifierPotentialFunctionTest.CreateModel1();

            // Declare some testing data
            int[][] inputs = new int[][]
            {
                new int[] { 0,1,1,0 },   // Class 0
                new int[] { 0,0,1,0 },   // Class 0
                new int[] { 0,1,1,1,0 }, // Class 0
                new int[] { 0,1,0 },     // Class 0

                new int[] { 1,0,0,1 },   // Class 1
                new int[] { 1,1,0,1 },   // Class 1
                new int[] { 1,0,0,0,1 }, // Class 1
                new int[] { 1,0,1 },     // Class 1
            };

            int[] outputs = new int[]
            {
                0,0,0,0, // First four sequences are of class 0
                1,1,1,1, // Last four sequences are of class 1
            };


            var function = new DiscreteMarkovClassifierFunction(hmm);
            var target = new HiddenConditionalRandomField<int>(function);


            for (int i = 0; i < inputs.Length; i++)
            {
                int expected = hmm.Compute(inputs[i]);

                int actual = target.Compute(inputs[i]);

                double h0 = hmm.LogLikelihood(inputs[i], 0);
                double h1 = hmm.LogLikelihood(inputs[i], 1);

                double c0 = target.LogLikelihood(inputs[i], 0);
                double c1 = target.LogLikelihood(inputs[i], 1);

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(h0, c0, 1e-10); 
                Assert.AreEqual(h1, c1, 1e-10);

                Assert.IsFalse(double.IsNaN(c0));
                Assert.IsFalse(double.IsNaN(c1));
            }
        }


    }
}
