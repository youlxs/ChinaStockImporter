using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Threading;

namespace Yootech.ChinaStockImporter
{
    public static class ConsoleServiceHelper
    {
        private static ServiceBase[] services;
        private static string serviceName;
        private static ManualResetEvent stopEvent;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void StartService(string serviceName, ServiceBase[] services, string[] args)
        {
            ConsoleServiceHelper.services = services;
            ConsoleServiceHelper.serviceName = serviceName;
            RunAsConsoleApplication();
        }

        public static void StopService()
        {
            if (stopEvent != null)
            {
                stopEvent.Set();
            }
            else
            {
                foreach (var service in services)
                {
                    service.Stop();
                }
            }
        }

        private static void RunAsConsoleApplication()
        {
            stopEvent = new ManualResetEvent(false);

            var thread = new Thread(
                () =>
                    {
                        ConsoleColor color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Running Service {0} as Console Application", serviceName);

                        Console.ForegroundColor = color;
                        Console.WriteLine("Running... Press any key to stop service");
                        Console.ReadKey();
                        Console.WriteLine("Stopping service!");
                        stopEvent.Set();
                    });
            foreach (var service in services)
            {
                var serviceType = service.GetType();
                var onStart = serviceType.GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
                onStart.Invoke(service, new[] { Environment.GetCommandLineArgs() });
            }

            thread.Start();
            stopEvent.WaitOne();
            thread.Abort();

            foreach (var service in services)
            {
                var serviceType = service.GetType();
                var onStop = serviceType.GetMethod("OnStop", BindingFlags.NonPublic | BindingFlags.Instance);
                onStop.Invoke(service, null);
            }
        }
    }
}