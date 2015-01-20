﻿using System;
using System.Linq;
using System.Threading;
using Yootech.ChinaStockImporter.Entity;
using Yootech.ChinaStockImporter.Log;
using Yootech.ChinaStockImporter.Setting;

namespace Yootech.ChinaStockImporter.Proxy
{
    public class ChinaStockInfoSZScratcherProxy<TScratcher, TScratherConfig, TServiceProxy> :
        IFixedOddsMatchResultScratcherProxy<TScratcher, TScratherConfig, TServiceProxy>
        where TScratcher : IStockInfoScratcher
        where TScratherConfig : IScratcherConfig
        where TServiceProxy : IServiceProxy<StockInfo>
    {
        public readonly ILogger Logger = LogFactory.GetLog4NetLogger();

        private readonly TScratcher scratcher;

        private readonly TScratherConfig scratherConfig;

        private readonly TServiceProxy serviceProxy;

        private readonly Thread workThread;

        public ChinaStockInfoSZScratcherProxy()
        {
            this.scratherConfig = Activator.CreateInstance<TScratherConfig>();
            this.ScratchIntervalSecond = this.scratherConfig.GetScratchIntervalSecond();
            this.scratcher = Activator.CreateInstance<TScratcher>();
            this.serviceProxy = Activator.CreateInstance<TServiceProxy>();
            this.workThread = new Thread(this.InnerDoScratchWork);
        }

        // Last hash value of ScratchPages.if that not changed,we don't need to change anything again
        public int GameHash { get; private set; }

        public int ScratchIntervalSecond { get; private set; }

        public bool WorkFlag { get; private set; }

        public void DoScratchWork()
        {
            if (this.WorkFlag)
            {
                return;
            }

            this.WorkFlag = true;

            this.workThread.Start();
        }

        public void StopScratchWork()
        {
            try
            {
                if (this.workThread.IsAlive)
                {
                    this.WorkFlag = false;
                    this.workThread.Abort();
                }

                this.Logger.Info("OnStop", "Scratcher Task stopped.");
            }
            catch (Exception e)
            {
                this.Logger.Error("OnStop", e.Message, e);
            }
        }

        protected void InnerDoScratchWork()
        {
            do
            {
                try
                {
                    int hashCode = 0;
                     
                    // scratch data,if the target page not changed,it will return empty list,so that should do nothing
                    var scratcherValue = this.scratcher.Scratch(this.scratherConfig.GetScratchPage(), this.scratherConfig.GetScratchDataPage());

                    if (scratcherValue.Any())
                    {
                        // call wcf service to add or change data
                        this.serviceProxy.DoServiceWork(scratcherValue);

                        // set the flag,after it would not work when target page without change
                        this.GameHash = hashCode;
                    }
                }               
                catch (Exception e)
                {
                    this.Logger.Error("PluginService.DoScratchWork", e.Message, e);
                }

                Thread.Sleep(this.ScratchIntervalSecond * 1000);
            }
            while (this.WorkFlag);
        }
    }
}
