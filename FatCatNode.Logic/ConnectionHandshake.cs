using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class ConnectionHandshake
    {
        private IRemoteNodeConnectionHelper RemoteConnection { get; set; }
        private IPAddress Address { get; set; }

        public ConnectionHandshake(IPAddress address, IRemoteNodeConnectionHelper remoteConnection)
        {
            Address = address;
            RemoteConnection = remoteConnection;
        }

        public NodeConnectionStatus PerformHandshake()
        {
            try
            {
                MessageWriter.Writer.Message("Attempting to connect to node at address {0}.", Address);

                RemoteConnection.OpenRemoteConnection(Address);

                return NodeConnectionStatus.None;
            }
            catch (Exception)
            {
                MessageWriter.Writer.Message("Error connecting to node at address {0}.", Address);

                return NodeConnectionStatus.CouldNotConnect;
            }
        }
    }
}
