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

    class PostgreSQLTest : DonkeyRawApiTestTarget
    {
        public PostgreSQLTest(string connectionString) : base(connectionString, typeof(Npgsql.NpgsqlConnection))
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar(128),
    v_short     smallint,
    v_int       integer,
    v_long      bigint,
    v_double    numeric(15, 9),
    v_decimal   numeric(28, 7),
    v_datetime  timestamp,
    v_blob1     bytea,
    v_blob2     bytea,
    v_long_text text
)", TEST_TABLE_NAME).Trim();
        }
    }

    class MySQLTest : DonkeyRawApiTestTarget
    {
        public MySQLTest(string connectionString) : base(connectionString, typeof(MySql.Data.MySqlClient.MySqlConnection), '@')
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar(128),
    v_short     smallint,
    v_int       int,
    v_long      bigint,
    v_double    decimal(15, 9),
    v_decimal   decimal(28, 7),
    v_datetime  datetime,
    v_blob1     blob,
    v_blob2     blob,
    v_long_text text
)", TEST_TABLE_NAME).Trim();
        }
    }

    class SQLiteTest : DonkeyRawApiTestTarget
    {
        public SQLiteTest(string connectionString) : base(connectionString, typeof(System.Data.SQLite.SQLiteConnection))
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar(128),
    v_short     smallint,
    v_int       int,
    v_long      bigint,
    v_double    decimal(15, 9),
    v_decimal   decimal(28, 7),
    v_datetime  datetime,
    v_blob1     blob,
    v_blob2     blob,
    v_long_text text
)", TEST_TABLE_NAME).Trim();
        }
    }

    class OracleTest : DonkeyRawApiTestTarget
    {
        public OracleTest(string connectionString) : base(connectionString, typeof(Oracle.ManagedDataAccess.Client.OracleConnection))
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar2(128),
    v_short     number(5),
    v_int       number(10),
    v_long      number(20),
    v_double    number(15,9),
    v_decimal   number(28,7),
    v_datetime  date,
    v_blob1     blob,
    v_blob2     blob,
    v_long_text clob
)", TEST_TABLE_NAME).Trim();
        }
    }

    class SqlServerTest : DonkeyRawApiTestTarget
    {
        public SqlServerTest(string connectionString) : base(connectionString, typeof(System.Data.SqlClient.SqlConnection), '@')
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar(128),
    v_short     smallint,
    v_int       int,
    v_long      bigint,
    v_double    numeric(15, 9),
    v_decimal   numeric(28, 7),
    v_datetime  datetime,
    v_blob1     varbinary(max),
    v_blob2     varbinary(max),
    v_long_text varchar(MAX)
)", TEST_TABLE_NAME).Trim();
        }
    }
}
