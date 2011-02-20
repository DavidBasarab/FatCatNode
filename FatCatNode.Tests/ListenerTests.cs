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

            var listener = new NodeListener();

            listener.Start();

            mocks.VerifyAll();
        }

        [Test]
        public void OnLineAndOffLineEventsAreRegisterdOnStartUpOnly()
        {
            MockRepository mocks = new MockRepository();

            IAnnouncementService announcementService = mocks.DynamicMock<IAnnouncementService>();

            announcementService.OnOnlineEvent += null;
            LastCall.IgnoreArguments().Repeat.Never();

            announcementService.OnOfflineEvent += null;
            LastCall.IgnoreArguments().Repeat.Never();

            mocks.ReplayAll();

            NodeAnnouncementService.AnnoucementService = announcementService;

            NodeListener listener = new NodeListener();

            mocks.VerifyAll();
        }
    }
}