using System;
using System.Net;
using System.ServiceModel;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Node : INode
    {
        private IAnnouncementService _announcementService;

        public Node(string nodeId, IMessageWriter messageWriter)
        {
            MessageWriter = messageWriter;
            
            SetNodeId(nodeId);
        }

        public Node(string nodeId)
        {
            SetNodeId(nodeId);
        }

        private IMessageWriter MessageWriter { get; set; }

        public string Id { get; set; }

        public Uri BaseAddress
        {
            get { return AddressHelper.Helper.FindBaseAddress(); }
        }

        public INodeConnections Connections { get; set; }

        public IAnnouncementService AnnouncementService
        {
            get { return _announcementService ?? (_announcementService = new NodeAnnouncementService()); }
            set { _announcementService = value; }
        }

        private IServiceHostHelper _serviceHostHelper;
        public IServiceHostHelper ServiceHostHelper
        {
            get { return _serviceHostHelper ?? (_serviceHostHelper = new ServiceHostHelper()); }
            set { _serviceHostHelper = value; }
        }

        private void Initialize()
        {
            RegisterForOfflineAndOnLineEvents();
        }

        private void SetNodeId(string nodeId)
        {
            Id = nodeId;

            AddressHelper.Helper.SetNodeId(nodeId);
        }

        public void Start()
        {
            Initialize();

            MakeServiceAnnoucement();
            OpenServiceHost();
        }

        private void OpenServiceHost()
        {
            ServiceHostHelper.OpenServiceHost(this, BaseAddress);
        }

        private void MakeServiceAnnoucement()
        {
            AnnouncementService.Start();
        }

        private void RegisterForOfflineAndOnLineEvents()
        {
            AnnouncementService.OnOnlineEvent += OnOnlineEvent;
            AnnouncementService.OnOfflineEvent += OnOfflineEvent;
        }

        private void OnOnlineEvent(object sender, NodeAnnoucementEventArgs e)
        {
            AddToConnections(e.Address);
        }

        private void AddToConnections(IPAddress address)
        {
            NodeConnectionStatus connectionStatus = Connections.AddNodeToConnections(address);

            if (SuccessfullyConnected(connectionStatus))
            {
                WriteSuccessfullyConnectionMessage(address);
            }
            else if (DidNotSuccessfullyConnect(connectionStatus))
            {
                WriteCouldNotConnectMessage(address);
            }
        }

        private static bool DidNotSuccessfullyConnect(NodeConnectionStatus connectionStatus)
        {
            return connectionStatus == NodeConnectionStatus.CouldNotConnect;
        }

        private void WriteCouldNotConnectMessage(IPAddress address)
        {
            WriteMessage("A node from address {0} could not be connected.", address);
        }

        private static bool SuccessfullyConnected(NodeConnectionStatus connectionStatus)
        {
            return connectionStatus == NodeConnectionStatus.Added;
        }

        private void WriteSuccessfullyConnectionMessage(IPAddress address)
        {
            WriteMessage("A node with Id '{0}' connected from address {1}", Connections.FindNodeIdByAddress(address),
                         address);
        }

        private void WriteMessage(string message, params object[] args)
        {
            if (MessageWriter != null)
            {
                MessageWriter.Message(message, args);
            }
        }

        private void OnOfflineEvent(object sender, NodeAnnoucementEventArgs e)
        {
        }
    }
}