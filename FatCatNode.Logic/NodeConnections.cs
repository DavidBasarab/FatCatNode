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

        public IRemoteNodeConnectionHelper RemoteHelper
        {
            get { return _remoteHelper ?? (_remoteHelper = new RemoteConnectionHandler()); }
            set { _remoteHelper = value; }
        }

        public NodeConnectionStatus AddNodeToConnections(IPAddress address)
        {
            ConnectionHandshake connectionHandshake = new ConnectionHandshake(address, RemoteHelper);

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
            throw new NotImplementedException();
        }

        private class Nested
        {
            internal static readonly NodeConnections Instance = new NodeConnections();
        }
    }
}