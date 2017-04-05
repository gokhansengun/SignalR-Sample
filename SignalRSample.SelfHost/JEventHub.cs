using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Serilog;

namespace SignalRSample.SelfHost
{
    public class JEventHub : Hub
    {
        public void JoinGroup(string groupName)
        {
            Log.Debug($"JoinGroup - {Context.ConnectionId}");

            Groups.Add(Context.ConnectionId, groupName);
        }

        public void LeaveGroup(string groupName)
        {
            Log.Debug($"LeaveGroup - {Context.ConnectionId}");

            Groups.Remove(Context.ConnectionId, groupName);
        }

        public void BroadcastEvent(string groupName, string eventBody)
        {
            Log.Debug($"BroadcastEvent - {Context.ConnectionId} - groupId: {groupName}, eventBody: {eventBody}");

            Clients.Group(groupName).broadcastEvent(groupName, eventBody);
        }
        
        public override Task OnConnected()
        {
            Log.Debug($"OnConnected - {Context.ConnectionId}");

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Log.Debug($"OnDisconnected - {Context.ConnectionId}");

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Log.Debug($"OnReconnected - {Context.ConnectionId}");

            return base.OnReconnected();
        }
    }
}
