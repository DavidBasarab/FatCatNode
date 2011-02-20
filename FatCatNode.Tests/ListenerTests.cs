using System;
using FatCatNode.Logic;
using FatCatNode.Logic.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatNode.Tests
{
    [TestFixture]
    public class ListenerTests
    {
        public MockRepository Mocks { get; set; }

        [SetUp]
        public void SetUp()
        {
            Mocks = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();

            ServiceHostHelper.Helper = null;
            NodeAnnouncementService.AnnoucementService = null;
        }

        [Test]
        public void ANodeListenerIsCreatedWithAId()
        {
            const string nodelistenid = "NodeListenId";

            var listener = new NodeListener(nodelistenid);

            Assert.That(listener.Id, Is.EqualTo(nodelistenid));
        }

        [Test]
        public void OnLineAndOffLineEventsAreRegisterdOnStartUpOnly()
        {
            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            announcementService.OnOnlineEvent += null;
            LastCall.IgnoreArguments().Repeat.Never();

            announcementService.OnOfflineEvent += null;
            LastCall.IgnoreArguments().Repeat.Never();

            Mocks.ReplayAll();

            NodeAnnouncementService.AnnoucementService = announcementService;

            var listener = new NodeListener("JUNK");
        }

        [Test]
        public void WhenListenStartsAnAnnouncementIsMade()
        {
            StubServiceHostHelper();

            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            announcementService.OnOnlineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.OnOfflineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.Expect(v => v.Start());

            Mocks.ReplayAll();

            NodeAnnouncementService.AnnoucementService = announcementService;

            var listener = new NodeListener("JUNK");

            listener.Start(null, null);
        }

        private void StubServiceHostHelper()
        {
            IServiceHostHelper serviceHostHelper = Mocks.Stub<IServiceHostHelper>();

            ServiceHostHelper.Helper = serviceHostHelper;
        }

        [Test]
        public void WhenListenerIsStartedAServiceHostIsOpened()
        {
            INode mockNode = Mocks.DynamicMock<INode>();

            IServiceHostHelper serviceHostHelper = Mocks.DynamicMock<IServiceHostHelper>();

            StubAnnoucementService();

            Uri baseAddress = new Uri("http://fatcatnode.com/Testing");

            serviceHostHelper.Expect(v => v.OpenServiceHost(mockNode, baseAddress));

            ServiceHostHelper.Helper = serviceHostHelper;

            Mocks.ReplayAll();

            NodeListener listener = new NodeListener("TestNodeId");

            listener.Start(mockNode, baseAddress);
        }

        private void StubAnnoucementService()
        {
            IAnnouncementService announcementServiceStub = Mocks.Stub<IAnnouncementService>();

            NodeAnnouncementService.AnnoucementService = announcementServiceStub;
        }
    }
}