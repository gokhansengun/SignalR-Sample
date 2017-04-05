using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;
using Serilog;

namespace SignalRSample.ClientConsole
{
    public class SignalRClient : IDisposable
    {
        public string Id { get; set; }

        public string ServerUrl { get; set; }

        private string _hubId;

        private string _groupId;

        private HubConnection _hubConnection;

        private IHubProxy _hubProxy;

        public async Task<IHubProxy> Setup(string hubId, string groupId)
        {
            Log.Debug("Starting SignalR Client at Url: {0}, Hub: {1}, Group: {2}",
                ServerUrl, hubId, groupId);

            _groupId = groupId;
            _hubId = hubId;

            Log.Debug($"Joining the group {groupId} now");

            var hubProxy = await SetupHubProxy(ServerUrl);

            await hubProxy.Invoke("joinGroup", groupId);

            Log.Debug($"Joined the group {groupId}");

            return hubProxy;
        }

        public async Task TearDown()
        {
            Log.Debug($"Leaving the group {_groupId} now");

            await _hubProxy.Invoke("leaveGroup", _groupId);

            Log.Debug($"Left the group {_groupId}");

            Dispose();
        }
        
        private async Task<IHubProxy> SetupHubProxy(string url)
        {
            _hubConnection = new HubConnection(url);

            _hubProxy = _hubConnection.CreateHubProxy(_hubId);
            
            _hubProxy.On<string>("broadcastEvent",
                (eventBody) =>
                {
                    Log.Debug("Received broadcastEvent: {0}", eventBody);
                });

            await _hubConnection.Start();

            return _hubProxy;
        }

        public void Dispose()
        {
            _hubConnection?.Stop();
        }
    }
}
