using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class LeettleDbRawApiTest
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
    }
}
