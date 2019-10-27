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
        private readonly FieldPropMapping<T>[] fieldPropMappingList;
        private readonly ObjectDefaultCreator<T> objectCreator;

        public ObjectBinder(FieldPropMapping<T>[] fieldPropMappingList)
        {
            this.fieldPropMappingList = fieldPropMappingList;
            this.objectCreator = LeettleDbUtil.CreateObjectDefaultCreator<T>();
        }

        public T BindNewObject(DbDataReader dataReader)
        {
            T target = objectCreator();

            foreach (var fieldPropMapping in fieldPropMappingList)
            {
                object columnValue = dataReader.IsDBNull(fieldPropMapping.FieldIndex) ? null : dataReader.GetValue(fieldPropMapping.FieldIndex);
                if (columnValue != null)
                {
                    fieldPropMapping.SetValue(target, columnValue);
                }
            }

            return target;
        }
    }
}
