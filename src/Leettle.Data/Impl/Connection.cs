﻿using System;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    class Connection : IConnection, IDisposable
    {
        private DbConnection dbCon;
        private DbTransaction dbTrans;
        private BindStrategy bindStrategy;

        public Connection(DbConnection dbCon, BindStrategy bindStrategy)
        {
            this.dbCon = dbCon;
            this.bindStrategy = bindStrategy;
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

        public IRawDataset NewRawDataset(string sql)
        {
            return new RawDataset(CreateDbCommand(sql), bindStrategy);
        }

        public IDataset NewDataset(string sql)
        {
            return new Dataset(CreateDbCommandWrapper(sql));
        }

        public ICommand NewCommand(string sql)
        {
            return new Command(CreateDbCommandWrapper(sql));
        }

        public void RunInTransaction(TransactionJob transactionJob)
        {
            try
            {
                dbTrans = dbCon.BeginTransaction();

                transactionJob(this);

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
    }
}