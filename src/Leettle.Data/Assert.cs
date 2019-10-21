using System;

namespace Leettle.Data
{
    class Assert
    {
        public static void NotNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void IsAssignable(Type superType, Type subType, string message)
        {
            if (!LeettleDbUtil.IsSubclassOf(subType, superType))
            {
                throw new ArgumentException(message);
            }
        }
    }
}
