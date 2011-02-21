using FatCatNode.Logic;
using FatCatNode.Logic.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatNode.Tests
{
    [TestFixture]
    public class NodeTest
    {
        public MockRepository MockRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockRepository = new MockRepository();
        }

        [Test]
        public void OnCreationNodeWillInformTheAddressHelperOfTheNodeId()
        {
            var mocks = new MockRepository();

            var addressHelper = mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.SetNodeId("UnitTestNode"));

            mocks.ReplayAll();

            AddressHelper.Helper = addressHelper;

            var node = new Node("UnitTestNode");

            Assert.That(node.Id, Is.EqualTo("UnitTestNode"));

            mocks.VerifyAll();
        }

        [Test]
        public void NodeStartWillOpenAServiceHostConnection()
        {
            Assert.Fail();
        }
    }
}