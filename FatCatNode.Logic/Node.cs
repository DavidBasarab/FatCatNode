using System;
using System.ServiceModel;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Node : INode
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

        public void Start()
        {
            ServiceHostHelper.Helper.OpenServiceHost(this, BaseAddress);
        }
    }
}