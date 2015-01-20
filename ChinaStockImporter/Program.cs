using System.ServiceProcess;

namespace Yootech.ChinaStockImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesToRun = new ServiceBase[] { new PluginService() };
            ConsoleServiceHelper.StartService("Yootech.ChinaStockInfo.Scratcher", servicesToRun, args);
        }
    }
}
