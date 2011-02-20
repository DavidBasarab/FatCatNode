using System;
using System.ServiceModel.Discovery;
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

        public event Action<object, AnnouncementEventArgs> OnOnlineEvent;
        public event Action<object, AnnouncementEventArgs> OnOfflineEvent;

        private class Nested
        {
            internal static readonly NodeAnnouncementService Instance = new NodeAnnouncementService();
        }
    }
}