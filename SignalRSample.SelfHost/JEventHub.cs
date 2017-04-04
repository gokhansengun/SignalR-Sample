using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRSample.Common;

namespace SignalRSample.SelfHost
{
    public class JEventHub : Hub
    {
        public void RegisterToEvents(RegistrationParameters registrationParameters)
        {
            Console.WriteLine("Hub received register to events msg with content: {0}",
                JsonConvert.SerializeObject(registrationParameters));

            Groups.Add(Context.ConnectionId, registrationParameters.RoomId);

            Clients.All.registerToEvents(registrationParameters);
        }

        public void DeregisterFromEvents(RegistrationParameters registrationParameters)
        {
            Console.WriteLine("Hub received de-register to events msg with content: {0}",
                JsonConvert.SerializeObject(registrationParameters));

            Groups.Remove(Context.ConnectionId, registrationParameters.RoomId);

            Clients.All.deregisterFromEvents(registrationParameters);
        }

        public void BroadcastEvent(string roomId, string eventBody)
        {
            Console.WriteLine("Hub received BroadcastEvent msg with roomId: {0} and eventBody: {1}", roomId, eventBody);

            Clients.Group(roomId).broadcastEvent(roomId, eventBody);
        }

        public void BroadcastEventsComplete(string roomId, string channelId)
        {
            Console.WriteLine("Hub received broadcast events complete event msg with roomId: {0}", roomId);

            Clients.Group(roomId).broadcastEventsComplete(roomId, channelId);
        }

        public void MsgClientHello(string roomId, string welcomeMsg)
        {
            Console.WriteLine("Hub received test hello: {0}", welcomeMsg);
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Hub OnConnected {0}", Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("Hub OnDisconnected {0}", Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("Hub OnReconnected {0}", Context.ConnectionId);
            return base.OnReconnected();
        }
    }
}
