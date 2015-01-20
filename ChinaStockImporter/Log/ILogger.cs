using System;

namespace Yootech.ChinaStockImporter.Log
{
    public interface ILogger
    {
        void Debug(string method, string message);
        void Info(string method, string message);
        void Warn(string method, string message);
        void Error(string method, string message, Exception exception);
        //void Fatal(string method, string message, Exception exception);

        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
    }
}
