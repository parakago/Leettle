using System;
using System.Linq;

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
            return snakeCaseVar.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}
