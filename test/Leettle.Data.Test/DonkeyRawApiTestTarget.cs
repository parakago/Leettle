using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test
{
    abstract class DonkeyRawApiTestTarget
    {
        public const string TEST_TABLE_NAME = "DK_TEST";
        const string FORMAT_DATETIME = "yyyy-MM-dd HH:mm:ss";
        const string FORMAT_DATETIME_MYSQL = "yyyy-MM-dd HH:mm";
        public string ConnectionString { get; }
        public Type DbConnectionType { get; }
        public string SqlCreateTable { get; internal set; }
        public string SqlDropTable { get; }
        public string SqlInsertInto { get; }
        public char ParamIndicator { get; }

        public DonkeyRawApiTestTarget(string connectionString, Type dbConnectionType) : this(connectionString, dbConnectionType, ':')
        {
            
        }

        public DonkeyRawApiTestTarget(string connectionString, Type dbConnectionType, char paramIndicator)
        {
            ConnectionString = connectionString;
            DbConnectionType = dbConnectionType;
            ParamIndicator = paramIndicator;

            SqlDropTable = "drop table " + TEST_TABLE_NAME;
            SqlInsertInto = "insert into " + TEST_TABLE_NAME + " (" + Environment.NewLine +
                             "    v_string, v_short, v_int, v_long, v_double, v_decimal, v_datetime, v_blob1, v_blob2 )" + Environment.NewLine +
                             "values (" + Environment.NewLine +
                             "    :v_string, :v_short, :v_int, :v_long, :v_double, :v_decimal, :v_datetime, :v_blob1, :v_blob2)";
        }

        private string ToProperSql(string sql)
        {
            return ParamIndicator.Equals(':') ? sql : sql.Replace(':', ParamIndicator);
        }

        public void Run()
        {
            using (var donkey = new Donkey(ConnectionString, DbConnectionType))
            {
                using (var con = donkey.OpenConnection())
                {
                    using (var command = con.NewCommand(ToProperSql(SqlCreateTable)))
                    {
                        command.Execute();
                    }

                    try
                    {
                        string vString = "TEST 테스트";
                        short vShort = short.MaxValue;
                        int vInt = int.MaxValue;
                        long vLong = long.MaxValue;
                        double vDouble = 8.358674532;
                        decimal vDecimal = 9728337.1390397M;
                        DateTime vDateTime = DateTime.Now;
                        byte[] vBlob1 = Encoding.UTF8.GetBytes(SqlCreateTable);
                        MemoryStream vBlob2 = new MemoryStream(vBlob1);

                        using (var command = con.NewCommand(ToProperSql(SqlInsertInto)))
                        {
                            command.SetParam("v_string", vString);
                            command.SetParam("v_short", vShort);
                            command.SetParam("v_int", vInt);
                            command.SetParam("v_long", vLong);
                            command.SetParam("v_double", vDouble);
                            command.SetParam("v_decimal", vDecimal);
                            command.SetParam("v_datetime", vDateTime);
                            command.SetParam("v_blob1", vBlob1);
                            command.SetParam("v_blob2", vBlob2);
                            command.Execute();
                        }

                        using (var dataset = con.NewDataset(ToProperSql(string.Format("SELECT * FROM {0} WHERE v_string = :v_string", TEST_TABLE_NAME))))
                        {
                            dataset.SetParam("v_string", vString).Open();

                            Assert.IsTrue(dataset.Next());
                            Assert.AreEqual(vString, dataset.GetString("v_string"));
                            Assert.AreEqual(vShort, dataset.GetShort("v_short"));
                            Assert.AreEqual(vInt, dataset.GetInt("v_int"));
                            Assert.AreEqual(vLong, dataset.GetLong("v_long"));
                            Assert.AreEqual(vDouble, dataset.GetDouble("v_double"));
                            Assert.AreEqual(vDecimal, dataset.GetDecimal("v_decimal"));

                            string dateFormat = DbConnectionType.Name.ToLower().IndexOf("mysql") == -1 ? FORMAT_DATETIME : FORMAT_DATETIME_MYSQL;

                            Assert.AreEqual(vDateTime.ToString(dateFormat), dataset.GetDateTime("v_datetime").ToString(dateFormat));
                            
                            CollectionAssert.AreEqual(vBlob1, dataset.GetBytes("v_blob1"));
                            using (MemoryStream msBlob2 = new MemoryStream())
                            {
                                dataset.GetStream("v_blob1", msBlob2);
                                CollectionAssert.AreEqual(vBlob2.ToArray(), msBlob2.ToArray());
                            }
                        }
                    }
                    finally
                    {
                        using (var command = con.NewCommand(ToProperSql(SqlDropTable)))
                        {
                            command.Execute();
                        }
                    }
                }
            }
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
    v_blob2     bytea
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
    v_blob2     blob
)", TEST_TABLE_NAME).Trim();
        }
    }

    class SQLiteTest: DonkeyRawApiTestTarget
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
    v_blob2     blob
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
    v_blob2     blob
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
    v_blob2     varbinary(max)
)", TEST_TABLE_NAME).Trim();
        }
    }
}
