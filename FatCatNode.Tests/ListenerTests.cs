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

            ServiceHostHelper.Helper = null;
            NodeAnnouncementService.AnnoucementService = null;
        }

        #endregion

        public MockRepository Mocks { get; set; }

        private void StubServiceHostHelper()
        {
            var serviceHostHelper = Mocks.Stub<IServiceHostHelper>();

            ServiceHostHelper.Helper = serviceHostHelper;
        }

        private void StubAnnoucementService()
        {
            var announcementServiceStub = Mocks.Stub<IAnnouncementService>();

            NodeAnnouncementService.AnnoucementService = announcementServiceStub;
        }

        [Test]
        public void ANodeListenerIsCreatedWithAId()
        {
            const string nodelistenid = "NodeListenId";

            var listener = new NodeListener(nodelistenid);

            Assert.That(listener.Id, Is.EqualTo(nodelistenid));
        }

        [Test]
        public void AnnouncementServiceStartIsCalledBeforeTheServiceHostIsOpened()
        {
            var announcementService = Mocks.DynamicMock<IAnnouncementService>();

            announcementService.OnOnlineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.OnOfflineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.Expect(v => v.Start());

            NodeAnnouncementService.AnnoucementService = announcementService;

            var serviceHostHelper = Mocks.DynamicMock<IServiceHostHelper>();

            serviceHostHelper.Expect(v => v.OpenServiceHost(null, null)).IgnoreArguments();

            ServiceHostHelper.Helper = serviceHostHelper;

            Mocks.ReplayAll();

            Mocks.Ordered();

            var listener = new NodeListener("JUNK");

            listener.Start(null, null);
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

        [Test]
        public void WhenListenerIsStartedAServiceHostIsOpened()
        {
            var mockNode = Mocks.DynamicMock<INode>();

            var serviceHostHelper = Mocks.DynamicMock<IServiceHostHelper>();

            StubAnnoucementService();

            var baseAddress = new Uri("http://fatcatnode.com/Testing");

            serviceHostHelper.Expect(v => v.OpenServiceHost(mockNode, baseAddress));

            ServiceHostHelper.Helper = serviceHostHelper;

            Mocks.ReplayAll();

            var listener = new NodeListener("TestNodeId");

            listener.Start(mockNode, baseAddress);
        }
    }
}