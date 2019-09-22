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

        public void TestRawApi()
        {
            RunTest((con) =>
            {
                var testTableItem = CreateNewRandomTestTableItem();

                InsertToTestTable(con, testTableItem);

                string selectSql = ToProperSql(string.Format("SELECT * FROM {0} WHERE v_string = :v_string", TEST_TABLE_NAME));
                using (var dataset = con.NewRawDataset(selectSql))
                {
                    dataset.SetParam("v_string", testTableItem.v_string).Open();

                    Assert.IsTrue(dataset.Next());
                    Assert.AreEqual(testTableItem.v_string, dataset.GetString("v_string"));
                    Assert.AreEqual(testTableItem.v_short, dataset.GetShort("v_short"));
                    Assert.AreEqual(testTableItem.v_int, dataset.GetInt("v_int"));
                    Assert.AreEqual(testTableItem.v_long, dataset.GetLong("v_long"));
                    Assert.AreEqual(testTableItem.v_double, dataset.GetDouble("v_double"));
                    Assert.AreEqual(testTableItem.v_decimal, dataset.GetDecimal("v_decimal"));
                    Assert.AreEqual(testTableItem.v_date_time, dataset.GetDateTime("v_date_time"));
                    CollectionAssert.AreEqual(testTableItem.v_blob, dataset.GetBytes("v_blob"));
                    Assert.AreEqual(testTableItem.v_long_text, dataset.GetString("v_long_text"));
                }
            });
        }

        public void TestEasyApi()
        {
            RunTest((con) =>
            {
                var testTableItem = CreateNewRandomTestTableItem();

                InsertToTestTable(con, testTableItem);

                string selectSql1 = ToProperSql(string.Format("SELECT * FROM {0} WHERE v_string = :v_string", TEST_TABLE_NAME));

                var fetchedItem = con.NewDataset(selectSql1)
                    .SetParam("v_string", testTableItem.v_string)
                    .OpenAndFetch<TestTableSnakeCaseItem>();

                Assert.IsNotNull(fetchedItem);
                Assert.AreEqual(testTableItem.v_string, fetchedItem.v_string);
                Assert.AreEqual(testTableItem.v_short, fetchedItem.v_short);
                Assert.AreEqual(testTableItem.v_int, fetchedItem.v_int);
                Assert.AreEqual(testTableItem.v_long, fetchedItem.v_long);
                Assert.AreEqual(testTableItem.v_double, fetchedItem.v_double);
                Assert.AreEqual(testTableItem.v_decimal, fetchedItem.v_decimal);
                Assert.AreEqual(testTableItem.v_date_time, fetchedItem.v_date_time);
                CollectionAssert.AreEqual(testTableItem.v_blob, fetchedItem.v_blob);
                Assert.AreEqual(testTableItem.v_long_text, fetchedItem.v_long_text);


                string selectSql2 = ToProperSql(string.Format("SELECT * FROM {0}", TEST_TABLE_NAME));
                var fetchedList = con.NewDataset(selectSql2)
                    .OpenAndFetchList<TestTableSnakeCaseItem>();

                Assert.IsNotNull(fetchedItem);
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

        public void TestBindStrategy()
        {
            RunTest(new CamelObjectSnakeDbBindStrategy(ParameterMarker), (con) =>
            {
                var testTableItem = CreateNewRandomTestTableItem();

                InsertToTestTable(con, testTableItem);

                var camelTableItem = new TestTableCamelCaseItem();
                camelTableItem.VString = testTableItem.v_string;
                camelTableItem.VShort = testTableItem.v_short;
                camelTableItem.VInt = testTableItem.v_int;
                camelTableItem.VLong = testTableItem.v_long;
                camelTableItem.VDouble = testTableItem.v_double;
                camelTableItem.VDecimal = testTableItem.v_decimal;
                camelTableItem.VDateTime = testTableItem.v_date_time;
                camelTableItem.VBlob = testTableItem.v_blob;
                camelTableItem.VLongText = testTableItem.v_long_text;

                string selectSql1 = ToProperSql(string.Format("SELECT * FROM {0} WHERE v_string = :v_string", TEST_TABLE_NAME));

                var fetchedItem = con.NewDataset(selectSql1)
                    .BindParam(camelTableItem)
                    .OpenAndFetch<TestTableCamelCaseItem>();

                Assert.IsNotNull(fetchedItem);
                Assert.AreEqual(camelTableItem.VString, fetchedItem.VString);
                Assert.AreEqual(camelTableItem.VShort, fetchedItem.VShort);
                Assert.AreEqual(camelTableItem.VInt, fetchedItem.VInt);
                Assert.AreEqual(camelTableItem.VLong, fetchedItem.VLong);
                Assert.AreEqual(camelTableItem.VDouble, fetchedItem.VDouble);
                Assert.AreEqual(camelTableItem.VDecimal, fetchedItem.VDecimal);
                Assert.AreEqual(camelTableItem.VDateTime, fetchedItem.VDateTime);
                CollectionAssert.AreEqual(camelTableItem.VBlob, fetchedItem.VBlob);
                Assert.AreEqual(camelTableItem.VLongText, fetchedItem.VLongText);


                string selectSql2 = ToProperSql(string.Format("SELECT * FROM {0}", TEST_TABLE_NAME));
                var fetchedList = con.NewDataset(selectSql2)
                    .OpenAndFetchList<TestTableCamelCaseItem>();

                Assert.IsNotNull(fetchedItem);
                Assert.AreEqual(1, fetchedList.Count);
                Assert.AreEqual(camelTableItem.VString, fetchedList[0].VString);
                Assert.AreEqual(camelTableItem.VShort, fetchedList[0].VShort);
                Assert.AreEqual(camelTableItem.VInt, fetchedList[0].VInt);
                Assert.AreEqual(camelTableItem.VLong, fetchedList[0].VLong);
                Assert.AreEqual(camelTableItem.VDouble, fetchedList[0].VDouble);
                Assert.AreEqual(camelTableItem.VDecimal, fetchedList[0].VDecimal);
                Assert.AreEqual(camelTableItem.VDateTime, fetchedList[0].VDateTime);
                CollectionAssert.AreEqual(camelTableItem.VBlob, fetchedList[0].VBlob);
                Assert.AreEqual(camelTableItem.VLongText, fetchedList[0].VLongText);
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
