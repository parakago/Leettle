using System;
using System.Data.Common;
using System.IO;

namespace Leettle.Data.Impl
{
    class Command : ICommand, IDisposable
    {
        private DbCommand dbCmd;
        public Command(DbCommand dbCmd)
        {
            this.dbCmd = dbCmd;
        }

        public void Dispose()
        {
            dbCmd.Dispose();
        }

        public int Execute()
        {
            try
            {
                int affected = dbCmd.ExecuteNonQuery();

                dbCmd.Parameters.Clear();

                return affected;
            }
            catch (Exception e)
            {
                throw SqlException.Wrap(e, dbCmd);
            }
        }

        public ICommand SetParam(string paramName, object paramValue)
        {
            var dbParam = dbCmd.CreateParameter();
            dbParam.ParameterName = paramName;
            dbParam.Value = paramValue == null ? DBNull.Value : paramValue;
            dbCmd.Parameters.Add(dbParam);
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
