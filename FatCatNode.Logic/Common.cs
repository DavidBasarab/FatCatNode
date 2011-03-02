using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatCatNode.Logic
{
    public enum NodeConnectionStatus
    {
        None,
        Connected,
        AlreadyConnected,
        CouldNotConnect,
        Removed
    }
}
