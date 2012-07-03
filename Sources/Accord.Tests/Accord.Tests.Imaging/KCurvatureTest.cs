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

using Accord.Math.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AForge;
using System.Collections.Generic;
using AForge.Imaging.Filters;
using Accord.Controls.Imaging;
using AForge.Imaging;
using Accord.Imaging;
using Accord.Imaging.Filters;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing.Imaging;

namespace Accord.Tests.Imaging
{


    /// <summary>
    ///This is a test class for KCurvatureTest and is intended
    ///to contain all KCurvatureTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KCurvatureTest
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


        /// <summary>
        ///A test for FindPeaks
        ///</summary>
        [TestMethod()]
        public void FindPeaksTest()
        {
            Bitmap hand = Properties.Resources.rhand;

            GaussianBlur median = new GaussianBlur(1.1);
            median.ApplyInPlace(hand);

            // Extract contour
            BorderFollowing bf = new BorderFollowing(1);
            List<IntPoint> contour = bf.FindContour(hand);

            hand = hand.Clone(new Rectangle(0, 0, hand.Width, hand.Height), PixelFormat.Format24bppRgb);

            // Find peaks
            KCurvature kcurv = new KCurvature(30, new DoubleRange(0, 45));
            // kcurv.Suppression = 30;
            var peaks = kcurv.FindPeaks(contour);

            List<IntPoint> supports = new List<IntPoint>();
            for (int i = 0; i < peaks.Count; i++)
            {
                int j = contour.IndexOf(peaks[i]);
                supports.Add(contour[(j + kcurv.K) % contour.Count]);
                supports.Add(contour[Accord.Math.Tools.Mod(j - kcurv.K, contour.Count)]);
            }

            // show(hand, contour, peaks, supports);

            Assert.AreEqual(2, peaks.Count);
            Assert.AreEqual(46, peaks[0].X);
            Assert.AreEqual(0, peaks[0].Y);
            Assert.AreEqual(2, peaks[1].X);
            Assert.AreEqual(11, peaks[1].Y);
        }

        private static void show(Bitmap hand, List<IntPoint> contour, List<IntPoint> peaks, List<IntPoint> supports)
        {
            PointsMarker cmarker = new PointsMarker(contour, Color.White, 1);
            cmarker.ApplyInPlace(hand);

            PointsMarker pmarker = new PointsMarker(peaks, Color.Green, 5);
            pmarker.ApplyInPlace(hand);

            PointsMarker hmarker = new PointsMarker(supports, Color.Yellow, 5);
            hmarker.ApplyInPlace(hand);

            ImageBox.Show(hand, PictureBoxSizeMode.Zoom);
        }

        /// <summary>
        ///A test for FindPeaks
        ///</summary>
        [TestMethod()]
        public void FindPeaksTest2()
        {
            Bitmap hand = Properties.Resources.rhand0;

            //  ImageBox.Show(hand);

            GaussianBlur median = new GaussianBlur(1.1);
            median.ApplyInPlace(hand);

            // Extract contour
            BorderFollowing bf = new BorderFollowing(20);
            List<IntPoint> contour = bf.FindContour(hand);

            hand = hand.Clone(new Rectangle(0, 0, hand.Width, hand.Height), PixelFormat.Format24bppRgb);

            // Find peaks
            KCurvature kcurv = new KCurvature(30, new DoubleRange(0, 45));
            var peaks = kcurv.FindPeaks(contour);

            List<IntPoint> supports = new List<IntPoint>();
            for (int i = 0; i < peaks.Count; i++)
            {
                int j = contour.IndexOf(peaks[i]);
                supports.Add(contour[(j + kcurv.K) % contour.Count]);
                supports.Add(contour[Accord.Math.Tools.Mod(j - kcurv.K, contour.Count)]);
            }

            // show(hand, contour, peaks, supports);

            Assert.AreEqual(5, peaks.Count);
            Assert.AreEqual(0, peaks[0].X);
            Assert.AreEqual(80, peaks[0].Y);
            Assert.AreEqual(113, peaks[1].X);
            Assert.AreEqual(26, peaks[1].Y);
            Assert.AreEqual(98, peaks[2].X);
            Assert.AreEqual(9, peaks[2].Y);
            Assert.AreEqual(73, peaks[3].X);
            Assert.AreEqual(2, peaks[3].Y);
            Assert.AreEqual(38, peaks[4].X);
            Assert.AreEqual(14, peaks[4].Y);
        }

