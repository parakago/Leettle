using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Leettle.Data
{
    public class SqlException : Exception
    {
        public string Sql { get; }
        public Dictionary<string, object> Parameters { get; }
        public SqlException(string sql, Dictionary<string, object> parameters, Exception innerException) : base(innerException.Message, innerException)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public static SqlException Wrap(Exception innerException, DbCommand command)
        {
            var parameters = new Dictionary<string, object>();
            foreach (DbParameter dbParam in command.Parameters)
            {
                parameters.Add(dbParam.ParameterName, dbParam.Value);
            }
            return new SqlException(command.CommandText, parameters, innerException);
        }
    }
}
