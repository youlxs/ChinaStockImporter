using System;
using System.Configuration;

namespace Yootech.ChinaStockImporter.Configuration
{
    public static class ConfigurationData
    {
        public static readonly string ChinaStockTableName = string.IsNullOrEmpty(ConfigurationManager.AppSettings["ChinaStockTableName"]) ? "Stocks" : ConfigurationManager.AppSettings["ChinaStockTableName"];
        public static readonly int TotalNumOfPage = string.IsNullOrEmpty(ConfigurationManager.AppSettings["TotalNumOfPage"]) ? 30 : Convert.ToInt32(ConfigurationManager.AppSettings["TotalNumOfPage"]);
    }
}
