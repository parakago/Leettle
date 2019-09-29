using System;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    class Connection : IConnection, IDisposable
    {
        private DbConnection dbCon;
        private DbTransaction dbTrans;
        private BindStrategy bindStrategy;
        private IPreparedSqlProvider sqlProvider;

        public Connection(DbConnection dbCon, BindStrategy bindStrategy, IPreparedSqlProvider sqlProvider)
        {
            this.dbCon = dbCon;
            this.bindStrategy = bindStrategy;
            this.sqlProvider = sqlProvider;
        }

        public void Dispose()
        {
            dbCon.Dispose();
        }

        private DbCommand CreateDbCommand(string sql)
        {
            var dbCmd = dbCon.CreateCommand();
            dbCmd.CommandText = sql;
            if (dbTrans != null)
            {
                dbCmd.Transaction = dbTrans;
            }
            return dbCmd;
        }

        private DbCommandWrapper CreateDbCommandWrapper(string sql)
        {
            return new DbCommandWrapper(CreateDbCommand(sql), bindStrategy);
        }

        public IDataset NewDataset(string sql)
        {
            return new Dataset(CreateDbCommandWrapper(sql));
        }

        public ICommand NewCommand(string sql)
        {
            return new Command(CreateDbCommandWrapper(sql));
        }

        public void RunInTransaction(Action<IConnection> job)
        {
            if (dbTrans != null)
            {
                throw new Exception("transaction already started!!!");
            }

            try
            {
                dbTrans = dbCon.BeginTransaction();

                job.Invoke(this);

                dbTrans.Commit();
            }
            catch
            {
                dbTrans.Rollback();
                throw;
            }
            finally
            {
                dbTrans = null;
            }
        }

        public IDataset PreparedDataset(string sqlId)
        {
            string sql = sqlProvider.GetSql(sqlId);
            return NewDataset(sql);
        }

        public ICommand PreparedCommand(string sqlId)
        {
            string sql = sqlProvider.GetSql(sqlId);
            return NewCommand(sql);
        }
    }
}
