using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FatCatNode.Logic.Interfaces
{
    public interface IAddressHelper
    {
        void SetNodeId(string nodeId);

        IPAddress FindComputerIpAddress();

        Uri FindBaseAddress();
    }
}
