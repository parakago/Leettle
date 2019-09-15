using Leettle.Data.Impl;
using System;
using System.Data.Common;

namespace Leettle.Data
{
    public class DonkeyBuilder
    {
        private string connectionString;
        private Type dbConnectionType;
        
        public DonkeyBuilder WithConnectionString(string connectionString)
        {
            Assert.NotNull(connectionString, "connectionString must not be null");

            this.connectionString = connectionString;
            return this;
        }
        public DonkeyBuilder WithConnectionType(Type dbConnectionType)
        {
            Assert.NotNull(dbConnectionType, "dbConnectionType must not be null");
            Assert.isAssignable(typeof(DbConnection), dbConnectionType, "dbConnectionType must be subtype of System.Data.Common.DbConnection");

            this.dbConnectionType = dbConnectionType;
            return this;
        }
        public Donkey Build()
        {
            Assert.NotNull(connectionString, "connectionString must not be null; use WithConnectionString");
            Assert.NotNull(dbConnectionType, "connectionString must not be null; use WithConnectionType");

            return new DonkeyImpl(connectionString, dbConnectionType);
        }
    }
}
