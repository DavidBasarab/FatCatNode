using System.Net;
using FatCatNode.Logic.Arguments;

namespace FatCatNode.Logic.Handlers
{
    internal class OnNodeConnectionHandler
    {
        public void ConnectNode(object sender, NodeAnnoucementEventArgs eventArgs)
        {
            AddToConnections(eventArgs.Address);
        }

        private void AddToConnections(IPAddress address)
        {
            NodeConnectionStatus connectionStatus = NodeConnections.Connections.AddNodeToConnections(address);

            WriteOnConnectionMessage(address, connectionStatus);
        }

        private void WriteOnConnectionMessage(IPAddress address, NodeConnectionStatus connectionStatus)
        {
            var onConnectionMessageWriter = new OnConnectionMessageWriter(connectionStatus, address);

            onConnectionMessageWriter.WriteConnectionMessage();
        }
    }
}