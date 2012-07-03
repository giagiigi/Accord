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

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Accord.Imaging.Filters;
using Accord.Imaging;
using Accord.Controls;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;

namespace Accord.Tests.Imaging
{
    /// <summary>
    /// Summary description for BlendTest
    /// </summary>
    [TestClass]
    public class BlendTest
    {
        public BlendTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ApplyTest()
        {
            var img1 = Properties.Resources.image2;
            var img2 = Properties.Resources.image2;

            MatrixH homography = new MatrixH(1, 0, 32,
                                             0, 1, 0,
                                             0, 0);

            Blend blend = new Blend(homography, img1);
            var actual = blend.Apply(img2);

            Assert.AreEqual(64, actual.Size.Width);
            Assert.AreEqual(32, actual.Size.Height);



            homography = new MatrixH(1, 0, 32,
                                     0, 1, 32,
                                     0, 0);

            blend = new Blend(homography, img1);
            actual = blend.Apply(img2);


            Assert.AreEqual(64, actual.Size.Width);
            Assert.AreEqual(64, actual.Size.Height);

            // ImageBox.Show(img3, PictureBoxSizeMode.Zoom);
        }

        [TestMethod]
        public void ApplyTest2()
        {
            var img1 = Properties.Resources.image2;
            var img2 = Properties.Resources.image2;

            var img3 = Properties.Resources.image2;
            var img4 = Properties.Resources.image2;


            MatrixH homography;
            Blend blend;

            homography = new MatrixH(1, 0, 32,
                                     0, 1, 0,
                                     0, 0);


            blend = new Blend(homography, img1);
            var img12 = blend.Apply(img2);

            //ImageBox.Show("Blend of 1 and 2", img12, PictureBoxSizeMode.Zoom);
            Assert.AreEqual(img12.PixelFormat, PixelFormat.Format32bppArgb);

            blend = new Blend(homography, img3);
            var img34 = blend.Apply(img4);

            //ImageBox.Show("Blend of 3 and 4", img34, PictureBoxSizeMode.Zoom);
            Assert.AreEqual(img34.PixelFormat, PixelFormat.Format32bppArgb);


            homography = new MatrixH(1, 0, 64,
                                     0, 1, 0,
                                     0, 0);


            blend = new Blend(homography, img12);
            var img1234 = blend.Apply(img34);

            //ImageBox.Show("Blend of 1, 2, 3, 4", img1234, PictureBoxSizeMode.Zoom);
            Assert.AreEqual(img1234.PixelFormat, PixelFormat.Format32bppArgb);



            // Blend of 1 and 5 (8bpp and 32bpp)
            homography = new MatrixH(1, 0, 0,
                                     0, 1, 32,
                                     0, 0);


            //ImageBox.Show("Image 1", img1, PictureBoxSizeMode.Zoom);
            blend = new Blend(homography, img1234);
            var img15 = blend.Apply(img1);
            //ImageBox.Show("Blend of 1 and 5", img15, PictureBoxSizeMode.Zoom);

            Assert.AreEqual(img1234.PixelFormat, PixelFormat.Format32bppArgb);
            Assert.AreEqual(img1.PixelFormat, PixelFormat.Format8bppIndexed);
            Assert.AreEqual(img15.PixelFormat, PixelFormat.Format32bppArgb);
            Assert.AreEqual(128, img15.Width);
            Assert.AreEqual(64, img15.Height);

        }
/*
        [TestMethod]
        public void ApplyTest3()
        {
            var img1 = Properties.Resources.sample_trans;
            var img2 = Properties.Resources.sample_trans;

            MatrixH homography = new MatrixH(1, 0, 16,
                                             0, 1, 0,
                                             0, 0);

            Blend blend = new Blend(homography, img1);
            var actual = blend.Apply(img2);


            ImageBox.Show(actual, PictureBoxSizeMode.Zoom, Color.White);
        }
*/
    }
}
