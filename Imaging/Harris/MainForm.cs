﻿// Accord.NET Sample Applications
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
using System.Drawing;
using System.Windows.Forms;
using Accord.Imaging;
using AForge.Imaging.Filters;

namespace Harris
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Open a image
            Bitmap lenna = Harris.Properties.Resources.lena512;

            double sigma = (double)numSigma.Value;
            float k = (float)numK.Value;
            float threshold = (float)numThreshold.Value;

            // Create a new Harris Corners Detector using the given parameters
            HarrisCornersDetector harris = new HarrisCornersDetector(k);
            harris.Measure = checkBox1.Checked ? HarrisCornerMeasure.Harris : HarrisCornerMeasure.Nobel;
            harris.Threshold = threshold;
            harris.Sigma = sigma;

            // Create a new AForge's Corner Marker Filter
            CornersMarker corners = new CornersMarker(harris, Color.White);

            // Apply the filter and display it on a picturebox
            pictureBox1.Image = corners.Apply(lenna);
        }
    }
}
