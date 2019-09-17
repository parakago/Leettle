using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leettle.Data.Test.Client
{
    class SqlServerTestClient : AbstractTestClient
    {
        public SqlServerTestClient(string connectionString) : base(connectionString, typeof(System.Data.SqlClient.SqlConnection), '@')
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar(128),
    v_short     smallint,
    v_int       int,
    v_long      bigint,
    v_double    numeric(15, 9),
    v_decimal   numeric(28, 7),
    v_date_time datetime,
    v_blob      varbinary(max),
    v_long_text varchar(MAX)
)", TEST_TABLE_NAME).Trim();
        }
    }
}
