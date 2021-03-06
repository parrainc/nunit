// ***********************************************************************
// Copyright (c) 2007 Charlie Poole, Rob Prouse
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace NUnit.Framework.Constraints
{
    [TestFixture]
    public class LessThanOrEqualConstraintTests : ComparisonConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            TheConstraint = ComparisonConstraint = new LessThanOrEqualConstraint(5);
            ExpectedDescription = "less than or equal to 5";
            StringRepresentation = "<lessthanorequal 5>";
        }

        static object[] SuccessData = new object[] { 4, 5 };

        static object[] FailureData = new object[] { new object[] { 6, "6" } };

        [Test]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(42);
            ClassWithIComparable actual = new ClassWithIComparable(0);
            Assert.That(actual, Is.LessThanOrEqualTo(expected));
        }

        [Test]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(42);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(0);
            Assert.That(actual, Is.LessThanOrEqualTo(expected));
        }

        [TestCase(4.0, 5.0, 0.05)]
        [TestCase(4.95, 5.0, 0.05)] // lower range bound
        [TestCase(4.9501, 5.0, 0.05)] // lower range bound + .01
        [TestCase(4.9999, 5.0, 0.05)]
        [TestCase(5.0001, 5.0, 0.05)]
        [TestCase(5.05, 5.0, 0.05)] // upper range bound
        [TestCase(190, 200, 5)]
        [TestCase(195, 200, 5)] // lower range bound
        [TestCase(196, 200, 5)] // lower range bound + 1
        [TestCase(198, 200, 5)]
        [TestCase(202, 200, 5)]
        [TestCase(205, 200, 5)] // upper range bound
        public void SimpleTolerance(object actual, object expected, object tolerance)
        {
            Assert.That(actual, Is.LessThanOrEqualTo(expected).Within(tolerance));
        }

        [TestCase(6.0, 5.0, 0.05)]
        [TestCase(210, 200, 5)]
        public void SimpleTolerance_Failure(object actual, object expected, object tolerance)
        {
            var ex = Assert.Throws<AssertionException>(
                () => Assert.That(actual, Is.LessThanOrEqualTo(expected).Within(tolerance)),
                "Assertion should have failed");

            Assert.That(ex.Message, Contains.Substring("Expected: less than or equal to " + expected.ToString()));
        }

        [TestCase(4.0, 5.0, 1)]
        [TestCase(4.95, 5.0, 1)] // lower range bound
        [TestCase(4.9501, 5.0, 1)] // lower range bound + .01
        [TestCase(4.9999, 5.0, 1)]
        [TestCase(5.0001, 5.0, 1)]
        [TestCase(5.05, 5.0, 1)] // upper range bound
        [TestCase(190, 200, 2.5)]
        [TestCase(195, 200, 2.5)] // lower range bound
        [TestCase(196, 200, 2.5)] // lower range bound + 1
        [TestCase(198, 200, 2.5)]
        [TestCase(202, 200, 2.5)]
        [TestCase(205, 200, 2.5)] // upper range bound
        public void PercentTolerance(object actual, object expected, object tolerance)
        {
            Assert.That(actual, Is.LessThanOrEqualTo(expected).Within(tolerance).Percent);
        }

        [TestCase(6.0, 5.0, 1)]
        [TestCase(210, 200, 2.5)]
        public void PercentTolerance_Failure(object actual, object expected, object tolerance)
        {
            var ex = Assert.Throws<AssertionException>(
                () => Assert.That(actual, Is.LessThanOrEqualTo(expected).Within(tolerance).Percent),
                "Assertion should have failed");

            Assert.That(ex.Message, Contains.Substring("Expected: less than or equal to " + MsgUtils.FormatValue(expected) + " within " + MsgUtils.FormatValue(tolerance) + " percent"));
        }
    }
}
