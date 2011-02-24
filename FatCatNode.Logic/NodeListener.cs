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

        public IAnnouncementService AnnouncementService { get; set; }

        public IServiceHostHelper ServiceHostHelper { get; set; }

        public void Start(INode instance, Uri baseAddress)
        {
            RegisterForOfflineAndOnLineEvents();

            AnnouncementService.Start();

            ServiceHostHelper.OpenServiceHost(instance, baseAddress);
        }

        private void RegisterForOfflineAndOnLineEvents()
        {
            AnnouncementService.OnOnlineEvent += OnOnlineEvent;
            AnnouncementService.OnOfflineEvent += OnOfflineEvent;
        }

        private void OnOnlineEvent(object sender, NodeAnnoucementEventArgs e)
        {
        }

        private void OnOfflineEvent(object sender, NodeAnnoucementEventArgs e)
        {
        }
    }
}