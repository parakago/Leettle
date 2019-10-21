using System;
using System.IO;

namespace Leettle.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        ICommand SetParam(string paramName, object paramValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        ICommand SetParam(String paramName, Stream paramValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramObject"></param>
        /// <returns></returns>
        ICommand BindParam(object paramObject);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int Execute();
    }
}
