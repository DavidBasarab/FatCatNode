using System;
using System.ServiceModel.Discovery;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class NodeAnnouncementService : IAnnouncementService
    {
        private static IAnnouncementService _overridenNodeAnnoucement;

        public static IAnnouncementService AnnoucementService
        {
            get
            {
                return _overridenNodeAnnoucement ?? Nested.Instance;
            }
            set
            {
                _overridenNodeAnnoucement = value;
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public event Action<object, NodeAnnoucementEventArgs> OnOnlineEvent;
        public event Action<object, NodeAnnoucementEventArgs> OnOfflineEvent;

        private class Nested
        {
            internal static readonly NodeAnnouncementService Instance = new NodeAnnouncementService();
        }
    }
}