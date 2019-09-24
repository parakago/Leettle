using System;
using System.Linq;
using System.Reflection;

namespace Leettle.Data
{
    public class LeettleDbUtil
    {
        public static void DisposeSilently(IDisposable disposable)
        {
            try
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            catch
            {
                
            }
        }

        public static string SnakeToCamel(string snakeCaseVar)
        {
            if (string.IsNullOrWhiteSpace(snakeCaseVar))
            {
                return snakeCaseVar;
            }
            else
            {
                return snakeCaseVar.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1).ToLower())
                    .Aggregate(string.Empty, (s1, s2) => s1 + s2);
            }
        }

        public static string CamelToSnake(string camelCaseVar)
        {
            if (string.IsNullOrWhiteSpace(camelCaseVar))
            {
                return camelCaseVar;
            }
            else
            {
                return string.Concat(camelCaseVar.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
            }
        }

        public static PropertyInfo FindProperty(Type t, string propertyName)
        {
            foreach (var propertyInfo in t.GetTypeInfo().DeclaredProperties)
            {
                if (propertyInfo.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return propertyInfo;
                }
            }
            return null;
        }

        public static bool IsSubclassOf(Type p, Type c)
        {
            if (p == c)
                return false;
            while (p != null)
            {
                if (p == c)
                    return true;
                p = p.GetTypeInfo().BaseType;
            }
            return false;
        }
    }
}
