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

using Accord.Math.Wavelets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accord.Math;
namespace Accord.Tests.Wavelets
{


    /// <summary>
    ///This is a test class for HaarTest and is intended
    ///to contain all HaarTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HaarTest
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
        public void FWT2DTest()
        {

            double[,] original =
            {
                { 5, 6, 1, 2 },
                { 4, 2, 5, 5 },
                { 3, 1, 7, 1 },
                { 6, 3, 5, 1 }
            };

            double[,] data = (double[,])original.Clone();

            Haar.FWT(data,1);

            double[,] expected = 
            {
                {  6.3640,    5.6569,    4.2426,    4.9497 },
                {  6.3640,    2.8284,    8.4853 ,   1.4142 },
                {  0.7071,    2.8284,   -2.8284 ,  -2.1213 },
                { -2.1213,   -1.4142,    1.4142 ,        0 }
            };


            Haar.IWT(data,1);

            Assert.IsTrue(Matrix.IsEqual(data, original, 0.0001));

        }

        [TestMethod()]
        public void IWTTest()
        {
            double[] original = { 1, 2, 3, 4 };
            double[] data = { 1, 2, 3, 4 };
            double[] expected = { 2.1213, 4.9497, -0.7071, -0.7071 };

            Haar.FWT(data);

            var d = data.Multiply(Constants.Sqrt2);

            Assert.IsTrue(Matrix.IsEqual(expected, d, 0.001));

            Haar.IWT(data);
            Assert.IsTrue(Matrix.IsEqual(original, data, 0.001));
        }

    }
}
