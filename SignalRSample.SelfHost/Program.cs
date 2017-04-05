using Microsoft.Owin.Hosting;
using System.Threading;
using System.Configuration;
using Serilog;

namespace SignalRSample.SelfHost
{
    static class Program
    {
        static void Main()
        {
            var url = ConfigurationManager.AppSettings["HubUrl"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            using (WebApp.Start(url))
            {
                Log.Information("Server running on {0}", url);
                
                // wait until process receives a signal
                new ManualResetEvent(false).WaitOne();
            }
        }
    }
}
