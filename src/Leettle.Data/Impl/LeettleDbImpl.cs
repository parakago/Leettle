using System;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    class LeettleDbImpl : LeettleDb
    {
        private readonly string connectionString;
        private readonly Type dbConnectionType;
        private readonly BindStrategy bindStrategy;
        private readonly IPreparedSqlProvider sqlProvider;

        public LeettleDbImpl(string connectionString, Type dbConnectionType, BindStrategy bindStrategy, IPreparedSqlProvider sqlProvider)
        {
            this.connectionString = connectionString;
            this.dbConnectionType = dbConnectionType;
            this.bindStrategy = bindStrategy;
            this.sqlProvider = sqlProvider;
        }

        public override IConnection OpenConnection()
        {
            var dbConnection = (DbConnection)Activator.CreateInstance(dbConnectionType);
            dbConnection.ConnectionString = connectionString;
            dbConnection.Open();

            return new Connection(dbConnection, bindStrategy, sqlProvider);
        }
    }
}
