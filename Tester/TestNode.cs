using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Tester
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TestNode : ITestNode
    {
        private readonly object _lockObj = new object();

        public Dictionary<string, TestNode> ConnectedTestNodes { get; set; }

        public void Message(string nodeId, string message, params object[] args)
        {
            lock (_lockObj)
            {
                ConsoleColor prevColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Green;

                string fullMessage = string.Format(message, args);

                Console.WriteLine("Message From {1}: {0}", fullMessage, nodeId);

                Console.ForegroundColor = prevColor;
            }
        }

        public void HandShake(string requestNodeId, Uri requestingAddress)
        {
            throw new NotImplementedException();
        }

        public NodeStatus GetStatus(string requestNodeId)
        {
            throw new NotImplementedException();
        }
    }
}