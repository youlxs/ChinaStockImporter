using System;

namespace Yootech.ChinaStockImporter.Entity
{
    public class StockInfo
    {
        public string Symbol { get; set; }

        public string StockName { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal AdjClose { get; set; }

        public long Volume { get; set; }

        public DateTime Date { get; set; }
    }
}
