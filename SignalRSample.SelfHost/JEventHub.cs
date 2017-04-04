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
        public void BroadcastEvent(string roomId, string eventBody)
        {
            Console.WriteLine("Hub received BroadcastEvent msg with roomId: {0} and eventBody: {1}", roomId, eventBody);

            Clients.Group(roomId).broadcastEvent(roomId, eventBody);
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
