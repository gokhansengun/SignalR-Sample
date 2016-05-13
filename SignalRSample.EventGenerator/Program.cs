using Microsoft.AspNet.SignalR.Client;
using SignalRSample.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRSample.EventGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = ConfigurationManager.AppSettings["HubUrl"].ToString();
            string hubId = ConfigurationManager.AppSettings["HubId"].ToString();

            HubConnection hubConnection = new HubConnection(url);
            IHubProxy hubProxy = hubConnection.CreateHubProxy(hubId);

            hubProxy.On<RegistrationParameters>("registerToEvents",
                async (registrationParameters) =>
                    {
                        for (int i = 0; i < registrationParameters.NoOfEventsToListenTo; ++i)
                        {
                            string eventText = string.Format("Event {0} for channel: {1}", 
                                i + 1, registrationParameters.ChannelId);

                            Console.WriteLine("Generating eventText: {0} for room: {1}", eventText, registrationParameters.RoomId);

                            await hubProxy.Invoke("broadcastEvent",
                                registrationParameters.RoomId, registrationParameters.ChannelId, eventText).ContinueWith(task =>
                                {
                                    if (task.IsFaulted)
                                    {
                                        Console.WriteLine("There was an error opening the connection: {0}", task.Exception.GetBaseException());
                                    }
                                });
                            
                            await Task.Delay(registrationParameters.SleepBetweenEvents);
                        }

                        await hubProxy.Invoke("broadcastEventsComplete", registrationParameters.RoomId, registrationParameters.ChannelId);
                    });

            hubConnection.Start().Wait();

            Console.ReadLine();
        }
    }
}
