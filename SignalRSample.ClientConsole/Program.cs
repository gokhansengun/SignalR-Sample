using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using Serilog;

namespace SignalRSample.ClientConsole
{
    static class Program
    {
        static void Main()
        {
            var url = ConfigurationManager.AppSettings["HubUrl"];
            var hubId = ConfigurationManager.AppSettings["HubId"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            const int noOfClients = 1;
            var listOfTasks = new List<Task>();

            for (var i = 0; i < noOfClients; ++i)
            {
                var clientId = i.ToString("0000");

                var tsk = new Task(() =>
                {
                    var srClient = new SignalRClient
                    {
                        Id = clientId,
                        ServerUrl = url
                    };

                    srClient.Setup(hubId, clientId).Wait();

                    srClient.TearDown().Wait();

                }, TaskCreationOptions.LongRunning);

                listOfTasks.Add(tsk);
            }

            listOfTasks.ForEach(t => t.Start());

            Task.WaitAll(listOfTasks.ToArray());
        }
    }
}
