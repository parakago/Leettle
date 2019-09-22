using Leettle.Data.Impl;
using System;
using System.Data.Common;

namespace Leettle.Data
{
    public class LeettleDbBuilder
    {
        private string connectionString;
        private Type dbConnectionType;
        private BindStrategy bindStrategy;


        public LeettleDbBuilder WithConnectionString(string connectionString)
        {
            Assert.NotNull(connectionString, "connectionString must not be null");

            this.connectionString = connectionString;
            return this;
        }
        public LeettleDbBuilder WithConnectionType(Type dbConnectionType)
        {
            Assert.NotNull(dbConnectionType, "dbConnectionType must not be null");
            Assert.isAssignable(typeof(DbConnection), dbConnectionType, "dbConnectionType must be subtype of System.Data.Common.DbConnection");

            this.dbConnectionType = dbConnectionType;
            return this;
        }

        public LeettleDbBuilder WithBindStrategy(BindStrategy bindStrategy)
        {
            Assert.NotNull(bindStrategy, "bindStrategy must not be null");
            this.bindStrategy = bindStrategy;
            return this;
        }
        public LeettleDb Build()
        {
            Assert.NotNull(connectionString, "connectionString must not be null; use WithConnectionString");
            Assert.NotNull(dbConnectionType, "connectionString must not be null; use WithConnectionType");
            if (bindStrategy == null)
            {
                bindStrategy = new CleanBindStrategy(':');
            }
            return new LeettleDbImpl(connectionString, dbConnectionType, bindStrategy);
        }
    }
}
