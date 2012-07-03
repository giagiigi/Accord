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

namespace Accord.Tests.Math
{
    using Accord.Math.Optimization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;


    [TestClass()]
    public class AugmentedLagrangianSolverTest
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
        public void AugmentedLagrangianSolverConstructorTest1()
        {
            // min 100(y-x*x)²+(1-x)²
            //
            // s.t.  x <= 0
            //       y <= 0
            //

            var f = new NonlinearObjectiveFunction(2,

                function: (x) => 100 * Math.Pow(x[1] - x[0] * x[0], 2) + Math.Pow(1 - x[0], 2),

                gradient: (x) => new[] 
                {
                    2.0 * (200.0 * x[0]*x[0]*x[0] - 200.0 * x[0] * x[1] + x[0] - 1), // df/dx
                    200 * (x[1] - x[0]*x[0])                                         // df/dy
                }

            );


            var constraints = new List<NonlinearConstraint>();

            constraints.Add(new NonlinearConstraint(f,

                function: (x) => x[0],
                gradient: (x) => new[] { 1.0, 0.0 },

                shouldBe: ConstraintType.LesserThanOrEqualTo, value: 0
            ));

            constraints.Add(new NonlinearConstraint(f,

                function: (x) => x[1],
                gradient: (x) => new[] { 0.0, 1.0 },

                shouldBe: ConstraintType.LesserThanOrEqualTo, value: 0
            ));

            var solver = new AugmentedLagrangianSolver(2, constraints);

            double minValue = solver.Minimize(f);

            Assert.AreEqual(1, minValue, 1e-5);
            Assert.AreEqual(0, solver.Solution[0], 1e-5);
            Assert.AreEqual(0, solver.Solution[1], 1e-5);
        }


        [TestMethod()]
        public void AugmentedLagrangianSolverConstructorTest2()
        {
            // min 100(y-x*x)²+(1-x)²
            //
            // s.t.  x >= 0
            //       y >= 0
            //

            var f = new NonlinearObjectiveFunction(2,

                function: (x) => 100 * Math.Pow(x[1] - x[0] * x[0], 2) + Math.Pow(1 - x[0], 2),

                gradient: (x) => new[] 
                {
                    2.0 * (200.0 * Math.Pow(x[0], 3) - 200.0 * x[0] * x[1] + x[0] - 1), // df/dx
                    200 * (x[1] - x[0]*x[0])                                            // df/dy
                }

            );


            var constraints = new List<NonlinearConstraint>();

            constraints.Add(new NonlinearConstraint(f,

                function: (x) => x[0],
                gradient: (x) => new[] { 1.0, 0.0 },

                shouldBe: ConstraintType.GreaterThanOrEqualTo, value: 0
            ));

            constraints.Add(new NonlinearConstraint(f,

                function: (x) => x[1],
                gradient: (x) => new[] { 0.0, 1.0 },

                shouldBe: ConstraintType.GreaterThanOrEqualTo, value: 0
            ));

            var solver = new AugmentedLagrangianSolver(2, constraints);

            double minValue = solver.Minimize(f);

            Assert.AreEqual(0, minValue, 1e-10);
            Assert.AreEqual(1, solver.Solution[0], 1e-10);
            Assert.AreEqual(1, solver.Solution[1], 1e-10);

            Assert.IsFalse(Double.IsNaN(minValue));
            Assert.IsFalse(Double.IsNaN(solver.Solution[0]));
            Assert.IsFalse(Double.IsNaN(solver.Solution[1]));

        }

        [TestMethod()]
        public void AugmentedLagrangianSolverConstructorTest3()
        {
            // min x*y+ y*z
            //
            // s.t.  x^2 - y^2 + z^2 - 2  >= 0
            //       x^2 + y^2 + z^2 - 10 <= 0
            //

            double x = 0, y = 0, z = 0;

            var f = new NonlinearObjectiveFunction(

                function: () => x * y + y * z,

                gradient: () => new[] 
                {
                    y,     // df/dx
                    x + z, // df/dy
                    y,     // df/dz
                }

            );


            var constraints = new List<NonlinearConstraint>();

            constraints.Add(new NonlinearConstraint(f,

                function: () => x * x - y * y + z * z,
                gradient: () => new[] { 2 * x, -2 * y, 2 * z },

                shouldBe: ConstraintType.GreaterThanOrEqualTo, value: 2
            ));

            constraints.Add(new NonlinearConstraint(f,

                function: () => x * x + y * y + z * z,
                gradient: () => new[] { 2 * x, 2 * y, 2 * z },

                shouldBe: ConstraintType.LesserThanOrEqualTo, value: 10
            ));

            var solver = new AugmentedLagrangianSolver(3, constraints);

            solver.Solution[0] = 1;
            solver.Solution[1] = 1;
            solver.Solution[2] = 1;

            double minValue = solver.Minimize(f);

            Assert.AreEqual(-6.9, minValue, 1e-1);
            Assert.AreEqual(+1.73, solver.Solution[0], 1e-2);
            Assert.AreEqual(-2.00, solver.Solution[1], 1e-2);
            Assert.AreEqual(+1.73, solver.Solution[2], 1e-2);

            Assert.IsFalse(Double.IsNaN(minValue));
            Assert.IsFalse(Double.IsNaN(solver.Solution[0]));
            Assert.IsFalse(Double.IsNaN(solver.Solution[1]));
            Assert.IsFalse(Double.IsNaN(solver.Solution[2]));

        }

