using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

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
            var objectBinderPropertyList = new List<ObjectBinderProperty<T>>(DataReader.FieldCount);

            for (int i = 0; i < DataReader.FieldCount; ++i)
            {
                string columnName = DataReader.GetName(i);
                string propertyName = bindStrategy.ToPropertyName(columnName);
                var propertyInfo = LeettleDbUtil.FindProperty(typeof(T), propertyName);
                if (propertyInfo != null)
                {
                    objectBinderPropertyList.Add(new ObjectBinderProperty<T>(i, propertyInfo));
                }
            }

            return new ObjectBinder<T>(objectBinderPropertyList.ToArray());
        }
    }
}
