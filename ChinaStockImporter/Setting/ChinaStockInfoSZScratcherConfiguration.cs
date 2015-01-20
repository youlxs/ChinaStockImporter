using System;
using Yootech.ChinaStockImporter.Log;

namespace Yootech.ChinaStockImporter.Setting
{
    public class ChinaStockInfoSZScratcherConfiguration : IScratcherConfig
    {
        private const string StoreFolder = "ChinaStockInfoSZ";
        private ILogger logger;

        public ChinaStockInfoSZScratcherConfiguration()
        {
            this.logger = LogFactory.GetLog4NetLogger();
        }

        public int GetScratchIntervalSecond()
        {
            const int DefaultScratchIntervalSecond = 300;
            int result = 0;

            try
            {
                var configValue = System.Configuration.ConfigurationManager.AppSettings["ScratchIntervalSecond"];
                var parseOk = int.TryParse(configValue, out result);
                if (!parseOk)
                {
                    result = DefaultScratchIntervalSecond;
                    throw new FormatException(string.Format("AppSetting value \"{0}\" not a useful value.set as default (300 second)", configValue));
                }
            }
            catch (System.Configuration.ConfigurationErrorsException e)
            {
                this.logger.Error("GetScratchIntervalSecond", e.Message, e);
            }
            catch (FormatException e)
            {
                this.logger.Error("GetScratchIntervalSecond", e.Message, e);
            }

            return result;
        }

        public string GetScratchPage()
        {
            try
            {
                var configValue = System.Configuration.ConfigurationManager.AppSettings["ChinaStockInfoSZScratchPage"];
                return configValue;
            }
            catch (Exception e)
            {
                this.logger.Error("GetScratchPage", e.Message, e);
            }

            return string.Empty;
        }

        public string GetScratchDataPage()
        {
            try
            {
                var configValue = System.Configuration.ConfigurationManager.AppSettings["ChinaStockInfoSZScratchDataPage"];
                return configValue;
            }
            catch (Exception e)
            {
                this.logger.Error("GetScratchDataPage", e.Message, e);
            }

            return string.Empty;
        }


        public string GetDataStorePath()
        {
            try
            {
                var configValue = string.Format("{0}\\{1}\\{2}", AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["DataStorePath"], StoreFolder);
                return configValue;
            }
            catch (Exception e)
            {
                this.logger.Error("GetDataStorePath", e.Message, e);
            }

            return string.Format("{0}\\", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
