using System;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    class AbstractQuery : IDisposable
    {
        private DbCommand dbCommand;
        public AbstractQuery(DbCommand dbCommand)
        {
            this.dbCommand = dbCommand;
        }

        public virtual void Dispose()
        {
            Util.DisposeSilently(dbCommand);
        }

        protected AbstractQuery AddParam(string paramName, object paramValue)
        {
            var dbParam = dbCommand.CreateParameter();
            dbParam.ParameterName = paramName;
            dbParam.Value = paramValue;
            dbCommand.Parameters.Add(dbParam);
            return this;
        }

        protected int ExecuteNonQuery()
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

        protected DbDataReader ExecuteReader()
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
