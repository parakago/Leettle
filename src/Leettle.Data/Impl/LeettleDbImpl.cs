using System;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    public class LeettleDbImpl : LeettleDb
    {
        private string connectionString;
        private Type dbConnectionType;
        private BindStrategy bindStrategy;

        public LeettleDbImpl(string connectionString, Type dbConnectionType, BindStrategy bindStrategy)
        {
            this.connectionString = connectionString;
            this.dbConnectionType = dbConnectionType;
            this.bindStrategy = bindStrategy;
        }

        public IConnection OpenConnection()
        {
            var dbConnection = (DbConnection)Activator.CreateInstance(dbConnectionType);
            dbConnection.ConnectionString = connectionString;
            dbConnection.Open();

            return new Connection(dbConnection, bindStrategy);
        }
    }
}
