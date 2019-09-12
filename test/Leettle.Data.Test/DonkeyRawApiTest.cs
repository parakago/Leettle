using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Leettle.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    [TestClass]
    public class DonkeyRawApiTest
    {
        [TestMethod]
        public void RowApiPostgreSQLTest()
        {
            new PostgreSQLTest("Host=192.168.10.19;Port=5432;Username=donkey;Password=donkey;Database=donkey;").Run();
        }

        [TestMethod]
        public void RowApiMySQLTest()
        {
            new MySQLTest("Server=192.168.10.19;Port=3306;Database=donkey;CharSet=utf8;Uid=donkey;Pwd=donkey").Run();
        }

        [TestMethod]
        public void RowApiSQLiteTest()
        {
            new SQLiteTest("Data Source=:memory:;Version=3;New=True;").Run();
        }

        [TestMethod]
        public void RowApiOracleTest()
        {
            new OracleTest("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.10.19)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=donkey;Password=donkey;").Run();
        }

        [TestMethod]
        public void RowApiSqlServerTest()
        {
            new SqlServerTest("Data Source=192.168.10.19,1433;Network Library=DBMSSOCN;Initial Catalog=donkey; User ID=donkey; Password=donkey;").Run();
        }
    }
}
