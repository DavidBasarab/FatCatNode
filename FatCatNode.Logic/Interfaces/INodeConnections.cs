using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FatCatNode.Logic.Interfaces
{
    public interface INodeConnections
    {
        NodeConnectionStatus AddNodeToConnections(IPAddress address);

        string FindNodeIdByAddress(IPAddress address);

        NodeConnectionStatus RemoveNodeFromConnections(IPAddress address);

        void SetNodeId(string nodeId);

        IRemoteNodeConnectionHelper RemoteHelper { get; set; }

        void ResetConnectedNodes();
    }
}
