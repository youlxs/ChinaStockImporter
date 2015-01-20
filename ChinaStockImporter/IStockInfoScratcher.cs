using System.Collections.Generic;
using Yootech.ChinaStockImporter.Entity;

namespace Yootech.ChinaStockImporter
{
    public interface IStockInfoScratcher
    {
        IEnumerable<StockInfo> Scratch(string url, string dataUrl);
    }
}
