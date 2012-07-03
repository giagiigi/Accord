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

using Accord.Statistics.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace Accord.Tests.Statistics
{
    
    
    /// <summary>
    ///This is a test class for EqualizingFilterTest and is intended
    ///to contain all EqualizingFilterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EqualizingFilterTest
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
        ///A test for Apply
        ///</summary>
        [TestMethod()]
        public void ApplyTest()
        {
            DataTable data = new DataTable("Sample data");
            data.Columns.Add("x", typeof(double));
            data.Columns.Add("Class", typeof(int));
            data.Rows.Add(0.21, 0);
            data.Rows.Add(0.25, 0);
            data.Rows.Add(0.54, 0);
            data.Rows.Add(0.19, 1);

            DataTable expected = new DataTable("Sample data");
            expected.Columns.Add("x", typeof(double));
            expected.Columns.Add("Class", typeof(int));
            expected.Rows.Add(0.21, 0);
            expected.Rows.Add(0.25, 0);
            expected.Rows.Add(0.54, 0);
            expected.Rows.Add(0.19, 1);
            expected.Rows.Add(0.19, 1);
            expected.Rows.Add(0.19, 1);


            DataTable actual;

            Equalization target = new Equalization("Class");
            target.Columns["Class"].Classes = new int[] { 0, 1 };
            
            actual = target.Apply(data);

            for (int i = 0; i < actual.Rows.Count; i++)
            {
                double ex = (double)expected.Rows[i][0];
                int ec = (int)expected.Rows[i][1];

                double ax = (double)actual.Rows[i][0];
                int ac = (int)actual.Rows[i][1];

                Assert.AreEqual(ex, ax);
                Assert.AreEqual(ec, ac);                    
                
            }
            
        }
    }
}
