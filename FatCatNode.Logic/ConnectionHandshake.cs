using System;
using System.Net;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class ConnectionHandshake
    {
        public ConnectionHandshake(IPAddress address, IRemoteNodeConnectionHelper remoteConnection, string nodeId)
        {
            Address = address;
            RemoteConnection = remoteConnection;
            NodeId = nodeId;
        }

        public string NodeId { get; set; }

        private IRemoteNodeConnectionHelper RemoteConnection { get; set; }
        
        private IPAddress Address { get; set; }

        public INode RemoteNode { get; set; }

        public string RemoteNodeId { get; set; }

        public NodeConnectionStatus ConnectionStatus { get; set; }

        public NodeConnectionStatus PerformHandshake()
        {
            try
            {
                WriteAttemptingConnectionMessage();

                FindRemoteNode();

                FindRemoteNodeId();

                DetermineConnectionStatus();
            }
            catch (Exception)
            {
                WriteConnectionErrorMessage();

                ConnectionStatus = NodeConnectionStatus.CouldNotConnect;
            }

            return ConnectionStatus;
        }

        private void DetermineConnectionStatus()
        {
            if (IsRemoteNodeEmpty())
            {
                ConnectionStatus = NodeConnectionStatus.CouldNotConnect;
            }
            else if (IsRemoteNodePopulated())
            {
                ConnectionStatus = NodeConnectionStatus.Connected;
            }

        }

        private bool IsRemoteNodePopulated()
        {
            return !string.IsNullOrEmpty(RemoteNodeId);
        }

        private bool IsRemoteNodeEmpty()
        {
            return string.IsNullOrEmpty(RemoteNodeId);
        }

        private void FindRemoteNodeId()
        {
            RemoteNodeId = RemoteNode.Handshake(NodeId);
        }

        private void FindRemoteNode()
        {
            RemoteNode = RemoteConnection.OpenRemoteConnection(Address);
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