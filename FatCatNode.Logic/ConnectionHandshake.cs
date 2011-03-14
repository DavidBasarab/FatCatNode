using System;
using System.Net;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Helpers;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class ConnectionHandshake
    {
        public ConnectionHandshake(IPAddress address, IRemoteNodeConnectionHelper remoteConnection, string nodeId)
        {
            RemoteAddress = address;
            RemoteConnection = remoteConnection;
            NodeId = nodeId;
        }

        public string NodeId { get; set; }

        private IRemoteNodeConnectionHelper RemoteConnection { get; set; }

        public IPAddress RemoteAddress { get; private set; }

        public INode RemoteNode { get; set; }

        public string RemoteNodeId { get; set; }

        public NodeConnectionStatus ConnectionStatus { get; set; }

        public bool IsRemoteNodeConnected
        {
            get
            {
                return ConnectionStatus == NodeConnectionStatus.Connected;
            }
        }

        public void PerformHandshake()
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
                HandleError();
            }
        }

        private void HandleError()
        {
            WriteConnectionErrorMessage();

            ConnectionStatus = NodeConnectionStatus.ErrorInHandShake;
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
            RemoteNodeId = RemoteNode.Handshake(CreateHandshakeInformation());
        }

        private HandshakeInformation CreateHandshakeInformation()
        {
            return new HandshakeInformation()
                       {
                           RemoteNodeAddress = AddressHelper.Helper.FindComputerIpAddress(),
                           RemoteNodeId = NodeId
                       };
        }

        private void FindRemoteNode()
        {
            RemoteNode = RemoteConnection.OpenRemoteConnection(RemoteAddress);
        }

        private void WriteConnectionErrorMessage()
        {
            MessageWriter.Writer.Message("Error connecting to node at address {0}.", RemoteAddress);
        }

        private void WriteAttemptingConnectionMessage()
        {
            MessageWriter.Writer.Message("Attempting to connect to node at address {0}.", RemoteAddress);
        }
    }
}