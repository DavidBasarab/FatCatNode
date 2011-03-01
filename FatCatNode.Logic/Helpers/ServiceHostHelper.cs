using System;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic.Helpers
{
    public class ServiceHostHelper : IServiceHostHelper
    {
        public IAnnouncementService AnnouncementService { get; set; }

        public void OpenServiceHost(INode nodeInstance, Uri baseAddress)
        {
            // TODO Implement
        }
    }
}