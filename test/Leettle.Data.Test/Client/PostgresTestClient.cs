using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leettle.Data.Test.Client
{
    class PostgresTestClient : AbstractTestClient
    {
        public PostgresTestClient(string connectionString) : base(connectionString, typeof(Npgsql.NpgsqlConnection))
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar(128),
    v_short     smallint,
    v_int       integer,
    v_long      bigint,
    v_double    numeric(15, 9),
    v_decimal   numeric(28, 7),
    v_date_time timestamp,
    v_blob      bytea,
    v_long_text text
)", TEST_TABLE_NAME).Trim();
        }
    }
}
