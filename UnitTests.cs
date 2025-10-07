using System;
using System.Collections.Generic;
using System.Reflection;

[TestClass]
public partial class UnitTests
{
    // Your code starts here
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethod1() { GetDiscountedPrice(int.MinValue); }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethod2() { GetDiscountedPrice(int.MinValue + 1); }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestMethod3() { GetDiscountedPrice(-1); }

    [TestMethod]
    public void TestMethod4() { Assert.AreEqual(0.0, GetDiscountedPrice(0)); }

    [TestMethod]
    public void TestMethod5() { Assert.AreEqual(1.0, GetDiscountedPrice(1)); }

    [TestMethod]
    public void TestMethod6() { Assert.AreEqual(99.0, GetDiscountedPrice(99)); }

    [TestMethod]
    public void TestMethod7() { Assert.AreEqual(90.0, GetDiscountedPrice(100)); }

    [TestMethod]
    public void TestMethod8() { Assert.AreEqual(90.9, GetDiscountedPrice(101)); }

    [TestMethod]
    public void TestMethod9() { Assert.AreEqual(899.1, GetDiscountedPrice(999)); }

    [TestMethod]
    public void TestMethod10() { Assert.AreEqual(800.0, GetDiscountedPrice(1000)); }

    [TestMethod]
    public void TestMethod11() { Assert.AreEqual(800.8, GetDiscountedPrice(1001)); }

    [TestMethod]
    public void TestMethod12() { Assert.AreEqual(7999.2, GetDiscountedPrice(9999)); }

    [TestMethod]
    public void TestMethod13() { Assert.AreEqual(7000.0, GetDiscountedPrice(10000)); }

    [TestMethod]
    public void TestMethod14() { Assert.AreEqual(7000.7, GetDiscountedPrice(10001)); }
    // Your code ends here
}
