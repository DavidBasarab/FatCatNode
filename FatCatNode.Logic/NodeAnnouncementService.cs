using System;
using System.ServiceModel.Discovery;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class NodeAnnouncementService : IAnnouncementService
    {
        public void Start()
        {
            // TODO
        }

        public event Action<object, NodeAnnoucementEventArgs> OnOnlineEvent;
        public event Action<object, NodeAnnoucementEventArgs> OnOfflineEvent;
    }
}