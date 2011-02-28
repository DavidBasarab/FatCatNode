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
        private static readonly Uri BaseAddress = new Uri("http://10.30.55.55:7777/UnitTestNode/FatCatNode");

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            CreateMockRepository();

            StubTimeHelper();
        }

        private void CreateMockRepository()
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
            TimeHelper.Helper = null;
        }

        public MockRepository Mocks { get; set; }

        [Flags]
        private enum HelperFlags
        {
            Address = 2
        }

        private void StubHelpers(HelperFlags flag)
        {
            if (flag.IsSet(HelperFlags.Address))
            {
                StubAddressHelper();
            }
        }

        private void StubTimeHelper()
        {
            ITimeHelper timeHelper = Mocks.Stub<ITimeHelper>();

            TimeHelper.Helper = timeHelper;
        }

        private void StubAddressHelper()
        {
            var addressHelper = Mocks.Stub<IAddressHelper>();

            AddressHelper.Helper = addressHelper;
        }

        private IServiceHostHelper StubServiceHostHelper()
        {
            var serviceHostStub = Mocks.Stub<IServiceHostHelper>();

            return serviceHostStub;
        }

        private IAnnouncementService StubAnnoucementService()
        {
            var announcementService = Mocks.Stub<IAnnouncementService>();

            return announcementService;
        }

        [Test]
        public void BaseAddressWillBeCalculatedFromTheAddressHelper()
        {
            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.SetNodeId(NodeId));

            var desiredBaseAddress = BaseAddress;

            addressHelper.Expect(v => v.FindBaseAddress()).Return(desiredBaseAddress);

            Mocks.ReplayAll();

            AddressHelper.Helper = addressHelper;

            var node = new Node(NodeId);

            Assert.That(node.BaseAddress, Is.EqualTo(desiredBaseAddress));
        }

        [Test]
        public void IfANodeCannotBeConnectedToAMessageWritten()
        {
            StubHelpers(HelperFlags.Address);
            IServiceHostHelper serviceHostHelper = StubServiceHostHelper();

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

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
                               Connections = nodeConnections,
                               AnnouncementService = announcementService,
                               ServiceHostHelper = serviceHostHelper
                           };

            node.Start();

            announcementService.Raise(v => v.OnOnlineEvent += null, this, args);

            Mocks.ReplayAll();

            Thread.Sleep(100);
        }

        [Test]
        public void IfANodeIsAlreadyConnectedAMessageIsWritten()
        {
            StubHelpers(HelperFlags.Address);
            IServiceHostHelper serviceHostHelper = StubServiceHostHelper();

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            IPAddress ipAddress = IPAddress.Parse("55.55.55.55");

            var args = new NodeAnnoucementEventArgs
            {
                Address = ipAddress
            };

            var nodeConnections = Mocks.DynamicMock<INodeConnections>();

            nodeConnections.Expect(v => v.AddNodeToConnections(ipAddress)).Return(NodeConnectionStatus.AlreadyConnected);
            nodeConnections.Expect(v => v.FindNodeIdByAddress(ipAddress)).Return("Node2");

            var messageWriter = Mocks.DynamicMock<IMessageWriter>();

            messageWriter.Expect(v => v.Message("A node from address {0} is already connected with an Id of {1}.", ipAddress, "Node2"));

            Mocks.ReplayAll();

            var node = new Node(NodeId, messageWriter)
            {
                Connections = nodeConnections,
                AnnouncementService = announcementService,
                ServiceHostHelper = serviceHostHelper
            };

            node.Start();

            announcementService.Raise(v => v.OnOnlineEvent += null, this, args);

            Mocks.ReplayAll();

            Thread.Sleep(150);
        }

        [Test]
        public void NodeStartWillAnnouceTheService()
        {
            StubHelpers(HelperFlags.Address);
            IServiceHostHelper serviceHostHelper = StubServiceHostHelper();

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            announcementService.OnOnlineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.OnOfflineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.Expect(v => v.Start());

            Mocks.ReplayAll();

            var node = new Node(NodeId)
                           {
                               ServiceHostHelper = serviceHostHelper,
                               AnnouncementService = announcementService
                           };

            node.Start();
        }

        [Test]
        public void NodeStartWillOpenAServiceHostConnection()
        {
            CreateAddressHelperReturningBaseAddress();

            Mocks.ReplayAll();

            var node = CreateNode();

            var serviceHostHelper = Mocks.DynamicMock<IServiceHostHelper>();

            serviceHostHelper.Expect(v => v.OpenServiceHost(node, node.BaseAddress));

            Mocks.ReplayAll();

            node.ServiceHostHelper = serviceHostHelper;

            node.Start();
        }

        private void CreateAddressHelperReturningBaseAddress()
        {
            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.FindBaseAddress()).Return(BaseAddress);

            AddressHelper.Helper = addressHelper;
        }

        [Test]
        public void ANodeIsGoingToConnectAnnounceServiceIsStartedAThreadSleepsBeforeOpening()
        {
            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            IAnnouncementService announcementService = Mocks.Stub<IAnnouncementService>();

            ITimeHelper timeHelper = Mocks.DynamicMock<ITimeHelper>();
            
            IServiceHostHelper serviceHostHelper = Mocks.DynamicMock<IServiceHostHelper>();

            using (Mocks.Ordered())
            {
                announcementService.OnOnlineEvent += null;
                LastCall.IgnoreArguments();

                announcementService.OnOfflineEvent += null;
                LastCall.IgnoreArguments();

                announcementService.Expect(v => v.Start());

                timeHelper.Expect(v => v.Sleep(400));

                addressHelper.Expect(v => v.FindBaseAddress()).Return(BaseAddress);

                serviceHostHelper.Expect(v => v.OpenServiceHost(null, null)).IgnoreArguments(); 
            }

            AddressHelper.Helper = addressHelper;
            TimeHelper.Helper = timeHelper;
            
            Mocks.ReplayAll();

            var node = CreateNode();

            node.AnnouncementService = announcementService;
            node.ServiceHostHelper = serviceHostHelper;

            node.Start();
        }

        private Node CreateNode()
        {
            return new Node(NodeId);
        }

        [Test]
        public void OnConnectionEventTheNodeWillBeAddedToConnections()
        {
            StubHelpers(HelperFlags.Address);
            IServiceHostHelper serviceHostHelper = StubServiceHostHelper();

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            IPAddress ipAddress = IPAddress.Parse("55.55.55.55");

            var args = new NodeAnnoucementEventArgs
                           {
                               Address = ipAddress
                           };

            var nodeConnections = Mocks.DynamicMock<INodeConnections>();

            nodeConnections.Expect(v => v.AddNodeToConnections(ipAddress)).Return(NodeConnectionStatus.Added);

            Mocks.ReplayAll();

            var node = new Node(NodeId)
                           {
                               Connections = nodeConnections,
                               AnnouncementService = announcementService,
                               ServiceHostHelper = serviceHostHelper
                           };

            node.Start();

            announcementService.Raise(v => v.OnOnlineEvent += null, this, args);

            Mocks.ReplayAll();

            Thread.Sleep(100);
        }

        [Test]
        public void OnCreationNodeWillInformTheAddressHelperOfTheNodeId()
        {
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
            StubHelpers(HelperFlags.Address);
            IServiceHostHelper serviceHostHelper = StubServiceHostHelper();

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

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
                               Connections = nodeConnections,
                               AnnouncementService = announcementService,
                               ServiceHostHelper = serviceHostHelper
                           };

            node.Start();

            announcementService.Raise(v => v.OnOnlineEvent += null, this, args);

            Mocks.ReplayAll();

            Thread.Sleep(100);
        }
    }
}