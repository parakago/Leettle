using System;
using System.Linq;
using System.Reflection;

namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class LeettleDbUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposable"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<보류 중>")]
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
                // do nothing
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snakeCaseVar"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="camelCaseVar"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <returns></returns>
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
