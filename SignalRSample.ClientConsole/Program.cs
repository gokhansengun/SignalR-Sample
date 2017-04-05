using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;

namespace SignalRSample.ClientConsole
{
    static class Program
    {
        static void Main()
        {
            string url = ConfigurationManager.AppSettings["HubUrl"];
            string hubId = ConfigurationManager.AppSettings["HubId"];

            const int noOfClients = 1;
            List<Task> listOfTasks = new List<Task>();

            for (int i = 0; i < noOfClients; ++i)
            {
                int clientId = i;

                var tsk = new Task(() =>
                {
                    var srClient = new SignalRClient
                    {
                        Description = $"SignalR client with id: {clientId}",
                        Id = clientId,
                        RegisteredHubId = hubId,
                        RegisteredRoomId = clientId,
                        ServerUrl = url
                    };

                    srClient.Setup();

                    new ManualResetEvent(false).WaitOne();
                    
                    srClient.TearDown();

                }, TaskCreationOptions.LongRunning);

                listOfTasks.Add(tsk);
            }

            listOfTasks.ForEach(t => t.Start());

            Task.WaitAll(listOfTasks.ToArray());
        }
    }
}
