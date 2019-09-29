using System;
using System.Collections.Generic;
using System.Text;

namespace Leettle.Data
{
    public interface IPreparedSqlProvider
    {
        string GetSql(string sqlId);
    }
}
