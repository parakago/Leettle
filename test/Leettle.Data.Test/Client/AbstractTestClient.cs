using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Leettle.Data.Test.Client
{
    abstract class AbstractTestClient
    {
        protected const string TEST_TABLE_NAME = "DK_TEST";
        
        public string ConnectionString { get; }
        public Type DbConnectionType { get; }
        public string SqlCreateTable { get; internal set; }
        public string SqlDropTable { get; }
        public string SqlInsertInto { get; }
        public char ParameterMarker { get; }

        delegate void TestTask(IConnection con);

        public AbstractTestClient(string connectionString, Type dbConnectionType) : this(connectionString, dbConnectionType, ':')
        {
            
        }

        public AbstractTestClient(string connectionString, Type dbConnectionType, char parameterMarker)
        {
            ConnectionString = connectionString;
            DbConnectionType = dbConnectionType;
            ParameterMarker = parameterMarker;

            SqlDropTable = "drop table " + TEST_TABLE_NAME;
            SqlInsertInto = "insert into " + TEST_TABLE_NAME + " (" + Environment.NewLine +
                             "    v_string, v_short, v_int, v_long, v_double, v_decimal, v_date_time, v_blob, v_long_text)" + Environment.NewLine +
                             "values (" + Environment.NewLine +
                             "    :v_string, :v_short, :v_int, :v_long, :v_double, :v_decimal, :v_date_time, :v_blob, :v_long_text)";
        }

        private string ToProperSql(string sql)
        {
            return ParameterMarker.Equals(':') ? sql : sql.Replace(':', ParameterMarker);
        }

        private void RunTest(TestTask testTask)
        {
            RunTest(new CleanBindStrategy(':'), testTask);
        }
        private void RunTest(BindStrategy bindStrategy, TestTask testTask)
        {
            var leettleDb = new LeettleDbBuilder()
                .WithConnectionString(ConnectionString)
                .WithConnectionType(DbConnectionType)
                .WithBindStrategy(bindStrategy)
                .Build();

            using (var con = leettleDb.OpenConnection())
            {
                try
                {
                    con.NewCommand(ToProperSql(SqlCreateTable)).Execute();

                    testTask(con);
                }
                finally
                {
                    con.NewCommand(ToProperSql(SqlDropTable)).Execute();
                }
            }

        }

        private TestTableSnakeCaseItem CreateNewRandomTestTableItem()
        {
            DateTime now = DateTime.Now;
            return new TestTableSnakeCaseItem()
            {
                v_string = "TEST 테스트",
                v_short = short.MaxValue,
                v_int = int.MaxValue,
                v_long = long.MaxValue,
                v_double = 8.358674532,
                v_decimal = 9728337.1390397M,
                v_date_time = now.AddTicks((now.Ticks * -1) % TimeSpan.TicksPerSecond),
                v_blob = Encoding.UTF8.GetBytes(SqlCreateTable),
                v_long_text = SqlCreateTable
            };
        }

        private void InsertToTestTable(IConnection con, TestTableSnakeCaseItem testTableItem)
        {
            con.NewCommand(ToProperSql(SqlInsertInto))
                .SetParam("v_string", testTableItem.v_string)
                .SetParam("v_short", testTableItem.v_short)
                .SetParam("v_int", testTableItem.v_int)
                .SetParam("v_long", testTableItem.v_long)
                .SetParam("v_double", testTableItem.v_double)
                .SetParam("v_decimal", testTableItem.v_decimal)
                .SetParam("v_date_time", testTableItem.v_date_time)
                .SetParam("v_blob", testTableItem.v_blob)
                .SetParam("v_long_text", testTableItem.v_long_text)
                .Execute();
        }

        public void TestOpen()
        {
            RunTest((con) =>
            {
                var testTableItem = CreateNewRandomTestTableItem();

                InsertToTestTable(con, testTableItem);

                string selectSql = ToProperSql(string.Format("SELECT * FROM {0} WHERE v_string = :v_string", TEST_TABLE_NAME));
                con.NewDataset(selectSql)
                    .SetParam("v_string", testTableItem.v_string)
                    .Open(rs =>
                {
                    Assert.IsTrue(rs.Next());
                    Assert.AreEqual(testTableItem.v_string, rs.GetString("v_string"));
                    Assert.AreEqual(testTableItem.v_short, rs.GetShort("v_short"));
                    Assert.AreEqual(testTableItem.v_int, rs.GetInt("v_int"));
                    Assert.AreEqual(testTableItem.v_long, rs.GetLong("v_long"));
                    Assert.AreEqual(testTableItem.v_double, rs.GetDouble("v_double"));
                    Assert.AreEqual(testTableItem.v_decimal, rs.GetDecimal("v_decimal"));
                    Assert.AreEqual(testTableItem.v_date_time, rs.GetDateTime("v_date_time"));
                    CollectionAssert.AreEqual(testTableItem.v_blob, rs.GetBytes("v_blob"));
                    Assert.AreEqual(testTableItem.v_long_text, rs.GetString("v_long_text"));
                });
            });
        }

        public void TestOpenAndFetchList()
        {
            RunTest((con) =>
            {
                var testTableItem = CreateNewRandomTestTableItem();

                InsertToTestTable(con, testTableItem);

                string sql = ToProperSql(string.Format("SELECT * FROM {0} WHERE v_string = :v_string", TEST_TABLE_NAME));
                var fetchedList = con.NewDataset(sql)
                    .SetParam("v_string", testTableItem.v_string)
                    .OpenAndFetchList<TestTableSnakeCaseItem>();

                Assert.AreEqual(1, fetchedList.Count);
                Assert.AreEqual(testTableItem.v_string, fetchedList[0].v_string);
                Assert.AreEqual(testTableItem.v_short, fetchedList[0].v_short);
                Assert.AreEqual(testTableItem.v_int, fetchedList[0].v_int);
                Assert.AreEqual(testTableItem.v_long, fetchedList[0].v_long);
                Assert.AreEqual(testTableItem.v_double, fetchedList[0].v_double);
                Assert.AreEqual(testTableItem.v_decimal, fetchedList[0].v_decimal);
                Assert.AreEqual(testTableItem.v_date_time, fetchedList[0].v_date_time);
                CollectionAssert.AreEqual(testTableItem.v_blob, fetchedList[0].v_blob);
                Assert.AreEqual(testTableItem.v_long_text, fetchedList[0].v_long_text);
            });
        }

        public void TestOpenAndFetchScalar()
        {
            RunTest((con) =>
            {
                var testTableItem = CreateNewRandomTestTableItem();

                InsertToTestTable(con, testTableItem);

                Assert.AreEqual(testTableItem.v_string, con.NewDataset(string.Format("select {0} from {1}", "v_string", TEST_TABLE_NAME)).OpenAndFetchScalar<string>());
                Assert.AreEqual(testTableItem.v_short, con.NewDataset(string.Format("select {0} from {1}", "v_short", TEST_TABLE_NAME)).OpenAndFetchScalar<short>());
                Assert.AreEqual(testTableItem.v_int, con.NewDataset(string.Format("select {0} from {1}", "v_int", TEST_TABLE_NAME)).OpenAndFetchScalar<int>());
                Assert.AreEqual(testTableItem.v_long, con.NewDataset(string.Format("select {0} from {1}", "v_long", TEST_TABLE_NAME)).OpenAndFetchScalar<long>());
                Assert.AreEqual(testTableItem.v_double, con.NewDataset(string.Format("select {0} from {1}", "v_double", TEST_TABLE_NAME)).OpenAndFetchScalar<double>());
                Assert.AreEqual(testTableItem.v_decimal, con.NewDataset(string.Format("select {0} from {1}", "v_decimal", TEST_TABLE_NAME)).OpenAndFetchScalar<decimal>());
                Assert.AreEqual(testTableItem.v_date_time, con.NewDataset(string.Format("select {0} from {1}", "v_date_time", TEST_TABLE_NAME)).OpenAndFetchScalar<DateTime>());
                CollectionAssert.AreEqual(testTableItem.v_blob, con.NewDataset(string.Format("select {0} from {1}", "v_blob", TEST_TABLE_NAME)).OpenAndFetchScalar<byte[]>());
                Assert.AreEqual(testTableItem.v_long_text, con.NewDataset(string.Format("select {0} from {1}", "v_long_text", TEST_TABLE_NAME)).OpenAndFetchScalar<string>());

                InsertToTestTable(con, testTableItem);
            });
        }

        public void TestBindStrategy()
        {
            RunTest(new CamelObjectSnakeDbBindStrategy(ParameterMarker), (con) =>
            {
                var testTableItem = CreateNewRandomTestTableItem();

                InsertToTestTable(con, testTableItem);

                var camelTableItem = new TestTableCamelCaseItem();
                camelTableItem.VString = testTableItem.v_string;

                string sql = ToProperSql(string.Format("SELECT * FROM {0} WHERE v_string = :v_string", TEST_TABLE_NAME));
                var fetched = con.NewDataset(sql)
                    .BindParam(camelTableItem)
                    .OpenAndFetch<TestTableCamelCaseItem>();

                Assert.AreEqual(testTableItem.v_string, fetched.VString);
                Assert.AreEqual(testTableItem.v_short, fetched.VShort);
                Assert.AreEqual(testTableItem.v_int, fetched.VInt);
                Assert.AreEqual(testTableItem.v_long, fetched.VLong);
                Assert.AreEqual(testTableItem.v_double, fetched.VDouble);
                Assert.AreEqual(testTableItem.v_decimal, fetched.VDecimal);
                Assert.AreEqual(testTableItem.v_date_time, fetched.VDateTime);
                CollectionAssert.AreEqual(testTableItem.v_blob, fetched.VBlob);
                Assert.AreEqual(testTableItem.v_long_text, fetched.VLongText);
            });
        }
    }

    class TestTableSnakeCaseItem
    {
        public string v_string { get; set; }
        public short v_short { get; set; }
        public int v_int { get; set; }
        public long v_long { get; set; }
        public double v_double { get; set; }
        public decimal v_decimal { get; set; }
        public DateTime v_date_time { get; set; }
        public byte[] v_blob { get; set; }
        public string v_long_text { get; set; }
    }

    class TestTableCamelCaseItem
    {
        public string VString { get; set; }
        public short VShort { get; set; }
        public int VInt { get; set; }
        public long VLong { get; set; }
        public double VDouble { get; set; }
        public decimal VDecimal { get; set; }
        public DateTime VDateTime { get; set; }
        public byte[] VBlob { get; set; }
        public string VLongText { get; set; }
    }
}
