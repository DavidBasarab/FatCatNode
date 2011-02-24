using System.Net;

namespace FatCatNode.Logic
{
    internal class OnConnectionMessageWriter
    {
        public OnConnectionMessageWriter(Node node, NodeConnectionStatus connectionStatus, IPAddress address)
        {
            Node = node;
            ConnectionStatus = connectionStatus;
            IpAddress = address;

            WriteConnectionMessage();
        }

        private NodeConnectionStatus ConnectionStatus { get; set; }
        private IPAddress IpAddress { get; set; }

        private Node Node { get; set; }

        private void WriteConnectionMessage()
        {
            if (SuccessfullyConnected())
            {
                WriteSuccessfullyConnectionMessage();
            }
            else if (DidNotSuccessfullyConnect())
            {
                WriteCouldNotConnectMessage();
            }
            else if (IsNodeAlreadyConnected())
            {
                WriteAlreadyConnectedMessage();
            }
        }

        private bool SuccessfullyConnected()
        {
            return ConnectionStatus == NodeConnectionStatus.Added;
        }

        private bool DidNotSuccessfullyConnect()
        {
            return ConnectionStatus == NodeConnectionStatus.CouldNotConnect;
        }

        private bool IsNodeAlreadyConnected()
        {
            return ConnectionStatus == NodeConnectionStatus.AlreadyConnected;
        }

        private void WriteSuccessfullyConnectionMessage()
        {
            WriteMessage("A node with Id '{0}' connected from address {1}",
                         Node.Connections.FindNodeIdByAddress(IpAddress), IpAddress);
        }

        private void WriteMessage(string message, params object[] args)
        {
            if (Node.MessageWriter != null)
            {
                Node.MessageWriter.Message(message, args);
            }
        }

        private void WriteCouldNotConnectMessage()
        {
            WriteMessage("A node from address {0} could not be connected.", IpAddress);
        }

        private void WriteAlreadyConnectedMessage()
        {
            string nodeId = Node.Connections.FindNodeIdByAddress(IpAddress);

            WriteMessage("A node from address {0} is already connected with an Id of {1}.", IpAddress, nodeId);
        }
    }
}