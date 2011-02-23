using System;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Node : INode
    {
        private INodeConnections _connections;

        public Node(string nodeId)
        {
            RegisterForOfflineAndOnLineEvents();
            SetNodeId(nodeId);
        }

        public string Id { get; set; }

        public Uri BaseAddress
        {
            get
            {
                return AddressHelper.Helper.FindBaseAddress();
            }
        }

        public INodeConnections Connections
        {
            get 
            {
                return _connections;
            }
            set 
            {
                _connections = value;
            }
        }

        private void SetNodeId(string nodeId)
        {
            Id = nodeId;

            AddressHelper.Helper.SetNodeId(nodeId);
        }

        public void Start()
        {
            MakeServiceAnnoucement();
            OpenServiceHost();
        }

        private void OpenServiceHost()
        {
            ServiceHostHelper.Helper.OpenServiceHost(this, BaseAddress);
        }

        private static void MakeServiceAnnoucement()
        {
            NodeAnnouncementService.AnnoucementService.Start();
        }

        private void RegisterForOfflineAndOnLineEvents()
        {
            NodeAnnouncementService.AnnoucementService.OnOnlineEvent += OnOnlineEvent;
            NodeAnnouncementService.AnnoucementService.OnOfflineEvent += OnOfflineEvent;
        }

        private void OnOnlineEvent(object sender, NodeAnnoucementEventArgs e)
        {
            Connections.AddNodeToConnections(e.Address);
        }

        private void OnOfflineEvent(object sender, NodeAnnoucementEventArgs e)
        {
        }
    }
}