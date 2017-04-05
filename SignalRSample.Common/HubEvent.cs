using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRSample.Common
{
    public enum HubEvent
    {
        HubEventUnknown = 0,
        HubEventJoinGroup,
        HubEventLeaveGroup,
        HubEventCallArrived
    }
}
