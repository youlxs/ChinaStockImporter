using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yootech.ChinaStockImporter.Entity;
using Yootech.ChinaStockImporter.Repositories;

namespace Yootech.ChinaStockImporter
{
    public class StockInfoScratcher : StockInfoScratcherAbstract
    {
        private readonly StockInfoRepository stockInfoRepository = new StockInfoRepository();

        private static readonly DateTime StartDateTime = new DateTime(2013, 1, 1, 0, 0, 0);

        public override IEnumerable<StockInfo> Scratch(string url, string dataUrl)
        { 
            var result = new List<StockInfo>();           
            var pageCount = this.GetPageCount();

            for (int i = 0; i < pageCount; i++)
            {  
                var sourcePageUrl = string.Format(url, i + 1);

                try
                {
                    this.Logger.Info("Scratch", string.Format("Start to scratch url {0}", sourcePageUrl));

                    var bTarget = this.WebClient.DownloadData(sourcePageUrl);

                    this.Logger.Info("Scratch", string.Format("Finish to scratch url {0}", sourcePageUrl));

                    var target = Encoding.UTF8.GetString(bTarget);

                    var document = new HtmlDocument();

                    document.LoadHtml(target);

                    var matchResultTable =
                        document.DocumentNode.Descendants("div")
                                .Where(
                                    d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("tab01"));

                    var trNodes = matchResultTable.Single().SelectNodes("//tr");

                    foreach (var node in trNodes.Skip(1))
                    {
                        try
                        {
                            var symbol = node.ChildNodes[1].InnerText;

                            var maxImportedDate = stockInfoRepository.Get(s => s.Symbol == symbol).Any()
                                                      ? new DateTime?(
                                                            stockInfoRepository.Get(s => s.Symbol == symbol)
                                                                               .Max(s => s.Date))
                                                      : null;

                            this.Logger.Info("Scratch", string.Format("The max imported data for symbol {0} is {1}", symbol, maxImportedDate.HasValue ? maxImportedDate.ToString() : "Empty"));

                            var dataPageUrl = string.Format(dataUrl, symbol);

                            this.Logger.Info("Scratch", string.Format("Start to download data url {0}", dataPageUrl));

                            var data = this.WebClient.DownloadData(dataPageUrl);

                            this.Logger.Info("Scratch", string.Format("Finish to download data url {0}", dataPageUrl));

                            var content = Encoding.UTF8.GetString(data);

                            var hisData = content.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

                            var hisStockInfo = new List<StockInfo>(hisData.Length - 1);

                            this.Logger.Info("Scratch", string.Format("Total {0} data was downloaded.", hisData.Length - 1));

                            foreach (var hd in hisData.Skip(1))
                            {
                                var items = hd.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

                                var date = DateTime.Parse(items[0]);

                                if(date < StartDateTime)
                                    continue;

                                if (maxImportedDate.HasValue && date <= maxImportedDate)
                                    continue;

                                hisStockInfo.Add(new StockInfo
                                    {
                                        Date = DateTime.Parse(items[0]),
                                        AdjClose = decimal.Parse(items[6]),
                                        Close = decimal.Parse(items[4]),
                                        High = decimal.Parse(items[2]),
                                        Low = decimal.Parse(items[3]),
                                        Open = decimal.Parse(items[1]),
                                        StockName = node.ChildNodes[3].InnerText,
                                        Symbol = symbol,
                                        Volume = long.Parse(items[5])
                                    });
                            }

                            this.Logger.Info("Scratch", string.Format("Total {0} data was inserted into database.", hisStockInfo.Count));

                            stockInfoRepository.BulkInsert(hisStockInfo);
                        }
                        catch (Exception e)
                        {
                            this.Logger.Error("StockInfoScratcher.Scratch.ReadPageData",
                                              string.Format("Error Happened while fetch stock {0} {1}",
                                                            node.ChildNodes[1].InnerText, node.ChildNodes[3].InnerText),
                                              e);
                        }
                    }
                }
                catch (Exception exp)
                {
                    this.Logger.Error("StockInfoScratcher.Scratch", string.Format("Error Happened while fetch page {0}", sourcePageUrl), exp);
                }
            }

            return result;
        }

        private int GetPageCount()
        {
            return Configuration.ConfigurationData.TotalNumOfPage;
        }
    }
}
