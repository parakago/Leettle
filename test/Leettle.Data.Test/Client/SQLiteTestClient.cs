using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leettle.Data.Test.Client
{
    class SQLiteTestClient : AbstractTestClient
    {
        public SQLiteTestClient(string connectionString) : base(connectionString, typeof(System.Data.SQLite.SQLiteConnection))
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar(128),
    v_short     smallint,
    v_int       int,
    v_long      bigint,
    v_double    decimal(15, 9),
    v_decimal   decimal(28, 7),
    v_date_time datetime,
    v_blob      blob,
    v_long_text text
)", TEST_TABLE_NAME).Trim();
        }
    }
}
