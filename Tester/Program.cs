using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.Threading;

namespace Tester
{
    internal class Program
    {
        private static readonly object LockObj = new object();
        public static ServiceHost Host { get; set; }
        public static ServiceHost AnnouncementServiceHost { get; set; }

        public static string TesterId { get; set; }

        private static Uri _baseAddress;

        public static Uri BaseAddress
        {
            get
            {
                return _baseAddress ?? (_baseAddress = FindNodeAddress());
            }
        }

        private static void Main()
        {
            try
            {
                Console.Write("Please enter tester id: ");
                TesterId = Console.ReadLine();

                Console.WriteLine(Environment.NewLine);

                StartAnnouncementService();

                Thread.Sleep(400);

                var testNode = new TestNode();

                Host = new ServiceHost(testNode, BaseAddress);

                Host.AddServiceEndpoint(typeof (ITestNode), new WSHttpBinding(), string.Empty);

                var serviceDiscoveryBehavior = new ServiceDiscoveryBehavior();

                serviceDiscoveryBehavior.AnnouncementEndpoints.Add(new UdpAnnouncementEndpoint());

                Host.Description.Behaviors.Add(serviceDiscoveryBehavior);

                Host.AddServiceEndpoint(new UdpDiscoveryEndpoint());

                Host.Open();


                Console.WriteLine("Press any key to exit . . . .");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                PrintErrorMessageToConsole(ex);
            }
        }

        private static Uri FindNodeAddress()
        {
            Guid id = Guid.NewGuid();

            string hostName = Dns.GetHostName();

            string ipAddress = Dns.GetHostEntry(hostName).FindIPAddress();

            return new Uri(string.Format("http://{0}:21213/{1}/FatCatNode", ipAddress, id));
        }

        private static void PrintErrorMessageToConsole(Exception ex)
        {
            lock (LockObj)
            {
                ConsoleColor prevColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("Error Message: {0}", ex.Message);
                Console.WriteLine("Stacktrace: {0}", ex.StackTrace);

                Console.ForegroundColor = prevColor;
            }
        }

        private static void StartAnnouncementService()
        {
            var announcementService = new AnnouncementService();

            announcementService.OnlineAnnouncementReceived += OnOnlineEvent;
            announcementService.OfflineAnnouncementReceived += OnOfflineEvent;

            AnnouncementServiceHost = new ServiceHost(announcementService);

            AnnouncementServiceHost.AddServiceEndpoint(new UdpAnnouncementEndpoint());
            AnnouncementServiceHost.Open();

            Console.WriteLine("Listening for service announcements.");

            Console.WriteLine();
        }

        private static void OnOnlineEvent(object sender, AnnouncementEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Received an online announcement from {0}", e.EndpointDiscoveryMetadata.Address);

            ConnectToNode(e.EndpointDiscoveryMetadata.Address.Uri);
        }

        private static void OnOfflineEvent(object sender, AnnouncementEventArgs e)
        {
            Console.WriteLine("Received an offline announcement from {0}", e.EndpointDiscoveryMetadata.Address);

            ConnectedNode = null;
        }

        public static ITestNode ConnectedNode { get; set; }

        private static void ConnectToNode(Uri otherAddress)
        {
            if (BaseAddress == otherAddress)
            {
                return;
            }

            ChannelFactory<ITestNode> channelFactory = CreateChannelFactory<ITestNode>(otherAddress, new WSHttpBinding());

            ConnectedNode = channelFactory.CreateChannel();

            ConnectedNode.Message("I just connected to you at {0} | From: {1}", DateTime.Now, TesterId);
        }

        public static ChannelFactory<T> CreateChannelFactory<T>(Uri endpointAddress, Binding binding)
        {
            Type contractType = typeof(T);

            ServiceEndpoint endpoint = CreateServiceEndpoint(contractType, binding, endpointAddress);

            return new ChannelFactory<T>(endpoint);
        }

        public static ServiceEndpoint CreateServiceEndpoint(Type contractType, Binding binding, Uri endpointAddress)
        {
            return new ServiceEndpoint(ContractDescription.GetContract(contractType), binding, new EndpointAddress(endpointAddress));
        }
    }
}