        /// <summary>
        ///A test for FindPeaks
        ///</summary>
        [TestMethod()]
        public void FindPeaksTest3()
        {
            Bitmap hand = Properties.Resources.rhand1;

            GaussianBlur median = new GaussianBlur(1.1);
            median.ApplyInPlace(hand);

            // Extract contour
            BorderFollowing bf = new BorderFollowing(20);
            List<IntPoint> contour = bf.FindContour(hand);

            hand = hand.Clone(new Rectangle(0, 0, hand.Width, hand.Height), PixelFormat.Format24bppRgb);

            // Find peaks
            KCurvature kcurv = new KCurvature(30, new DoubleRange(0, 45));
            var peaks = kcurv.FindPeaks(contour);

            List<IntPoint> supports = new List<IntPoint>();
            for (int i = 0; i < peaks.Count; i++)
            {
                int j = contour.IndexOf(peaks[i]);
                supports.Add(contour[(j + kcurv.K) % contour.Count]);
                supports.Add(contour[Accord.Math.Tools.Mod(j - kcurv.K, contour.Count)]);
            }

            // show(hand, contour, peaks, supports);

            Assert.AreEqual(2, peaks.Count);
            Assert.AreEqual(38, peaks[0].X);
            Assert.AreEqual(2, peaks[0].Y);
            Assert.AreEqual(1, peaks[1].X);
            Assert.AreEqual(15, peaks[1].Y);
        }

        /// <summary>
        ///A test for FindPeaks
        ///</summary>
        [TestMethod()]
        public void FindPeaksTest4()
        {
            Bitmap hand = Properties.Resources.rhand2;

            GaussianBlur median = new GaussianBlur(1.1);
            median.ApplyInPlace(hand);

            // Extract contour
            BorderFollowing bf = new BorderFollowing(20);
            List<IntPoint> contour = bf.FindContour(hand);

            hand = hand.Clone(new Rectangle(0, 0, hand.Width, hand.Height), PixelFormat.Format24bppRgb);

            // Find peaks
            KCurvature kcurv = new KCurvature(30, new DoubleRange(0, 45));
            var peaks = kcurv.FindPeaks(contour);

            List<IntPoint> supports = new List<IntPoint>();
            for (int i = 0; i < peaks.Count; i++)
            {
                int j = contour.IndexOf(peaks[i]);
                supports.Add(contour[(j + kcurv.K) % contour.Count]);
                supports.Add(contour[Accord.Math.Tools.Mod(j - kcurv.K, contour.Count)]);
            }

            // show(hand, contour, peaks, supports);

            Assert.AreEqual(1, peaks.Count);
            Assert.AreEqual(18, peaks[0].X);
            Assert.AreEqual(0, peaks[0].Y);
        }

        /// <summary>
        ///A test for FindPeaks
        ///</summary>
        [TestMethod()]
        public void FindPeaksTest6()
        {
            Bitmap hand = Properties.Resources.rhand3;

            GaussianBlur median = new GaussianBlur(1.1);
            median.ApplyInPlace(hand);

            // Extract contour
            BorderFollowing bf = new BorderFollowing(1);
            List<IntPoint> contour = bf.FindContour(hand);

            hand = hand.Clone(new Rectangle(0, 0, hand.Width, hand.Height), PixelFormat.Format24bppRgb);

            // Find peaks
            KCurvature kcurv = new KCurvature(30, new DoubleRange(0, 45));
            var peaks = kcurv.FindPeaks(contour);

            List<IntPoint> supports = new List<IntPoint>();
            for (int i = 0; i < peaks.Count; i++)
            {
                int j = contour.IndexOf(peaks[i]);
                supports.Add(contour[(j + kcurv.K) % contour.Count]);
                supports.Add(contour[Accord.Math.Tools.Mod(j - kcurv.K, contour.Count)]);
            }

            // show(hand, contour, peaks, supports);

            Assert.AreEqual(5, peaks.Count);
            Assert.AreEqual(0, peaks[0].X);
            Assert.AreEqual(95, peaks[0].Y);
            Assert.AreEqual(98, peaks[1].X);
            Assert.AreEqual(13, peaks[1].Y);
            Assert.AreEqual(69, peaks[2].X);
            Assert.AreEqual(0, peaks[2].Y);
            Assert.AreEqual(39, peaks[3].X);
            Assert.AreEqual(0, peaks[3].Y);
            Assert.AreEqual(17, peaks[4].X);
            Assert.AreEqual(17, peaks[4].Y);
        }
    }
}
