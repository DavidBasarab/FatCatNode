using System;
using System.ServiceModel;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ConnectedNode : INode
    {
        public string Handshake(string otherNodeId)
        {
            throw new NotImplementedException();
        }
    }
}