using System;
using System.Net;
using System.Threading;
using FatCatNode.Logic;
using FatCatNode.Logic.Arguments;
using FatCatNode.Logic.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatNode.Tests
{
    [TestFixture]
    public class NodeTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();

            ResetHelpers();
        }

        #endregion

        private const string NodeId = "UnitTestNode";

        private static void ResetHelpers()
        {
            AddressHelper.Helper = null;
            ServiceHostHelper.Helper = null;
            NodeAnnouncementService.AnnoucementService = null;
        }

        public MockRepository Mocks { get; set; }

        [Flags]
        private enum HelperFlags
        {
            ServiceHost = 1,
            Address = 2,
            Announcement = 4
        }

        private void StubHelpers(HelperFlags flag)
        {
            if (flag.IsSet(HelperFlags.ServiceHost))
            {
                StubServiceHostHelper();
            }

            if (flag.IsSet(HelperFlags.Address))
            {
                StubAddressHelper();
            }

            if (flag.IsSet(HelperFlags.Announcement))
            {
                StubAnnoucementService();
            }
        }

        private void StubAddressHelper()
        {
            var addressHelper = Mocks.Stub<IAddressHelper>();

            AddressHelper.Helper = addressHelper;
        }

        private void StubServiceHostHelper()
        {
            var serviceHostStub = Mocks.Stub<IServiceHostHelper>();

            ServiceHostHelper.Helper = serviceHostStub;
        }

        private void StubAnnoucementService()
        {
            var announcementService = Mocks.Stub<IAnnouncementService>();

            NodeAnnouncementService.AnnoucementService = announcementService;
        }

        [Test]
        public void BaseAddressWillBeCalculatedFromTheAddressHelper()
        {
            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.SetNodeId(NodeId));

            var desiredBaseAddress = new Uri("http://10.30.55.55:7777/UnitTestNode/FatCatNode");

            addressHelper.Expect(v => v.FindBaseAddress()).Return(desiredBaseAddress);

            Mocks.ReplayAll();

            AddressHelper.Helper = addressHelper;

            var node = new Node(NodeId);

            Assert.That(node.BaseAddress, Is.EqualTo(desiredBaseAddress));
        }

        [Test]
        public void NodeStartWillAnnouceTheService()
        {
            StubHelpers(HelperFlags.ServiceHost | HelperFlags.Address);

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            announcementService.OnOnlineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.OnOfflineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.Expect(v => v.Start());

            NodeAnnouncementService.AnnoucementService = announcementService;

            Mocks.ReplayAll();

            var node = new Node(NodeId);

            node.Start();
        }

        [Test]
        public void NodeStartWillOpenAServiceHostConnection()
        {
            StubHelpers(HelperFlags.Announcement);

            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.FindBaseAddress()).Return(
                new Uri("http://10.30.55.55:7777/UnitTestNode/FatCatNode"));

            AddressHelper.Helper = addressHelper;

            Mocks.ReplayAll();

            var node = new Node(NodeId);

            var serviceHostHelper = Mocks.DynamicMock<IServiceHostHelper>();

            serviceHostHelper.Expect(v => v.OpenServiceHost(node, node.BaseAddress));

            ServiceHostHelper.Helper = serviceHostHelper;

            Mocks.ReplayAll();

            node.Start();
        }

        [Test]
        public void OnConnectionEventTheNodeWillBeAddedToConnections()
        {
            StubHelpers(HelperFlags.ServiceHost | HelperFlags.Address);

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            NodeAnnouncementService.AnnoucementService = announcementService;

            IPAddress ipAddress = IPAddress.Parse("55.55.55.55");

            var args = new NodeAnnoucementEventArgs
                           {
                               Address = ipAddress
                           };

            var nodeConnections = Mocks.DynamicMock<INodeConnections>();

            nodeConnections.Expect(v => v.AddNodeToConnections(ipAddress)).Return(NodeConnectionStatus.Added);

            Mocks.ReplayAll();

            var node = new Node(NodeId) {Connections = nodeConnections};

            node.Start();

            announcementService.Raise(v => v.OnOnlineEvent += null, this, args);

            Mocks.ReplayAll();

            Thread.Sleep(100);
        }

        [Test]
        public void OnCreationNodeWillInformTheAddressHelperOfTheNodeId()
        {
            StubHelpers(HelperFlags.Announcement);

            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.SetNodeId(NodeId));

            Mocks.ReplayAll();

            AddressHelper.Helper = addressHelper;

            var node = new Node(NodeId);

            Assert.That(node.Id, Is.EqualTo(NodeId));
        }

        [Test]
        public void OnSuccessfullyConnectionAMessageWillBeSentToMessageWriter()
        {
            StubHelpers(HelperFlags.ServiceHost | HelperFlags.Address);

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            NodeAnnouncementService.AnnoucementService = announcementService;

            IPAddress ipAddress = IPAddress.Parse("55.55.55.55");

            var args = new NodeAnnoucementEventArgs
                           {
                               Address = ipAddress
                           };

            var nodeConnections = Mocks.DynamicMock<INodeConnections>();

            nodeConnections.Expect(v => v.AddNodeToConnections(ipAddress)).Return(NodeConnectionStatus.Added);
            nodeConnections.Expect(v => v.FindNodeIdByAddress(ipAddress)).Return("Node2");

            var messageWriter = Mocks.DynamicMock<IMessageWriter>();

            messageWriter.Expect(v => v.Message("A node with Id '{0}' connected from address {1}", "Node2", ipAddress));

            Mocks.ReplayAll();

            var node = new Node(NodeId, messageWriter)
                           {
                               Connections = nodeConnections
                           };

            node.Start();

            announcementService.Raise(v => v.OnOnlineEvent += null, this, args);

            Mocks.ReplayAll();

            Thread.Sleep(100);
        }

        [Test]
        public void IfANodeCannotBeConnectedToAMessageWritten()
        {
            StubHelpers(HelperFlags.ServiceHost | HelperFlags.Address);

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            NodeAnnouncementService.AnnoucementService = announcementService;

            IPAddress ipAddress = IPAddress.Parse("55.55.55.55");

            var args = new NodeAnnoucementEventArgs
            {
                Address = ipAddress
            };

            var nodeConnections = Mocks.DynamicMock<INodeConnections>();

            nodeConnections.Expect(v => v.AddNodeToConnections(ipAddress)).Return(NodeConnectionStatus.CouldNotConnect);
            nodeConnections.Expect(v => v.FindNodeIdByAddress(ipAddress)).IgnoreArguments().Repeat.Never();

            var messageWriter = Mocks.DynamicMock<IMessageWriter>();

            messageWriter.Expect(v => v.Message("A node from address {0} could not be connected.", ipAddress));

            Mocks.ReplayAll();

            var node = new Node(NodeId, messageWriter)
            {
                Connections = nodeConnections
            };

            node.Start();

            announcementService.Raise(v => v.OnOnlineEvent += null, this, args);

            Mocks.ReplayAll();

            Thread.Sleep(100);
        }
    }
}