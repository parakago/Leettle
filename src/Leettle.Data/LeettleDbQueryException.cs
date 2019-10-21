using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "<보류 중>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<보류 중>")]
    public class LeettleDbQueryException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public string Sql { get; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> Parameters { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="innerException"></param>
        public LeettleDbQueryException(string sql, Dictionary<string, object> parameters, Exception innerException) : base(innerException.Message, innerException)
        {
            Sql = sql;
            Parameters = parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="command"></param>
        /// <returns></returns>
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
