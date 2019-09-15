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
                             "    v_string, v_short, v_int, v_long, v_double, v_decimal, v_datetime, v_blob1, v_blob2, v_long_text)" + Environment.NewLine +
                             "values (" + Environment.NewLine +
                             "    :v_string, :v_short, :v_int, :v_long, :v_double, :v_decimal, :v_datetime, :v_blob1, :v_blob2, :v_long_text)";
        }

        private string ToProperSql(string sql)
        {
            return ParamIndicator.Equals(':') ? sql : sql.Replace(':', ParamIndicator);
        }

        public void Run()
        {
            var donkey = new DonkeyBuilder()
                .WithConnectionString(ConnectionString)
                .WithConnectionType(DbConnectionType)
                .Build();
            
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
                    string vLongText = SqlCreateTable;

                    using (var command = con.NewCommand(ToProperSql(SqlInsertInto)))
                    {
                        command.SetParam("v_string", vString)
                            .SetParam("v_short", vShort)
                            .SetParam("v_int", vInt)
                            .SetParam("v_long", vLong)
                            .SetParam("v_double", vDouble)
                            .SetParam("v_decimal", vDecimal)
                            .SetParam("v_datetime", vDateTime)
                            .SetParam("v_blob1", vBlob1)
                            .SetParam("v_blob2", vBlob2)
                            .SetParam("v_long_text", vLongText)
                            .Execute();
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

                        Assert.AreEqual(vLongText, dataset.GetString("v_long_text"));
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
