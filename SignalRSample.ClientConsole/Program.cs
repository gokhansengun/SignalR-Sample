using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using CommandLine;
using Serilog;

namespace SignalRSample.ClientConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["HubUrl"];
            var hubId = ConfigurationManager.AppSettings["HubId"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                options.GetUsage();
                return;
            }

            var clientId = options.ExtensionId;
            
            var srClient = new SignalRClient
            {
                Id = clientId,
                ServerUrl = url
            };

            var hubProxyTask = srClient.Setup(hubId, clientId);

            hubProxyTask.Wait();

            if (hubProxyTask.IsFaulted)
            {
                Log.Error($"Error in connecting to the hub {hubProxyTask.Exception?.Message}");
                Environment.Exit(1);
            }

            var hubProxy = hubProxyTask.Result;

            hubProxy.Invoke("joinGroup", "0001");

            new ManualResetEvent(false).WaitOne();

            srClient.TearDown().Wait();
        }
    }
}
