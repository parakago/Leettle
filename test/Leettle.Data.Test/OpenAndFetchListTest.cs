using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class OpenAndFetchListTest
    {
        [TestMethod]
        public void PostgresOpenAndFetchListTest()
        {
            TestClientProvider.Postgres().TestOpenAndFetchList();
        }

        [TestMethod]
        public void MySQLOpenAndFetchListTest()
        {
            TestClientProvider.MySQL().TestOpenAndFetchList();
        }

        [TestMethod]
        public void SQLiteOpenAndFetchListTest()
        {
            TestClientProvider.SQLite().TestOpenAndFetchList();
        }

        [TestMethod]
        public void OracleOpenAndFetchListTest()
        {
            TestClientProvider.Oracle().TestOpenAndFetchList();
        }

        [TestMethod]
        public void SqlServerOpenAndFetchListTest()
        {
            TestClientProvider.SqlServer().TestOpenAndFetchList();
        }
    }
}
