using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatCatNode.Logic
{
    public class Node
    {
        public Node(string nodeId)
        {
            AddressHelper.Helper.SetNodeId(nodeId);
        }
    }
}
