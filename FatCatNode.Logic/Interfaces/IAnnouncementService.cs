using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Discovery;
using System.Text;

namespace FatCatNode.Logic.Interfaces
{
    public interface IAnnouncementService
    {
        void Start();

        event Action<object, AnnouncementEventArgs> OnOnlineEvent;
        event Action<object, AnnouncementEventArgs> OnOfflineEvent;
    }
}
