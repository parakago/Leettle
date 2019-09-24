using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class BindStrategyTest
    {
        [TestMethod]
        public void TestCamelObjectSnakeDbBindStrategy()
        {
            var strategy = new CamelObjectSnakeDbBindStrategy(':');
            Assert.AreEqual("HelloWorld", strategy.ToPropertyName("hello_world"));
        }

        [TestMethod]
        public void TestCleanBindStrategy()
        {
            var strategy = new CleanBindStrategy(':');
            Assert.AreEqual("hello_world", strategy.ToPropertyName("hello_world"));
        }

        [TestMethod]
        public void PostgresBindStrategyTest()
        {
            TestClientProvider.Postgres().TestBindStrategy();
        }

        [TestMethod]
        public void MySQLBindStrategyTest()
        {
            TestClientProvider.MySQL().TestBindStrategy();
        }

        [TestMethod]
        public void SQLiteBindStrategyTest()
        {
            TestClientProvider.SQLite().TestBindStrategy();
        }

        [TestMethod]
        public void OracleBindStrategyTest()
        {
            TestClientProvider.Oracle().TestBindStrategy();
        }

        [TestMethod]
        public void SqlServerBindStrategyTest()
        {
            TestClientProvider.SqlServer().TestBindStrategy();
        }
    }
}
