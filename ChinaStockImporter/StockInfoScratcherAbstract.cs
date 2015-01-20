using System.Collections.Generic;
using System.Net;
using System.Text;
using Yootech.ChinaStockImporter.Entity;
using Yootech.ChinaStockImporter.Log;

namespace Yootech.ChinaStockImporter
{
    public abstract class StockInfoScratcherAbstract : IStockInfoScratcher
    {
        protected readonly ILogger Logger = LogFactory.GetLog4NetLogger();

        private WebClient webClient;

        public WebClient WebClient
        {
            get
            {
                if (this.webClient == null)
                {
                    // NoCacheNoStore Client with Chines GB2312 encoding
                    this.webClient = new WebClient
                    {
                        CachePolicy =
                            new System.Net.Cache.RequestCachePolicy(
                            System.Net.Cache.RequestCacheLevel.NoCacheNoStore),
                        Encoding = Encoding.GetEncoding("GB2312")
                    };
                }

                return this.webClient;
            }

            protected set
            {
                this.webClient = value;
            }
        }

        public abstract IEnumerable<StockInfo> Scratch(string url, string dataUrl);
    }
}
