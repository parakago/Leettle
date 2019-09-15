using System;
using System.Data.Common;
using System.IO;

namespace Leettle.Data.Impl
{
    class Command : AbstractQuery, ICommand, IDisposable
    {
        public Command(DbCommand dbCommand) : base(dbCommand)
        {
            
        }

        public int Execute()
        {
            return ExecuteNonQuery();
        }

        public ICommand SetParam(string paramName, object paramValue)
        {
            return (ICommand)AddParam(paramName, paramValue);
        }

        public ICommand SetParam(string paramName, Stream paramValue)
        {
            byte[] blob = null;
            if (paramValue != null)
            {
                if (paramValue is MemoryStream)
                {
                    blob = ((MemoryStream)paramValue).ToArray();
                }
                else
                {
                    paramValue.Position = 0;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] buffer = new byte[8192];
                        for (int count = 0; (count = paramValue.Read(buffer, 0, buffer.Length)) > 0;)
                        {
                            ms.Write(buffer, 0, count);
                        }
                        blob = ms.ToArray();
                    }
                }
            }

            return SetParam(paramName, blob);
        }
    }
}
