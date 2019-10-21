using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Leettle.Data.Impl
{
    class FieldPropMapping<T>
    {
        public int FieldIndex { get; }
        private readonly Action<T, object> setter;
        private readonly PropertyInfo propertyInfo;

        public FieldPropMapping(int fieldIndex, PropertyInfo propertyInfo)
        {
            this.FieldIndex = fieldIndex;
            this.propertyInfo = propertyInfo;

            if (propertyInfo.SetMethod != null)
            {
                var instanceParam = Expression.Parameter(propertyInfo.DeclaringType);
                var argumentParam = Expression.Parameter(typeof(object));
                var methodCallExpression = Expression.Call(instanceParam, propertyInfo.SetMethod, Expression.Convert(argumentParam, propertyInfo.PropertyType));
                this.setter = Expression.Lambda<Action<T, object>>(methodCallExpression, instanceParam, argumentParam).Compile();
            }
        }

        public void SetValue(object target, object value)
        {
            setter?.Invoke((T)target, Convert.ChangeType(value, propertyInfo.PropertyType));
        }
    }
}
