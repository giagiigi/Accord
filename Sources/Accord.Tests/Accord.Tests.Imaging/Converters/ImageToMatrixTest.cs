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

namespace Accord.Tests.Imaging
{
    using System.Drawing;
    using Accord.Imaging.Converters;
    using AForge.Imaging;
    using AForge.Imaging.Filters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Collections.Generic;
    using AForge;
    using System;
    using System.Drawing.Imaging;

    [TestClass()]
    public class ImageToMatrixTest
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


        [TestMethod()]
        public void ImageToMatrixConstructorTest()
        {
            double min = -10;
            double max = +10;
            ImageToMatrix target = new ImageToMatrix(min, max);

            Assert.AreEqual(min, target.Min);
            Assert.AreEqual(max, target.Max);
            Assert.AreEqual(0, target.Channel);
        }

        [TestMethod()]
        public void ImageToMatrixConstructorTest1()
        {
            ImageToMatrix target = new ImageToMatrix();

            Assert.AreEqual(0, target.Min);
            Assert.AreEqual(1, target.Max);
            Assert.AreEqual(0, target.Channel);
        }

        [TestMethod()]
        public void ImageToMatrixConstructorTest2()
        {
            double min = -10;
            double max = +10;
            int channel = 2;
            ImageToMatrix target = new ImageToMatrix(min, max, channel);

            Assert.AreEqual(min, target.Min);
            Assert.AreEqual(max, target.Max);
            Assert.AreEqual(channel, target.Channel);
        }

        [TestMethod()]
        public void ConvertTest()
        {
            ImageToMatrix target = new ImageToMatrix(min: 0, max: 255);
            Bitmap image = Properties.Resources.image1;

            new Invert().ApplyInPlace(image);
            new Threshold().ApplyInPlace(image);

            double[,] output;
            double[,] outputExpected =
            {
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 0
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 1
                 { 0, 0, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 0, 0 }, // 2 
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 3
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 4
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 5
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 6
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 7
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 8
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 9
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 10
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 11
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 12
                 { 0, 0, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 0, 0 }, // 13
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 14
                 { 0, 0,   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,   0, 0, 0 }, // 15
            };


            target.Convert(image, out output);

            for (int i = 0; i < outputExpected.GetLength(0); i++)
                for (int j = 0; j < outputExpected.GetLength(1); j++)
                    Assert.AreEqual(outputExpected[i, j], output[i, j]);
        }

        [TestMethod()]
        public void ConvertTest2()
        {
            // Load a test image
            Bitmap sourceImage = Properties.Resources.image1;

            // Make sure values are binary
            new Threshold().ApplyInPlace(sourceImage);

            // Create the converters
            ImageToMatrix imageToMatrix = new ImageToMatrix() { Min = 0, Max = 255 };
            MatrixToImage matrixToImage = new MatrixToImage() { Min = 0, Max = 255 };

            // Convert to matrix
            double[,] matrix; // initialization is not needed
            imageToMatrix.Convert(sourceImage, out matrix);

            // Revert to image
            Bitmap resultImage; // initialization is not needed
            matrixToImage.Convert(matrix, out resultImage);

            // Show both images, which should be equal
            // ImageBox.Show(sourceImage, PictureBoxSizeMode.Zoom);
            // ImageBox.Show(resultImage, PictureBoxSizeMode.Zoom);

            UnmanagedImage img1 = UnmanagedImage.FromManagedImage(sourceImage);
            UnmanagedImage img2 = UnmanagedImage.FromManagedImage(resultImage);

            List<IntPoint> p1 = img1.CollectActivePixels();
            List<IntPoint> p2 = img2.CollectActivePixels();

            bool equals = new HashSet<IntPoint>(p1).SetEquals(p2);

            Assert.IsTrue(equals);
        }

    }
}
