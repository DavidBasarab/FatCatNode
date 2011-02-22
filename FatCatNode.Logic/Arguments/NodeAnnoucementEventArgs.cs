using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FatCatNode.Logic.Arguments
{
    public class NodeAnnoucementEventArgs : EventArgs
    {
        public new static readonly NodeAnnoucementEventArgs Empty = new NodeAnnoucementEventArgs();

        public IPAddress Address { get; set; }
    }
}
