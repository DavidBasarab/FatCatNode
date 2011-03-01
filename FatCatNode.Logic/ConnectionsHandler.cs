using System;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class ConnectionsHandler
    {
        private IAnnouncementService _announcementService;
        private INode _connectedNode;
        private DisconnectionNodeHandler _disconnectionNodeHandler;
        private IServiceHostHelper _serviceHostHelper;
        private ITimeHelper _timeHelper;

        public ConnectionsHandler(string nodeId, IMessageWriter messageWriter)
        {
            MessageWriter = messageWriter;

            SetNodeId(nodeId);
        }

        public ConnectionsHandler(string nodeId)
        {
            SetNodeId(nodeId);
        }

        public IMessageWriter MessageWriter { get; private set; }

        public string Id { get; set; }

        public Uri BaseAddress
        {
            get { return AddressHelper.Helper.FindBaseAddress(); }
        }

        public INode ConnectedNode
        {
            get { return _connectedNode ?? (_connectedNode = new ConnectedNode()); }
            set { _connectedNode = value; }
        }

        public ITimeHelper TimeHelper
        {
            get { return _timeHelper ?? Logic.TimeHelper.Helper; }
            set { _timeHelper = value; }
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

        public DisconnectionNodeHandler DisconnectionNodeHandler
        {
            get
            {
                return _disconnectionNodeHandler ?? (_disconnectionNodeHandler = new DisconnectionNodeHandler());
            }
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
            TimeHelper.Sleep(400);
        }

        private void OpenServiceHost()
        {
            ServiceHostHelper.OpenServiceHost(ConnectedNode, BaseAddress);
        }

        private void MakeServiceAnnoucement()
        {
            AnnouncementService.Start();
        }

        private OnNodeConnectionHandler _onNodeConnectionHandler;

        private OnNodeConnectionHandler OnNodeConnectionHandler
        {
            get { return _onNodeConnectionHandler ?? (_onNodeConnectionHandler = new OnNodeConnectionHandler()); }
        }

        private void RegisterForOfflineAndOnLineEvents()
        {
            AnnouncementService.OnOnlineEvent += OnNodeConnectionHandler.ConnectNode;
            AnnouncementService.OnOfflineEvent += DisconnectionNodeHandler.DisconnectNode;
        }
    }
}