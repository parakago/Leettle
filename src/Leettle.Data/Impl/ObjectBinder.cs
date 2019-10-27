using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Leettle.Data.Impl
{
    class ObjectBinder<T>
    {
        private readonly ObjectBinderProperty<T>[] objectBinderProperties;
        private readonly ObjectDefaultCreator<T> objectCreator;

        public ObjectBinder(ObjectBinderProperty<T>[] objectBinderProperties)
        {
            this.objectBinderProperties = objectBinderProperties;
            this.objectCreator = LeettleDbUtil.CreateObjectDefaultCreator<T>();
        }

        public T BindNewObject(DbDataReader dataReader)
        {
            T target = objectCreator();

            foreach (var objectBinderProperty in objectBinderProperties)
            {
                object columnValue = dataReader.IsDBNull(objectBinderProperty.FieldIndex) ? null : dataReader.GetValue(objectBinderProperty.FieldIndex);
                if (columnValue != null)
                {
                    objectBinderProperty.SetValue(target, columnValue);
                }
            }

            return target;
        }
    }
}
