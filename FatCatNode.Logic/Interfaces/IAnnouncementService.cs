using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Discovery;
using System.Text;
using FatCatNode.Logic.Arguments;

namespace FatCatNode.Logic.Interfaces
{
    public interface IAnnouncementService
    {
        void Start();

        event Action<object, NodeAnnoucementEventArgs> OnOnlineEvent;
        event Action<object, NodeAnnoucementEventArgs> OnOfflineEvent;
    }
}
