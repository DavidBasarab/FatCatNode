using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Discovery;
using System.Text;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class NodeListener
    {
        public NodeListener()
        {
            NodeAnnouncementService.AnnoucementService.OnOnlineEvent += OnOnlineEvent;
            NodeAnnouncementService.AnnoucementService.OnOfflineEvent += OnOfflineEvent;
        }

        public void Start()
        {
            NodeAnnouncementService.AnnoucementService.Start();
        }

        private void OnOnlineEvent(object sender, AnnouncementEventArgs e)
        {

        }

        private void OnOfflineEvent(object sender, AnnouncementEventArgs e)
        {

        }
    }
}
