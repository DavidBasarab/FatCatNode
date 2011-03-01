using System;
using System.Net;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class ConnectionHandshake
    {
        public ConnectionHandshake(IPAddress address, IRemoteNodeConnectionHelper remoteConnection)
        {
            Address = address;
            RemoteConnection = remoteConnection;
        }

        private IRemoteNodeConnectionHelper RemoteConnection { get; set; }
        private IPAddress Address { get; set; }

        public NodeConnectionStatus PerformHandshake()
        {
            try
            {
                WriteAttemptingConnectionMessage();

                RemoteConnection.OpenRemoteConnection(Address);

                return NodeConnectionStatus.None;
            }
            catch (Exception)
            {
                WriteConnectionErrorMessage();

                return NodeConnectionStatus.CouldNotConnect;
            }
        }

        private void WriteConnectionErrorMessage()
        {
            MessageWriter.Writer.Message("Error connecting to node at address {0}.", Address);
        }

        private void WriteAttemptingConnectionMessage()
        {
            MessageWriter.Writer.Message("Attempting to connect to node at address {0}.", Address);
        }
    }
}