using System;
using System.Net;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class NodeConnections : INodeConnections
    {
        private static INodeConnections _overridenConnections;

        public static INodeConnections Connections
        {
            get { return _overridenConnections ?? (Nested.Instance); }
            set { _overridenConnections = value; }
        }

        public NodeConnectionStatus AddNodeToConnections(IPAddress address)
        {
            throw new NotImplementedException();
        }

        public string FindNodeIdByAddress(IPAddress address)
        {
            throw new NotImplementedException();
        }

        public NodeConnectionStatus RemoveNodeFromConnections(IPAddress address)
        {
            throw new NotImplementedException();
        }

        private class Nested
        {
            internal static readonly NodeConnections Instance = new NodeConnections();
        }
    }
}