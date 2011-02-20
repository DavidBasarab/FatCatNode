using System.ServiceModel.Discovery;

namespace FatCatNode.Logic
{
    public class NodeListener
    {
        public void Start()
        {
            RegisterForOfflineAndOnLineEvents();

            NodeAnnouncementService.AnnoucementService.Start();
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