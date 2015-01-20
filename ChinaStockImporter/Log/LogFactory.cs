using System;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

namespace Yootech.ChinaStockImporter.Log
{
    public class LogFactory
    {
        private static readonly Lazy<ILogger> logger = new Lazy<ILogger>(
            () =>
            {
                log4net.Config.XmlConfigurator.Configure();

                ILog log = LogManager.GetLogger(loggerName);

                Logger l = (Logger)log.Logger;
                l.Repository.Configured = true;

                l.AddAppender(new FileAppender());
              
                return new Log4NetLogger(log);
            });

        private static string loggerName;

        public static ILogger GetLog4NetLogger()
        {
            return GetLog4NetLogger("DefaultLogFile");
        }

        public static ILogger GetLog4NetLogger(string loggerName)
        {
            LogFactory.loggerName = loggerName;
            return logger.Value;
        }

    }
}
