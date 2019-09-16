using System;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    class DbCommandWrapper : IDisposable
    {
        private DbCommand dbCommand;
        public DbCommandWrapper(DbCommand dbCommand)
        {
            this.dbCommand = dbCommand;
        }

        public virtual void Dispose()
        {
            Util.DisposeSilently(dbCommand);
        }

        public DbCommandWrapper AddParam(string paramName, object paramValue)
        {
            var dbParam = dbCommand.CreateParameter();
            dbParam.ParameterName = paramName;
            dbParam.Value = paramValue;
            dbCommand.Parameters.Add(dbParam);
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
                throw SqlException.Wrap(e, dbCommand);
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
                throw SqlException.Wrap(e, dbCommand);
            }
        }
    }
}
