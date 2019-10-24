using BenchmarkDotNet.Attributes;
using NHibernate;
using NHibernate.Cfg;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leettle.Data.Benchmark
{
    [ClrJob]
    [KeepBenchmarkFiles]
    [AsciiDocExporter]
    [CsvExporter]
    [CsvMeasurementsExporter]
    [HtmlExporter]
    [PlainExporter]
    [RPlotExporter]
    [JsonExporterAttribute.Brief]
    [JsonExporterAttribute.BriefCompressed]
    [JsonExporterAttribute.Full]
    [JsonExporterAttribute.FullCompressed]
    [MarkdownExporterAttribute.Default]
    [MarkdownExporterAttribute.GitHub]
    [MarkdownExporterAttribute.StackOverflow]
    [MarkdownExporterAttribute.Atlassian]
    [XmlExporterAttribute.Brief]
    [XmlExporterAttribute.BriefCompressed]
    [XmlExporterAttribute.Full]
    [XmlExporterAttribute.FullCompressed]
    public class OraMngVsLeettleVsNHiberSelectTest
    {
        const string CONNECTION_STRING = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.10.19)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=donkey;Password=donkey;";
        private readonly LeettleDb leettleDb;
        private readonly ISessionFactory sessionFactory;
        public OraMngVsLeettleVsNHiberSelectTest()
        {
            leettleDb = new LeettleDbBuilder()
                .WithConnectionString(CONNECTION_STRING)
                .WithConnectionType(typeof(Oracle.ManagedDataAccess.Client.OracleConnection))
                .WithBindStrategy(new CleanBindStrategy(':'))
                .Build();

            this.sessionFactory = new Configuration()
                .Configure("hibernate.cfg.xml")
                .AddFile("hibernate.hbm.xml")
                .BuildSessionFactory();
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            using (var con = leettleDb.OpenConnection())
            {
                con.NewCommand("CREATE TABLE BT_USER (ID NUMBER, NAME VARCHAR2(128), AGE NUMBER, ADDR1 VARCHAR2(256), ADDR2 VARCHAR2(256), FCDT DATE, LUDT DATE)").Execute();
                con.RunInTransaction((tx) =>
                {
                    for (int i = 0; i < 10000; ++i)
                    {
                        con.NewCommand("INSERT INTO BT_USER VALUES (:ID, :NAME, :AGE, :ADDR1, :ADDR2, :FCDT, :LUDT)")
                            .BindParam(User.NewUser(i))
                            .Execute();
                    }
                });
            }
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            using (var con = leettleDb.OpenConnection())
            {
                con.NewCommand("DROP TABLE BT_USER").Execute();
            }
        }

        [Benchmark]
        public void SelectAndFetchWithLeettle()
        {
            using (var con = leettleDb.OpenConnection())
            {
                List<User> users = con.NewDataset("SELECT ID, NAME, AGE, ADDR1, ADDR2, FCDT, LUDT FROM BT_USER").OpenAndFetchList<User>();
            }
        }

        [Benchmark]
        public void SelectAndFetchWithLeettleRawByIndex()
        {
            using (var con = leettleDb.OpenConnection())
            {
                List<User> users = new List<User>();
                con.NewDataset("SELECT ID, NAME, AGE, ADDR1, ADDR2, FCDT, LUDT FROM BT_USER").Open(dr =>
                {
                    while (dr.Next())
                    {
                        users.Add(new User
                        {
                            ID = dr.GetInt(0),
                            NAME = dr.GetString(1),
                            AGE = dr.GetInt(2),
                            ADDR1 = dr.GetString(3),
                            ADDR2 = dr.GetString(4),
                            FCDT = dr.GetDateTime(5),
                            LUDT = dr.GetDateTime(6)
                        });
                    }
                });
            }
        }

        [Benchmark]
        public void SelectAndFetchWithLeettleRawByName()
        {
            using (var con = leettleDb.OpenConnection())
            {
                List<User> users = new List<User>();
                con.NewDataset("SELECT ID, NAME, AGE, ADDR1, ADDR2, FCDT, LUDT FROM BT_USER").Open(dr =>
                {
                    while (dr.Next())
                    {
                        users.Add(new User
                        {
                            ID = dr.GetInt("ID"),
                            NAME = dr.GetString("NAME"),
                            AGE = dr.GetInt("AGE"),
                            ADDR1 = dr.GetString("ADDR1"),
                            ADDR2 = dr.GetString("ADDR2"),
                            FCDT = dr.GetDateTime("FCDT"),
                            LUDT = dr.GetDateTime("LUDT")
                        }); 
                    }
                });
            }
        }

        [Benchmark(Baseline = true)]
#pragma warning disable CA1822 // Mark members as static
        public void SelectAndFetchWithOMDA()
#pragma warning restore CA1822 // Mark members as static
        {
            List<User> users = new List<User>();

            using (var con = new OracleConnection(CONNECTION_STRING))
            {
                con.Open();
                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT ID, NAME, AGE, ADDR1, ADDR2, FCDT, LUDT FROM BT_USER";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                ID = reader.GetInt32(0),
                                NAME = reader.GetString(1),
                                AGE = reader.GetInt32(2),
                                ADDR1 = reader.GetString(3),
                                ADDR2 = reader.GetString(4),
                                FCDT = reader.GetDateTime(5),
                                LUDT = reader.GetDateTime(6),
                            });
                        }
                    }
                }
            }
        }
        
        [Benchmark]
        public void SelectAndFetchWithNHibernate()
        {
            using (var session = sessionFactory.OpenSession())
            {
                List<User> users = session.Query<User>().ToList();
            }
            
        }
    }

    public class User
    {
        public virtual int ID { get; set; }
        public virtual string NAME { get; set; }
        public virtual int AGE { get; set; }
        public virtual string ADDR1 { get; set; }
        public virtual string ADDR2 { get; set; }
        public virtual DateTime FCDT { get; set; }
        public virtual DateTime LUDT { get; set; }

        private static readonly Random random = new Random();
        public static User NewUser(int id)
        {
            return new User
            {
                ID = id,
                NAME = "홍길동1",
                AGE = random.Next(1, 100),
                ADDR1 = "MyAddr1 Addr1 Addr1 " + id,
                ADDR2 = "MyAddr2 Addr2 Addr2 " + id,
                FCDT = DateTime.Now,
                LUDT = DateTime.Now,
            };
        }
    }
}
