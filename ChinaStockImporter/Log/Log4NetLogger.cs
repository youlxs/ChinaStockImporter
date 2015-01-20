using System;
using System.Diagnostics;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace Yootech.ChinaStockImporter.Log
{
    public class Log4NetLogger : ILogger
    {
        private readonly string loggerName;
        private readonly ILog log;

        public Log4NetLogger(ILog log)
        {
            this.log = log;
        }

        public Log4NetLogger(ILog log, string loggerName)
            : this(log)
        {
            this.loggerName = loggerName;
        }

        public void Debug(string method, string message)
        {
            if (this.IsDebugEnabled)
            {
                this.WriteLog(Level.Debug, method, message, null);
            }
        }

        public void Info(string method, string message)
        {
            if (this.IsInfoEnabled)
            {
                this.WriteLog(Level.Info, method, message, null);
            }
        }

        public void Warn(string method, string message)
        {
            if (this.IsWarnEnabled)
            {
                this.WriteLog(Level.Warn, method, message, null);
            }
        }

        public void Error(string method, string message, Exception exception)
        {
            if (this.IsErrorEnabled)
            {
                this.WriteLog(Level.Error, method, message, exception);
            }
        }

        public bool IsDebugEnabled
        {
            get
            {
                return this.log.IsDebugEnabled;
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return this.log.IsInfoEnabled;
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return this.log.IsWarnEnabled;
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return this.log.IsErrorEnabled;
            }
        }

        private void WriteLog(Level level, string method, string message, Exception exception)
        {
            try
            {
                var loggingEvent = new LoggingEvent(typeof(Log4NetLogger), this.log.Logger.Repository, this.log.Logger.Name, level, message, exception);

                loggingEvent.Properties["Method"] = method;

                Task.Factory.StartNew(() => this.log.Logger.Log(loggingEvent));

                Console.WriteLine(string.Format("{0} -- {1}", method, message));
            }
            catch (Exception)
            {
                EventLog.WriteEntry("Yootech.ChinaStockImporter.Logging", exception.Message);
            }
        }
    }
}
