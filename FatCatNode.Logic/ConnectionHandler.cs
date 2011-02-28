using System;
using System.Net;
using System.ServiceModel;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    
    public class ConnectionHandler
    {
        private IAnnouncementService _announcementService;
        private IServiceHostHelper _serviceHostHelper;

        public ConnectionHandler(string nodeId, IMessageWriter messageWriter)
        {
            MessageWriter = messageWriter;

            SetNodeId(nodeId);
        }

        public ConnectionHandler(string nodeId)
        {
            SetNodeId(nodeId);
        }

        public IMessageWriter MessageWriter { get; private set; }

        public string Id { get; set; }

        public Uri BaseAddress
        {
            get { return AddressHelper.Helper.FindBaseAddress(); }
        }

        public INodeConnections Connections { get; set; }

        private INode _connectedNode;
        public INode ConnectedNode
        {
            get { return _connectedNode ?? (_connectedNode = new ConnectedNode()); }
            set { _connectedNode = value; }
        }

        private void CreateConnectedNode()
        {
            _connectedNode = new ConnectedNode();
        }

        public IAnnouncementService AnnouncementService
        {
            get { return _announcementService ?? (_announcementService = new NodeAnnouncementService()); }
            set { _announcementService = value; }
        }

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

            SleepForServiceAnnouncement();

            OpenServiceHost();
        }

        private void SleepForServiceAnnouncement()
        {
            TimeHelper.Helper.Sleep(400);
        }

        private void OpenServiceHost()
        {
            ServiceHostHelper.OpenServiceHost(ConnectedNode, BaseAddress);
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

            WriteOnConnectionMessage(address, connectionStatus);
        }

        private void WriteOnConnectionMessage(IPAddress address, NodeConnectionStatus connectionStatus)
        {
            var onConnectionMessageWriter = new OnConnectionMessageWriter(this, connectionStatus, address);
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