using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yootech.ChinaStockImporter.Entity;
using Yootech.ChinaStockImporter.Log;
using Yootech.ChinaStockImporter.Repositories;

namespace Yootech.ChinaStockImporter
{
    public class ChinaStockInfoProxy : IServiceProxy<StockInfo>
    {        
        private readonly StockInfoRepository stockInfoRepository;
        private readonly ILogger logger;

        public ChinaStockInfoProxy()
        {
            this.stockInfoRepository = new StockInfoRepository();
            this.logger = LogFactory.GetLog4NetLogger();
        }

        public void DoServiceWork(IEnumerable<StockInfo> infos)
        {
            foreach (var info in infos)
            {
                try
                {
                    var checkResult =
                        this.stockInfoRepository.Any(s => s.Symbol == info.Symbol);

                    if (checkResult)
                    {
                        this.stockInfoRepository.Update(info);
                    }
                    else
                    {
                        this.stockInfoRepository.Add(info);
                    }
                }
                catch (Exception exp)
                {
                    this.logger.Error("DoServiceWork", "Save StockInfo Failed", exp);
                }
            }
        }         
    }
}
