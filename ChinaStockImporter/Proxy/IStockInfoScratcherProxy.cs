using Yootech.ChinaStockImporter.Entity;
using Yootech.ChinaStockImporter.Setting;

namespace Yootech.ChinaStockImporter.Proxy
{
    public interface IFixedOddsMatchResultScratcherProxy<TScratcher, TScratherConfig, TServiceProxy> : IProxy
        where TScratcher : IStockInfoScratcher
        where TScratherConfig : IScratcherConfig
        where TServiceProxy : IServiceProxy<StockInfo>
    {
    }
}
