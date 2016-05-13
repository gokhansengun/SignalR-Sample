using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using SignalRSample.Common;
using System.Configuration;

namespace SignalRSample.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = ConfigurationManager.AppSettings["HubUrl"].ToString();
            string hubId = ConfigurationManager.AppSettings["HubId"].ToString();

            const int NO_OF_CLIENTS = 1;
            List<Task> listOfTasks = new List<Task>();

            for (int i = 0; i < NO_OF_CLIENTS; ++i)
            {
                int clientId = i;

                Task tsk = new Task(() =>
                {
                    SignalRClient srClient = new SignalRClient
                    {
                        Description = string.Format("SignalR client with id: {0}", clientId),
                        Id = clientId,
                        RegisteredHubId = hubId,
                        RegisteredRoomId = clientId,
                        ServerUrl = url
                    };

                    IHubProxy hubProxy = srClient.Setup();

                    srClient.EnterLoop(hubProxy).Wait();

                    srClient.TearDown();

                }, TaskCreationOptions.LongRunning);

                listOfTasks.Add(tsk);
            }

            listOfTasks.ForEach(t => t.Start());

            Task.WaitAll(listOfTasks.ToArray());
        }
    }
}
