﻿using System;
using System.Net;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class ConnectionsHandler
    {
        private IAnnouncementService _announcementService;
        private INode _connectedNode;
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

        public INodeConnections Connections { get; set; }

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

            onConnectionMessageWriter.WriteConnectionMessage();
        }

        private void OnOfflineEvent(object sender, NodeAnnoucementEventArgs e)
        {
        }
    }
}