using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leettle.Data.Test.Client
{
    class OracleTestClient : AbstractTestClient
    {
        public OracleTestClient(string connectionString) : base(connectionString, typeof(Oracle.ManagedDataAccess.Client.OracleConnection))
        {
            SqlCreateTable = string.Format(@"
create table {0} (
    v_string    varchar2(128),
    v_short     number(5),
    v_int       number(10),
    v_long      number(20),
    v_double    number(15,9),
    v_decimal   number(28,7),
    v_date_time date,
    v_blob      blob,
    v_long_text clob
)", TEST_TABLE_NAME).Trim();
        }
    }
}
