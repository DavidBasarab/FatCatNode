using System;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class ServiceHostHelper : IServiceHostHelper
    {
        private static IServiceHostHelper _overridenServiceHostHelper;

        public static IServiceHostHelper Helper
        {
            get
            {
                return _overridenServiceHostHelper ?? Nested.Instance;
            }
            set
            {
                _overridenServiceHostHelper = value;
            }
        }

        public IAnnouncementService AnnouncementService { get; set; }

        public void OpenServiceHost(INode nodeInstance, Uri baseAddress)
        {
            throw new NotImplementedException();
        }

        private class Nested
        {
            internal static readonly ServiceHostHelper Instance = new ServiceHostHelper();
        }
    }
}