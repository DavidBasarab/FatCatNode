using System;
using System.ServiceModel;

namespace FatCatNode.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Node
    {
        public Node(string nodeId)
        {
            Id = nodeId;

            AddressHelper.Helper.SetNodeId(nodeId);
        }

        public string Id { get; set; }

        public Uri BaseAddress
        {
            get
            {
                return AddressHelper.Helper.FindBaseAddress();
            }
        }
    }
}