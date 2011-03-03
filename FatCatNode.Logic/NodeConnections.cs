using System;
using System.Collections.Generic;
using System.Net;
using FatCatNode.Logic.Handlers;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class NodeConnections : INodeConnections
    {
        private static INodeConnections _overridenConnections;
        private Dictionary<IPAddress, string> _connectedNodes;

        private IRemoteNodeConnectionHelper _remoteHelper;

        public static INodeConnections Connections
        {
            get { return _overridenConnections ?? (Nested.Instance); }
            set { _overridenConnections = value; }
        }

        public string NodeId { get; set; }

        public Dictionary<IPAddress, string> ConnectedNodes
        {
            get { return _connectedNodes ?? (_connectedNodes = new Dictionary<IPAddress, string>()); }
            set { _connectedNodes = value; }
        }

        public IRemoteNodeConnectionHelper RemoteHelper
        {
            get { return _remoteHelper ?? (_remoteHelper = new RemoteConnectionHandler()); }
            set { _remoteHelper = value; }
        }

        public NodeConnectionStatus AddNodeToConnections(IPAddress address)
        {
            var connectionHandshake = new ConnectionHandshake(address, RemoteHelper, NodeId);

            connectionHandshake.PerformHandshake();

            AddToConnectedNodes(connectionHandshake);

            return connectionHandshake.ConnectionStatus;
        }

        private void AddToConnectedNodes(ConnectionHandshake connectionHandshake)
        {
            if (connectionHandshake.IsRemoteNodeConnected)
            {
                if (IsAddressNotInCollection(connectionHandshake))
                {
                    AddAddressToCollection(connectionHandshake); 
                }
            }
        }

        private void AddAddressToCollection(ConnectionHandshake connectionHandshake)
        {
            ConnectedNodes.Add(connectionHandshake.RemoteAddress, connectionHandshake.RemoteNodeId);
        }

        private bool IsAddressNotInCollection(ConnectionHandshake connectionHandshake)
        {
            return !ConnectedNodes.ContainsKey(connectionHandshake.RemoteAddress);
        }

        public string FindNodeIdByAddress(IPAddress address)
        {
            if (ConnectedNodes.ContainsKey(address))
            {
                return ConnectedNodes[address]; 
            }

            return string.Empty;
        }

        public NodeConnectionStatus RemoveNodeFromConnections(IPAddress address)
        {
            throw new NotImplementedException();
        }

        public void SetNodeId(string nodeId)
        {
            NodeId = nodeId;
        }

        private class Nested
        {
            internal static readonly NodeConnections Instance = new NodeConnections();
        }
    }
}