using System.Collections.Generic;
using System.Data.Common;

namespace Leettle.Data.Impl
{
    class DbDataReaderWrapper
    {
        public DbDataReader DataReader { get; }
        private readonly BindStrategy bindStrategy;
        public DbDataReaderWrapper(DbDataReader dataReader, BindStrategy bindStrategy)
        {
            this.DataReader = dataReader;
            this.bindStrategy = bindStrategy;
        }

        public ObjectBinder<T> CreateObjectBinder<T>()
        {
            var fieldPropMappings = new List<FieldPropMapping<T>>(DataReader.FieldCount);

            for (int i = 0; i < DataReader.FieldCount; ++i)
            {
                string columnName = DataReader.GetName(i);
                string propertyName = bindStrategy.ToPropertyName(columnName);
                var propertyInfo = LeettleDbUtil.FindProperty(typeof(T), propertyName);
                if (propertyInfo != null)
                {
                    fieldPropMappings.Add(new FieldPropMapping<T>(i, propertyInfo));
                }
            }

            return new ObjectBinder<T>(fieldPropMappings.ToArray());
        }
    }
}
