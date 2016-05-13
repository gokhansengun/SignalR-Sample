using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using SignalRSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRSample.ClientConsole
{
    public class SignalRClient : IDisposable
    {
        public int Id { get; set; }

        public string ServerUrl { get; set; }

        public string Description { get; set; }

        public string RegisteredHubId { get; set; }

        public int RegisteredRoomId { get; set; }

        private HubConnection _hubConnection = null;

        private IHubProxy _hubProxy = null;

        public IHubProxy Setup()
        {
            Console.WriteLine("Starting SignalR Client at Url: {0}, Hub: {1}, Room: {2}",
                ServerUrl, RegisteredHubId, RegisteredRoomId);

            return SetupHubProxy(ServerUrl, RegisteredHubId);
        }

        public void TearDown()
        {
            Dispose();
        }

        public async Task EnterLoop(IHubProxy hubProxy)
        {
            const int MIN_SLEEP_IN_MS_BETWEEN_TESTS = 4000;
            const int MAX_SLEEP_IN_MS_BETWEEN_TESTS = 5000;

            while (true)
            {
                RegistrationParameters registrationParameters = new RegistrationParameters
                {
                    // for testing purposes randomly select the number of events to be listened to
                    NoOfEventsToListenTo = new Random().Next(2, 10),
                    SleepBetweenEvents = 100,
                    RoomId = this.Id.ToString(),
                    ChannelId = this.Id,
                    Description = "Dummy Description"
                };

                Console.WriteLine("Client: {0}, MsgTestModel: {1} sent RegisterToEvents request to the server",
                    this.Id, JsonConvert.SerializeObject(registrationParameters));

                // Register to the events related with the RoomId provided
                await hubProxy.Invoke("RegisterToEvents", registrationParameters).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error opening the connection: {0}", task.Exception.GetBaseException());
                    }
                });

                EventWaitHandle waitCompleteEvent = new System.Threading.AutoResetEvent(false);

                Action<string, string> eventsCompleteAction = (roomId, targetId) =>
                {
                    Console.WriteLine("Received events complete action with {0} and {1}", roomId, targetId);

                    waitCompleteEvent.Set();
                };

                // wait until all of the events from the EventGenerator have finished.
                using (_hubProxy.On<string, string>("broadcastEventsComplete", eventsCompleteAction))
                {
                    // wait for 15 seconds for all the events to complete
                    if (waitCompleteEvent.WaitOne(new TimeSpan(0, 0, 15)))
                    {
                        Console.WriteLine("Client: {0}, MsgTestModel: {1} received BroadcastEventsComplete request from the server",
                            this.Id, JsonConvert.SerializeObject(registrationParameters));
                    }
                    else
                    {
                        Console.WriteLine("Time out while waiting for BroadcastEventsComplete event");
                    }
                }

                // Deregister from the events related with the RoomId provided
                await hubProxy.Invoke("DeregisterFromEvents", registrationParameters).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Console.WriteLine("There was an error opening the connection: {0}", task.Exception.GetBaseException());
                    }
                });

                await Task.Delay(new Random().Next(MIN_SLEEP_IN_MS_BETWEEN_TESTS, MAX_SLEEP_IN_MS_BETWEEN_TESTS));
            }
        }

        public void Dispose()
        {
            if (_hubConnection != null)
            {
                _hubConnection.Stop();
            }
        }

        private IHubProxy SetupHubProxy(string url, string hubId)
        {
            _hubConnection = new HubConnection(url);

            _hubProxy = _hubConnection.CreateHubProxy(hubId);

            _hubProxy.On<string>("msgClientHello",
                (message) =>
                    Console.WriteLine("Recieved msgClientHello: " + message));

            _hubProxy.On<string, string>("broadcastEvent",
                (channelId, eventText) =>
                {
                    Console.WriteLine("Received broadcastEvent: {0} for channel: {1}", eventText, channelId);
                });


            // TODO: gseng - get rid of Wait and let it be async
            _hubConnection.Start().Wait();

            return _hubProxy;
        }
    }
}
