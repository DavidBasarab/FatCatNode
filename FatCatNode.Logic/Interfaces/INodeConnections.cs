﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FatCatNode.Logic.Interfaces
{
    public interface INodeConnections
    {
        bool AddNodeToConnections(IPAddress address);
    }
}
