using System.Net;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Helpers;

namespace FatCatNode.Logic.Handlers
{
    internal class OnNodeConnectionHandler
    {
        private int ConnectionAttempts { get; set; }

        public void ConnectNode(object sender, NodeAnnoucementEventArgs eventArgs)
        {
            AddToConnections(eventArgs.Address);

            ResetConnectionAttempts();
        }

        private void ResetConnectionAttempts()
        {
            ConnectionAttempts = 0;
        }

        private void AddToConnections(IPAddress address)
        {
            IncrementConnectionAttempts();

            NodeConnectionStatus connectionStatus = NodeConnections.Connections.AddNodeToConnections(address);

            WriteOnConnectionMessage(address, connectionStatus);
        }

        private void IncrementConnectionAttempts()
        {
            ConnectionAttempts++;
        }

        private void WriteOnConnectionMessage(IPAddress address, NodeConnectionStatus connectionStatus)
        {
            var onConnectionMessageWriter = new OnConnectionMessageWriter(connectionStatus, address);

            onConnectionMessageWriter.WriteConnectionMessage();

            if (ReAttemptAnotherHandshake(connectionStatus))
            {
                TimeHelper.Helper.Sleep(750);

                AddToConnections(address);
            }
        }

        private bool ReAttemptAnotherHandshake(NodeConnectionStatus connectionStatus)
        {
            return connectionStatus == NodeConnectionStatus.ErrorInHandShake && ConnectionAttempts < 2;
        }
    }
}