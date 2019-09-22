using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Leettle.Data
{
    public class LeettleDbQueryException : Exception
    {
        public string Sql { get; }
        public Dictionary<string, object> Parameters { get; }
        public LeettleDbQueryException(string sql, Dictionary<string, object> parameters, Exception innerException) : base(innerException.Message, innerException)
        {
            Sql = sql;
            Parameters = parameters;
        }

        public static LeettleDbQueryException Wrap(Exception innerException, DbCommand command)
        {
            var parameters = new Dictionary<string, object>();
            foreach (DbParameter dbParam in command.Parameters)
            {
                parameters.Add(dbParam.ParameterName, dbParam.Value);
            }
            return new LeettleDbQueryException(command.CommandText, parameters, innerException);
        }
    }
}
