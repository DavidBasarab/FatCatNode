using System;
using System.ServiceModel;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ConnectedNode : INode
    {
        public ConnectedNode(string nodeId)
        {
            NodeId = nodeId;
        }

        public string NodeId { get; set; }

        public string Handshake(HandshakeInformation args)
        {
            throw new NotImplementedException();
        }
    }
}