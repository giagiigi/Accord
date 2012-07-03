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
using AForge.Math;
using Accord.Math;
using Tools = Accord.Audio.Tools;

namespace Accord.Tests.Audio
{
    
    
    /// <summary>
    ///This is a test class for ToolsTest and is intended
    ///to contain all ToolsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ToolsTest
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
        public void GetPowerCepstrumTest()
        {
            // http://mi.eng.cam.ac.uk/~ajr/SpeechAnalysis/node33.html

            Complex[] source = 
            {
                (Complex)(-1.0), (Complex)(+0.5),
                (Complex)(+0.3), (Complex)(-0.4),
                (Complex)(+0.1), (Complex)(-0.9),
                (Complex)(-0.2), (Complex)(-0.5),
            };

            double[] expected = 
            {
                0.3319, 0.1285, -0.0218, 0.2303, -0.2639, 0.2303, -0.0218, 0.1285
            };

            double[] actual = Tools.GetPowerCepstrum(source);

            Assert.IsTrue(expected.IsEqual(actual, 1e-4));
        }

        [TestMethod()]
        public void InterleaveTest()
        {
            float[,] channels = 
            {
                {  0.1f, -5.1f },
                { -0.2f, -6.0f },
                {  2.3f, -3.4f },
                {  0.7f,  5.2f },
                {  0.6f, -1.7f },
            };

            float[] expected = 
            {
                0.1f, -5.1f,
               -0.2f, -6.0f,
                2.3f, -3.4f,
                0.7f,  5.2f,
                0.6f, -1.7f,
            };

            float[] actual = Tools.Interleave(channels);

            Assert.IsTrue(actual.IsEqual(expected));
        }
    }
}
