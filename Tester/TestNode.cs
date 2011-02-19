using System;
using System.ServiceModel;

namespace Tester
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TestNode : ITestNode
    {
        private readonly object _lockObj = new object();

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
    }
}