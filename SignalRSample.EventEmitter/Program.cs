using Microsoft.AspNet.SignalR.Client;
using System;
using System.Configuration;
using System.Threading.Tasks;
using CommandLine;

namespace SignalRSample.EventEmitter
{
    static class Program
    {
        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["HubUrl"];
            var hubId = ConfigurationManager.AppSettings["HubId"];

            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                options.GetUsage();
                return;
            }

            using (HubConnection hubConnection = new HubConnection(url))
            {
                IHubProxy hubProxy = hubConnection.CreateHubProxy(hubId);

                hubConnection.Start().Wait();

                Task eventTask;
                
                // TODO: gseng - switch the mock event according to the event passed from the command line
                eventTask = hubProxy.Invoke("broadcastEvent", options.GroupId, GenerateCallArrivedEvent());

                Task.WaitAll(eventTask);

                if (eventTask.IsFaulted)
                {
                    Console.WriteLine("An error occurred and event could not be sent!");
                    Environment.Exit(1);
                }

                Console.WriteLine("Event sent and ACKed successfully.");
            }
        }

        private static string GenerateCallArrivedEvent()
        {
            return "CallArrivedEvent";
        }
    }
}
