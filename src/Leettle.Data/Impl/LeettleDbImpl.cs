using System;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    public class LeettleDbImpl : LeettleDb
    {
        private string connectionString;
        private Type dbConnectionType;
        private BindStrategy bindStrategy;
        private IPreparedSqlProvider sqlProvider;

        public LeettleDbImpl(string connectionString, Type dbConnectionType, BindStrategy bindStrategy, IPreparedSqlProvider sqlProvider)
        {
            this.connectionString = connectionString;
            this.dbConnectionType = dbConnectionType;
            this.bindStrategy = bindStrategy;
            this.sqlProvider = sqlProvider;
        }

        public IConnection OpenConnection()
        {
            var dbConnection = (DbConnection)Activator.CreateInstance(dbConnectionType);
            dbConnection.ConnectionString = connectionString;
            dbConnection.Open();

            return new Connection(dbConnection, bindStrategy, sqlProvider);
        }
    }
}
