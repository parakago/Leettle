using System;
using System.Data.Common;
using System.IO;

namespace Leettle.Data.Impl
{
    class Command : ICommand
    {
        DbCommandWrapper dbCommand;
        public Command(DbCommandWrapper dbCommand)
        {
            this.dbCommand = dbCommand;
        }

        public int Execute()
        {
            return dbCommand.ExecuteNonQuery();
        }

        public ICommand SetParam(string paramName, object paramValue)
        {
            dbCommand.AddParam(paramName, paramValue);
            return this;
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