        [TestMethod()]
        public void AugmentedLagrangianSolverConstructorTest4()
        {
            // min x*y+ y*z
            //
            // s.t.  x^2 - y^2 + z^2 - 2  >= 0
            //       x^2 + y^2 + z^2 - 10 <= 0
            //       x   + y               = 1
            //

            double x = 0, y = 0, z = 0;

            var f = new NonlinearObjectiveFunction(

                function: () => x * y + y * z,

                gradient: () => new[] 
                {
                    y,     // df/dx
                    x + z, // df/dy
                    y,     // df/dz
                }

            );


            var constraints = new List<NonlinearConstraint>();

            constraints.Add(new NonlinearConstraint(f,

                function: () => x * x - y * y + z * z,
                gradient: () => new[] { 2 * x, -2 * y, 2 * z },

                shouldBe: ConstraintType.GreaterThanOrEqualTo, value: 2
            ));

            constraints.Add(new NonlinearConstraint(f,

                function: () => x * x + y * y + z * z,
                gradient: () => new[] { 2 * x, 2 * y, 2 * z },

                shouldBe: ConstraintType.LesserThanOrEqualTo, value: 10
            ));

            constraints.Add(new NonlinearConstraint(f,

                function: () => x + y,
                gradient: () => new[] { 1.0, 1.0, 0.0 },

                shouldBe: ConstraintType.EqualTo, value: 1
            )
            {
                Tolerance = 1e-5
            });

            var solver = new AugmentedLagrangianSolver(3, constraints);

            solver.Solution[0] = 1;
            solver.Solution[1] = 1;
            solver.Solution[2] = 1;
            double minValue = solver.Minimize(f);

            Assert.AreEqual(1, solver.Solution[0] + solver.Solution[1], 1e-5);

            Assert.IsFalse(Double.IsNaN(minValue));
            Assert.IsFalse(Double.IsNaN(solver.Solution[0]));
            Assert.IsFalse(Double.IsNaN(solver.Solution[1]));
            Assert.IsFalse(Double.IsNaN(solver.Solution[2]));

        }

        [TestMethod()]
        public void AugmentedLagrangianSolverConstructorTest5()
        {
            // Suppose we would like to minimize the following function:
            //
            //    f(x,y) = min 100(y-x²)²+(1-x)²
            //
            // Subject to the constraints
            //
            //    x >= 0  (x must be positive)
            //    y >= 0  (y must be positive)
            //

            double x = 0, y = 0;


            // First, we create our objective function
            var f = new NonlinearObjectiveFunction(

                // This is the objective function:  f(x,y) = min 100(y-x²)²+(1-x)²
                function: () => 100 * Math.Pow(y - x * x, 2) + Math.Pow(1 - x, 2),

                // The gradient vector:
                gradient: () => new[] 
                {
                    2 * (200 * Math.Pow(x, 3) - 200 * x * y + x - 1), // df/dx = 2(200x³-200xy+x-1)
                    200 * (y - x*x)                                   // df/dy = 200(y-x²)
                }

            );


            // Now we can start stating the constraints
            var constraints = new List<NonlinearConstraint>();

            // Add the non-negativity constraint for x
            constraints.Add(new NonlinearConstraint(f,

                // 1st contraint: x should be greater than or equal to 0
                function: () => x, shouldBe: ConstraintType.GreaterThanOrEqualTo, value: 0,

                gradient: () => new[] { 1.0, 0.0 }
            ));

            // Add the non-negativity constraint for y
            constraints.Add(new NonlinearConstraint(f,

                // 2nd constraint: y should be greater than or equal to 0
                function: () => y, shouldBe: ConstraintType.GreaterThanOrEqualTo, value: 0,

                gradient: () => new[] { 0.0, 1.0 }
            ));


            // Finally, we create the non-linear programming solver
            var solver = new AugmentedLagrangianSolver(2, constraints);

            // And attempt to solve the problem
            double minValue = solver.Minimize(f);

            Assert.AreEqual(0, minValue, 1e-10);
            Assert.AreEqual(1, solver.Solution[0], 1e-10);
            Assert.AreEqual(1, solver.Solution[1], 1e-10);

            Assert.IsFalse(Double.IsNaN(minValue));
            Assert.IsFalse(Double.IsNaN(solver.Solution[0]));
            Assert.IsFalse(Double.IsNaN(solver.Solution[1]));

        }

    }
}
