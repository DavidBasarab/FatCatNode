using System;
using System.Net;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic.Helpers
{
    public class AddressHelper : IAddressHelper
    {
        private static IAddressHelper _overridenHelper;

        public static IAddressHelper Helper
        {
            get { return _overridenHelper ?? Nested.Instance; }
            set { _overridenHelper = value; }
        }

        public void SetNodeId(string nodeId)
        {
            throw new NotImplementedException();
        }

        public IPAddress FindComputerIpAddress()
        {
            throw new NotImplementedException();
        }

        public Uri FindBaseAddress()
        {
            throw new NotImplementedException();
        }

        private class Nested
        {
            internal static readonly AddressHelper Instance = new AddressHelper();
        }
    }
}