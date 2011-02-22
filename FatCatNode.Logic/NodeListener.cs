using System;
using System.ServiceModel.Discovery;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class NodeListener
    {
        public NodeListener(string nodeId)
        {
            Id = nodeId;
        }

        public string Id { get; private set; }

        public void Start(INode instance, Uri baseAddress)
        {
            RegisterForOfflineAndOnLineEvents();

            NodeAnnouncementService.AnnoucementService.Start();

            ServiceHostHelper.Helper.OpenServiceHost(instance, baseAddress);
        }

        private void RegisterForOfflineAndOnLineEvents()
        {
            NodeAnnouncementService.AnnoucementService.OnOnlineEvent += OnOnlineEvent;
            NodeAnnouncementService.AnnoucementService.OnOfflineEvent += OnOfflineEvent;
        }

        private void OnOnlineEvent(object sender, NodeAnnoucementEventArgs e)
        {
        }

        private void OnOfflineEvent(object sender, NodeAnnoucementEventArgs e)
        {
        }
    }
}