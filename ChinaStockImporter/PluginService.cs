using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Yootech.ChinaStockImporter.Log;
using Yootech.ChinaStockImporter.Proxy;
using Yootech.ChinaStockImporter.Setting;
using log4net.Core;
using ILogger = Yootech.ChinaStockImporter.Log.ILogger;

namespace Yootech.ChinaStockImporter
{
    /// <summary>
    /// Service for get matchs info from pages
    /// </summary>
    public class PluginService : ServiceBase
    {
        private static readonly ILogger Logger = LogFactory.GetLog4NetLogger();

        private readonly IList<IProxy> scratcherProxies;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginService"/> class.
        /// </summary>
        public PluginService()
        {
            this.CanHandlePowerEvent = false;
            this.CanPauseAndContinue = false;
            this.CanStop = true;
            this.CanShutdown = false;

            this.scratcherProxies = new List<IProxy>
                { 
                    new ChinaStockInfoSSScratcherProxy<StockInfoScratcher, ChinaStockInfoSSScratcherConfiguration, ChinaStockInfoProxy>(),                 
                    ////new ChinaStockInfoSSScratcherProxy<StockInfoScratcher, ChinaStockInfoSZScratcherConfiguration, ChinaStockInfoProxy>(),   
                };
        }

        /// <summary>
        /// This method is called when the service is started.
        /// </summary>
        /// <param name="args">Start arguments. They are not used by default, but can be used in derived classes.</param>
        protected override void OnStart(string[] args)
        {
            foreach (var scratcherProxy in this.scratcherProxies)
            {
                try
                {
                    scratcherProxy.DoScratchWork();
                }
                catch (Exception e)
                {
                    Logger.Error("OnStart", e.Message, e);
                }
            }
        }

        /// <summary>
        /// This method is called when the service is Stopped.
        /// </summary>
        protected override void OnStop()
        {
            foreach (var scratcherProxy in this.scratcherProxies)
            {
                try
                {
                    scratcherProxy.StopScratchWork();
                }
                catch (Exception e)
                {
                    Logger.Error("OnStop", e.Message, e);
                }
            }

            Logger.Info("OnStop", "Scratcher Task stopped.");
        }
    }
}