using System;
using System.IO;

namespace Leettle.Data
{
    public interface ICommand
    {
        ICommand SetParam(string paramName, object paramValue);
        ICommand SetParam(String paramName, Stream paramValue);
        int Execute();
    }
}
