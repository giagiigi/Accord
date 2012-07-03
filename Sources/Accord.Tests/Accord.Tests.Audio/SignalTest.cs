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

using Accord.Audio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Accord.Tests.Audio
{


    /// <summary>
    ///This is a test class for SignalTest and is intended
    ///to contain all SignalTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SignalTest
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


        private float[,] data = 
            {
                {  0.00f, 0.2f  },
                {  0.32f, 0.1f  },
                {  0.22f, 0.2f  },
                {  0.12f, 0.42f },
                { -0.12f, 0.1f  },
                { -0.22f, 0.2f  },
            };

        /// <summary>
        ///A test for GetEnergy
        ///</summary>
        [TestMethod()]
        public void GetEnergyTest()
        {
            Signal target = Signal.FromArray(data, 8000);

            double expected = 0.54439;
            double actual = target.GetEnergy();
            Assert.AreEqual(expected, actual, 1e-4);
        }

        /// <summary>
        ///A test for Signal Constructor
        ///</summary>
        [TestMethod()]
        public void SignalConstructorTest()
        {
            Signal target = Signal.FromArray(data, 8000);
            Assert.AreEqual(target.Length, 6);
            Assert.AreEqual(target.Samples, 12);
            Assert.AreEqual(target.Channels, 2);
            Assert.AreEqual(target.SampleRate, 8000);
        }


        /// <summary>
        ///A test for GetSample
        ///</summary>
        [TestMethod()]
        public void GetSampleTest()
        {
            float[,] data = (float[,])this.data.Clone();
            Signal target = Signal.FromArray(data, 8000);

            int channel = 1;
            int position = 3;

            float expected, actual;
            
            expected = 0.42f;
            actual = target.GetSample(channel, position);
            Assert.AreEqual(expected, actual);

            target.SetSample(channel, position, -1);

            expected = -1;
            actual = target.GetSample(channel, position);
            Assert.AreEqual(expected, actual);
            
        }
    }
}
