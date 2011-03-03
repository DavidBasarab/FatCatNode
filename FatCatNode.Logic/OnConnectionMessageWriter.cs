using System.Net;

namespace FatCatNode.Logic
{
    internal class OnConnectionMessageWriter
    {
        public OnConnectionMessageWriter(NodeConnectionStatus connectionStatus, IPAddress address)
        {
            ConnectionStatus = connectionStatus;
            IpAddress = address;
        }

        private NodeConnectionStatus ConnectionStatus { get; set; }
        private IPAddress IpAddress { get; set; }

        public void WriteConnectionMessage()
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
            else if (ErrorInHandShakeWIthNode())
            {
                WriteHandshakeErrorMessage();
            }
        }

        private void WriteHandshakeErrorMessage()
        {
            WriteMessage("An error in handshake with node at {0}.", IpAddress);
        }

        private bool ErrorInHandShakeWIthNode()
        {
            return ConnectionStatus == NodeConnectionStatus.ErrorInHandShake;
        }

        private bool SuccessfullyConnected()
        {
            return ConnectionStatus == NodeConnectionStatus.Connected;
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
            WriteMessage("A node with Id '{0}' connected from address {1}", NodeConnections.Connections.FindNodeIdByAddress(IpAddress), IpAddress);
        }

        private void WriteMessage(string message, params object[] args)
        {
            MessageWriter.Writer.Message(message, args);
        }

        private void WriteCouldNotConnectMessage()
        {
            WriteMessage("A node from address {0} could not be connected.", IpAddress);
        }

        private void WriteAlreadyConnectedMessage()
        {
            string nodeId = NodeConnections.Connections.FindNodeIdByAddress(IpAddress);

            WriteMessage("A node from address {0} is already connected with an Id of {1}.", IpAddress, nodeId);
        }
    }
}