using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FatCatNode.Logic.Interfaces
{
    [ServiceContract(Namespace = "http://fatcatnode.com/")]
    public interface INode
    {
        string Handshake(string otherNodeId);

    }
}
