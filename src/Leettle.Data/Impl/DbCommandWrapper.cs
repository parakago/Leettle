using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Leettle.Data.Impl
{
    class DbCommandWrapper
    {
        private readonly Connection connection;
        public BindStrategy BindStrategy { get; }
        private readonly string sql;
        private Dictionary<string, object> parameters;

        public DbCommandWrapper(Connection connection, BindStrategy bindStrategy, string sql)
        {
            this.connection = connection;
            this.BindStrategy = bindStrategy;
            this.sql = sql;
        }

        public DbCommandWrapper AddParam(string paramName, object paramValue)
        {
            Assert.NotNull(paramName, "paramName must not be null");
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
            }
            parameters.Add(paramName, paramValue);

            return this;
        }

        public DbCommandWrapper BindParam(object paramObject)
        {
            Assert.NotNull(paramObject, "paramObject must not be null");

            Type paramType = paramObject.GetType();

            var matches = new Regex("(" + BindStrategy.ParameterMarker + "[A-Za-z0-9_$#]*)").Matches(sql);
            foreach (var match in matches)
            {
                string parameterName = match.ToString().Substring(1);
                string propertyName = BindStrategy.ToPropertyName(parameterName);
                var property = LeettleDbUtil.FindProperty(paramType, propertyName);
                if (property != null)
                {
                    object paramValue = property.GetValue(paramObject, null);
                    AddParam(parameterName, paramValue);
                }
            }
            
            return this;
        }

        private void SetDbCommandParam(DbCommand dbCommand)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var dbParam = dbCommand.CreateParameter();
                    dbParam.ParameterName = parameter.Key;
                    dbParam.Value = parameter.Value ?? DBNull.Value;
                    dbCommand.Parameters.Add(dbParam);
                }
            }
        }

        public int ExecuteNonQuery()
        {
            using (var dbCommand = connection.CreateDbCommand(sql))
            {
                try
                {
                    SetDbCommandParam(dbCommand);
                    return dbCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw LeettleDbQueryException.Wrap(e, dbCommand);
                }
            }
        }

        public void ExecuteReader(Action<DbDataReader> consumer)
        {
            using (var dbCommand = connection.CreateDbCommand(sql))
            {
                try
                {
                    SetDbCommandParam(dbCommand);
                    using (var dbReader = dbCommand.ExecuteReader())
                    {
                        consumer.Invoke(dbReader);
                    }
                }
                catch (Exception e)
                {
                    throw LeettleDbQueryException.Wrap(e, dbCommand);
                }
            }
        }
    }
}
