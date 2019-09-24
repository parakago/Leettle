using System;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace Leettle.Data.Impl
{
    class DbCommandWrapper : IDisposable
    {
        private DbCommand dbCommand;
        public BindStrategy BindStrategy { get; }
        public DbCommandWrapper(DbCommand dbCommand, BindStrategy bindStrategy)
        {
            this.dbCommand = dbCommand;
            this.BindStrategy = bindStrategy;
        }

        public virtual void Dispose()
        {
            LeettleDbUtil.DisposeSilently(dbCommand);
        }

        public DbCommandWrapper AddParam(string paramName, object paramValue)
        {
            Assert.NotNull(paramName, "paramName must not be null");

            var dbParam = dbCommand.CreateParameter();
            dbParam.ParameterName = paramName;
            dbParam.Value = paramValue == null ? DBNull.Value : paramValue;
            dbCommand.Parameters.Add(dbParam);
            return this;
        }

        public DbCommandWrapper BindParam(object paramObject)
        {
            Assert.NotNull(paramObject, "paramObject must not be null");

            Type paramType = paramObject.GetType();

            var matches = new Regex("(" + BindStrategy.ParameterMarker + "[A-Za-z0-9_$#]*)").Matches(dbCommand.CommandText);
            foreach (var match in matches)
            {
                string parameterName = match.ToString().Substring(1);
                string propertyName = BindStrategy.ToPropertyName(parameterName);
                var property = LeettleDbUtil.FindProperty(paramType, propertyName);
                if (property != null)
                {
                    object paramValue = property.GetValue(paramObject);
                    AddParam(parameterName, paramValue);
                }
            }
            
            return this;
        }

        public int ExecuteNonQuery()
        {
            try
            {
                int affected = dbCommand.ExecuteNonQuery();

                dbCommand.Parameters.Clear();

                return affected;
            }
            catch (Exception e)
            {
                throw LeettleDbQueryException.Wrap(e, dbCommand);
            }
        }

        public DbDataReader ExecuteReader()
        {
            try
            {
                return dbCommand.ExecuteReader();
            }
            catch (Exception e)
            {
                throw LeettleDbQueryException.Wrap(e, dbCommand);
            }
        }
    }
}
