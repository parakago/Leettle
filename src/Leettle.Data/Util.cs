using System;

namespace Leettle.Data
{
    class Util
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
    }
}
