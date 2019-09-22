using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class LeettleDbEasyApiTest
    {
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
