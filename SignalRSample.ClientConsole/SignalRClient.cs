using Microsoft.AspNet.SignalR.Client;
using System;

namespace SignalRSample.ClientConsole
{
    public class SignalRClient : IDisposable
    {
        public int Id { get; set; }

        public string ServerUrl { get; set; }

        public string Description { get; set; }

        public string RegisteredHubId { get; set; }

        public int RegisteredRoomId { get; set; }

        private HubConnection _hubConnection;

        private IHubProxy _hubProxy;

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

            _hubProxy.On<string>("broadcastEvent",
                (eventBody) =>
                {
                    Console.WriteLine("Received broadcastEvent: {0}", eventBody);
                });


            // TODO: gseng - get rid of Wait and let it be async
            _hubConnection.Start().Wait();

            return _hubProxy;
        }
    }
}
