using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using FatCatNode.Logic.Arguments;

namespace FatCatNode.Logic.Interfaces
{
    [ServiceContract(Namespace = "http://fatcatnode.com/INode")]
    public interface INode
    {
        string Handshake(HandshakeInformation args);

    }
}
