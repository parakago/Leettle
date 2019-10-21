﻿using System;

namespace Leettle.Data
{
    class EmptySqlProvider : IPreparedSqlProvider
    {
        public static EmptySqlProvider Instance = new EmptySqlProvider();

        public string GetSql(string queryId)
        {
            throw new Exception("create LeettleDb instance with PreparedQueryProvider");
        }
    }
}
