using System;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Node : INode
    {
        public Node(string nodeId)
        {
            SetNodeId(nodeId);

            RegisterForOfflineAndOnLineEvents();
        }

        private void SetNodeId(string nodeId)
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
            NodeAnnouncementService.AnnoucementService.Start();

            ServiceHostHelper.Helper.OpenServiceHost(this, BaseAddress);
        }

        private void RegisterForOfflineAndOnLineEvents()
        {
            NodeAnnouncementService.AnnoucementService.OnOnlineEvent += OnOnlineEvent;
            NodeAnnouncementService.AnnoucementService.OnOfflineEvent += OnOfflineEvent;
        }

        private void OnOnlineEvent(object sender, AnnouncementEventArgs e)
        {
        }

        private void OnOfflineEvent(object sender, AnnouncementEventArgs e)
        {
        }
    }
}