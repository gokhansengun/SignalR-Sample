using Microsoft.AspNet.SignalR.Client;
using SignalRSample.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CommandLine;
using CommandLine.Text;

namespace SignalRSample.EventEmitter
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = ConfigurationManager.AppSettings["HubUrl"].ToString();
            string hubId = ConfigurationManager.AppSettings["HubId"].ToString();

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

                Task eventTask = null;
                
                eventTask = hubProxy.Invoke("broadcastEvent", options.GroupId, GenerateCallArrivedEvent());

                Task.WaitAll(eventTask);

                if (eventTask.IsFaulted)
                {
                    Console.WriteLine("An error occurred and event could not be sent!");
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
