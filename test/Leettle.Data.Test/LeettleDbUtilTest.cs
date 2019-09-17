using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class LeettleDbUtilTest
    {
        [TestMethod]
        public void TestSnakeToCamel()
        {
            Assert.AreEqual("HelloWorld", LeettleDbUtil.SnakeToCamel("hello_world"));
            Assert.AreEqual("HelloWorld", LeettleDbUtil.SnakeToCamel("__hello__world___"));
        }
    }
}
