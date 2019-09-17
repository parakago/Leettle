using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class LeettleDbApiTest
    {
        [TestMethod]
        public void PostgresRawApiTest()
        {
            TestClientProvider.Postgres().TestRawApi();
        }

        [TestMethod]
        public void MySQLRawApiTest()
        {
            TestClientProvider.MySQL().TestRawApi();
        }

        [TestMethod]
        public void SQLiteRawApiTest()
        {
            TestClientProvider.SQLite().TestRawApi();
        }

        [TestMethod]
        public void OracleRawApiTest()
        {
            TestClientProvider.Oracle().TestRawApi();
        }

        [TestMethod]
        public void SqlServerRawApiTest()
        {
            TestClientProvider.SqlServer().TestRawApi();
        }

        [TestMethod]
        public void PostgresEasyApiTest()
        {
            TestClientProvider.Postgres().TestEasyApi();
        }

        [TestMethod]
        public void MySQLEasyApiTest()
        {
            TestClientProvider.MySQL().TestEasyApi();
        }

        [TestMethod]
        public void SQLiteEasyApiTest()
        {
            TestClientProvider.SQLite().TestEasyApi();
        }

        [TestMethod]
        public void OracleEasyApiTest()
        {
            TestClientProvider.Oracle().TestEasyApi();
        }

        [TestMethod]
        public void SqlServerEasyApiTest()
        {
            TestClientProvider.SqlServer().TestEasyApi();
        }
    }
}
