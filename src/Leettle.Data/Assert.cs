using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void isAssignable(Type superType, Type subType, string message)
        {
            if (!subType.IsSubclassOf(superType))
            {
                throw new ArgumentException(message);
            }
        }
    }
}
