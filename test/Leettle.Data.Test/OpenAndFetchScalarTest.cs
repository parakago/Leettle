using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class OpenAndFetchScalarTest
    {
        [TestMethod]
        public void PostgresOpenAndFetchScalarTest()
        {
            TestClientProvider.Postgres().TestOpenAndFetchScalar();
        }

        [TestMethod]
        public void MySQLOpenAndFetchScalarTest()
        {
            TestClientProvider.MySQL().TestOpenAndFetchScalar();
        }

        [TestMethod]
        public void SQLiteOpenAndFetchScalarTest()
        {
            TestClientProvider.SQLite().TestOpenAndFetchScalar();
        }

        [TestMethod]
        public void OracleOpenAndFetchScalarTest()
        {
            TestClientProvider.Oracle().TestOpenAndFetchScalar();
        }

        [TestMethod]
        public void SqlServerOpenAndFetchScalarTest()
        {
            TestClientProvider.SqlServer().TestOpenAndFetchScalar();
        }
    }
}
