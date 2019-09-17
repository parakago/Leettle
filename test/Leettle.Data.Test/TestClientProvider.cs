using Leettle.Data.Test.Client;

namespace Leettle.Data.Test
{
    class TestClientProvider
    {
        public static PostgresTestClient Postgres()
        {
            return new PostgresTestClient("Host=192.168.10.19;Port=5432;Username=donkey;Password=donkey;Database=donkey;");
        }

        public static MySQLTestClient MySQL()
        {
            return new MySQLTestClient("Server=192.168.10.19;Port=3306;Database=donkey;CharSet=utf8;Uid=donkey;Pwd=donkey");
        }

        public static SQLiteTestClient SQLite()
        {
            return new SQLiteTestClient("Data Source=:memory:;Version=3;New=True;");
        }

        public static OracleTestClient Oracle()
        {
            return new OracleTestClient("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.10.19)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=donkey;Password=donkey;");
        }

        public static SqlServerTestClient SqlServer()
        {
            return new SqlServerTestClient("Data Source=192.168.10.19,1433;Network Library=DBMSSOCN;Initial Catalog=donkey; User ID=donkey; Password=donkey;");
        }
    }
}
