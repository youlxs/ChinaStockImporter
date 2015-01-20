using System;
using System.Data;
using System.Linq;
using Yootech.ChinaStockImporter.Configuration;
using Yootech.ChinaStockImporter.Context;
using Yootech.ChinaStockImporter.Entity;

namespace Yootech.ChinaStockImporter.Repositories
{
    public class StockInfoRepository : RepositoryBase<StockInfo>
    {
        public static readonly DataTable MetaDataTable;

        static StockInfoRepository()
        {
            MetaDataTable = new DataTable { TableName = ConfigurationData.ChinaStockTableName };
            MetaDataTable.Columns.Add("Symbol", typeof(string));
             MetaDataTable.Columns.Add("StockName", typeof(string));
             MetaDataTable.Columns.Add("Open", typeof(decimal));
             MetaDataTable.Columns.Add("Low", typeof(decimal));
             MetaDataTable.Columns.Add("High", typeof(decimal));
             MetaDataTable.Columns.Add("Close", typeof(decimal));
             MetaDataTable.Columns.Add("AdjClose", typeof(decimal));
             MetaDataTable.Columns.Add("Volume", typeof(decimal));
             MetaDataTable.Columns.Add("Date", typeof(DateTime));             
        }

        public StockInfoRepository() : base(new ChinaStockContext()) { }

        public override void BulkInsert(System.Collections.Generic.IEnumerable<StockInfo> entities)
        {
            if(!entities.Any()) return;

            var table = MetaDataTable.Copy();

            foreach (var stockInfo in entities)
            {
                var row = table.NewRow();
                row["Symbol"] = stockInfo.Symbol;
                row["StockName"] = stockInfo.StockName;
                row["Open"] = stockInfo.Open;
                row["Low"] = stockInfo.Low;
                row["High"] = stockInfo.High;
                row["Close"] = stockInfo.Close;
                row["AdjClose"] = stockInfo.AdjClose;
                row["Volume"] = stockInfo.Volume;
                row["Date"] = stockInfo.Date;
                table.Rows.Add(row);
            }

            this.BulkInsert(table);
        }
    }
}
