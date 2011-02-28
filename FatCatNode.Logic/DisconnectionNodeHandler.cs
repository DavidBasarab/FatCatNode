using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class DisconnectionNodeHandler
    {
        private INodeConnections Connections { get; set; }

        private IMessageWriter MessageWriter { get; set; }

        public DisconnectionNodeHandler(INodeConnections nodeConnections, IMessageWriter messageWriter)
        {
            Connections = nodeConnections;
            MessageWriter = messageWriter;
        }

        public void DisconnectNode(object sender, NodeAnnoucementEventArgs e)
        {
            string disconnectingNodeId = Connections.FindNodeIdByAddress(e.Address);

            Connections.RemoveNodeFromConnections(e.Address);

            if (MessageWriter != null)
            {
                MessageWriter.Message("Node Id {0} at address {1} has disconnected.", disconnectingNodeId, e.Address);
            }
        }
    }
}
