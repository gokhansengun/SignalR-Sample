using Microsoft.AspNet.SignalR.Client;
using System;
using System.Configuration;
using System.Threading.Tasks;
using CommandLine;
using Serilog;
using SignalRSample.Common;

namespace SignalRSample.EventEmitter
{
    static class Program
    {
        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["HubUrl"];
            var hubId = ConfigurationManager.AppSettings["HubId"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
            {
                options.GetUsage();
                return;
            }

            using (var hubConnection = new HubConnection(url))
            {
                var hubProxy = hubConnection.CreateHubProxy(hubId);

                hubConnection.Start().Wait();

                var hubEvent = HubEvent.HubEventUnknown;

                if (!Enum.TryParse(options.EventType, out hubEvent))
                {
                    Log.Error($"An unknown event {options.EventType} passed!");
                    
                    Environment.Exit(1);
                }

                // TODO: gseng - switch the mock event according to the event passed from the command line
                var eventTask = hubProxy.Invoke("broadcastEvent", options.GroupId, GenerateMsgBodyFromEvent(hubEvent));

                Task.WaitAll(eventTask);

                if (eventTask.IsFaulted)
                {
                    Log.Error("An error occurred and event could not be sent!");
                    Environment.Exit(1);
                }

                Log.Information("Event sent and ACKed successfully.");
            }
        }

        private static string GenerateMsgBodyFromEvent(HubEvent hubEvent)
        {
            switch (hubEvent)
            {
                case HubEvent.HubEventJoinGroup:
                case HubEvent.HubEventLeaveGroup:
                    return string.Empty;

                case HubEvent.HubEventCallArrived:
                    return GenerateCallArrivedEvent();

                default:
                    throw new Exception($"Unknown event {hubEvent}");
            }
        }

        private static string GenerateCallArrivedEvent()
        {
            return "CallArrivedEventBody";
        }
    }
}
