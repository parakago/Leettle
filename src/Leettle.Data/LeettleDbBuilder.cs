using Leettle.Data.Impl;
using System;
using System.Data.Common;

namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class LeettleDbBuilder
    {
        private string connectionString;
        private Type dbConnectionType;
        private BindStrategy bindStrategy;
        private IPreparedSqlProvider sqlProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public LeettleDbBuilder WithConnectionString(string connectionString)
        {
            Assert.NotNull(connectionString, "connectionString must not be null");

            this.connectionString = connectionString;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnectionType"></param>
        /// <returns></returns>
        public LeettleDbBuilder WithConnectionType(Type dbConnectionType)
        {
            Assert.NotNull(dbConnectionType, "dbConnectionType must not be null");
            Assert.IsAssignable(typeof(DbConnection), dbConnectionType, "dbConnectionType must be subtype of System.Data.Common.DbConnection");

            this.dbConnectionType = dbConnectionType;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindStrategy"></param>
        /// <returns></returns>
        public LeettleDbBuilder WithBindStrategy(BindStrategy bindStrategy)
        {
            Assert.NotNull(bindStrategy, "bindStrategy must not be null");
            this.bindStrategy = bindStrategy;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlProvider"></param>
        /// <returns></returns>
        public LeettleDbBuilder WithPrepqredSqlProvider(IPreparedSqlProvider sqlProvider)
        {
            Assert.NotNull(sqlProvider, "queryProvider must not be null");
            this.sqlProvider = sqlProvider;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public LeettleDb Build()
        {
            Assert.NotNull(connectionString, "connectionString must not be null; use WithConnectionString");
            Assert.NotNull(dbConnectionType, "connectionString must not be null; use WithConnectionType");
            if (bindStrategy == null)
            {
                bindStrategy = new CleanBindStrategy(':');
            }
            if (sqlProvider == null)
            {
                sqlProvider = new EmptySqlProvider();
            }
            return new LeettleDbImpl(connectionString, dbConnectionType, bindStrategy, sqlProvider);
        }
    }
}
