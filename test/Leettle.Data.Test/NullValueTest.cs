using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class NullValueTest
    {
        [TestMethod]
        public void PostgresRawApiTest()
        {
            TestClientProvider.Postgres().TestNullValue();
        }

        [TestMethod]
        public void MySQLRawApiTest()
        {
            TestClientProvider.MySQL().TestNullValue();
        }

        [TestMethod]
        public void SQLiteRawApiTest()
        {
            TestClientProvider.SQLite().TestNullValue();
        }

        [TestMethod]
        public void OracleRawApiTest()
        {
            TestClientProvider.Oracle().TestNullValue();
        }

        [TestMethod]
        public void SqlServerRawApiTest()
        {
            TestClientProvider.SqlServer().TestNullValue();
        }
    }
}
