using System;
using System.Net;
using FatCatNode.Logic.Handlers;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class NodeConnections : INodeConnections
    {
        private static INodeConnections _overridenConnections;

        private IRemoteNodeConnectionHelper _remoteHelper;

        public static INodeConnections Connections
        {
            get { return _overridenConnections ?? (Nested.Instance); }
            set { _overridenConnections = value; }
        }

        public string NodeId { get; set; }

        public IRemoteNodeConnectionHelper RemoteHelper
        {
            get { return _remoteHelper ?? (_remoteHelper = new RemoteConnectionHandler()); }
            set { _remoteHelper = value; }
        }

        public NodeConnectionStatus AddNodeToConnections(IPAddress address)
        {
            var connectionHandshake = new ConnectionHandshake(address, RemoteHelper, NodeId);

            return connectionHandshake.PerformHandshake();
        }

        public string FindNodeIdByAddress(IPAddress address)
        {
            throw new NotImplementedException();
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