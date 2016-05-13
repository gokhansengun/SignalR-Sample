using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRSample.Common
{
    public class RegistrationParameters
    {
        public int ChannelId { get; set; }

        public string RoomId { get; set; }

        public int NoOfEventsToListenTo { get; set; }

        public int SleepBetweenEvents { get; set; }

        public string Description { get; set; }
    }
}
