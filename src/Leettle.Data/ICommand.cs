using System;
using System.IO;

namespace Leettle.Data
{
    public interface ICommand
    {
        ICommand SetParam(string paramName, object paramValue);
        ICommand SetParam(String paramName, Stream paramValue);
        ICommand BindParam(object paramObject);
        int Execute();
    }
}
