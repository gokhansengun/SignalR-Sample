using System;
using Microsoft.Owin.Hosting;
using System.Threading;
using System.Configuration;

namespace SignalRSample.SelfHost
{
    static class Program
    {
        static void Main()
        {
            var url = ConfigurationManager.AppSettings["HubUrl"];

            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                
                // wait until process receives a signal
                new ManualResetEvent(false).WaitOne();
            }
        }
    }
}
