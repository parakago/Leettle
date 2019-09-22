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
            Assert.AreEqual("HelloWorld", LeettleDbUtil.SnakeToCamel("helLo_wOrld"));
        }

        [TestMethod]
        public void TestCamelToSnake()
        {
            Assert.AreEqual("hello_world", LeettleDbUtil.CamelToSnake("HelloWorld"));
            Assert.AreEqual("h_ello_worl_d", LeettleDbUtil.CamelToSnake("HElloWorlD"));
            Assert.AreEqual("hello", LeettleDbUtil.CamelToSnake("Hello"));
        }
    }
}
