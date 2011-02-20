using FatCatNode.Logic;
using FatCatNode.Logic.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatNode.Tests
{
    [TestFixture]
    public class ListenerTests
    {
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
            var mocks = new MockRepository();

            var announcementService = mocks.DynamicMock<IAnnouncementService>();

            announcementService.OnOnlineEvent += null;
            LastCall.IgnoreArguments().Repeat.Never();

            announcementService.OnOfflineEvent += null;
            LastCall.IgnoreArguments().Repeat.Never();

            mocks.ReplayAll();

            NodeAnnouncementService.AnnoucementService = announcementService;

            var listener = new NodeListener("JUNK");

            mocks.VerifyAll();
        }

        [Test]
        public void WhenListenStartsAnAnnouncementIsMade()
        {
            var mocks = new MockRepository();

            var announcementService = mocks.DynamicMock<IAnnouncementService>();

            announcementService.OnOnlineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.OnOfflineEvent += null;
            LastCall.IgnoreArguments();

            announcementService.Expect(v => v.Start());

            mocks.ReplayAll();

            NodeAnnouncementService.AnnoucementService = announcementService;

            var listener = new NodeListener("JUNK");

            listener.Start();

            mocks.VerifyAll();
        }
    }
}