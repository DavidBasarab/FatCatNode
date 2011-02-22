using System;
using FatCatNode.Logic;
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

        private static void ResetHelpers()
        {
            AddressHelper.Helper = null;
            ServiceHostHelper.Helper = null;
        }

        public MockRepository Mocks { get; set; }

        [Test]
        public void BaseAddressWillBeCalculatedFromTheAddressHelper()
        {
            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.SetNodeId("UnitTestNode"));

            var desiredBaseAddress = new Uri("http://10.30.55.55:7777/UnitTestNode/FatCatNode");

            addressHelper.Expect(v => v.FindBaseAddress()).Return(desiredBaseAddress);

            Mocks.ReplayAll();

            AddressHelper.Helper = addressHelper;

            var node = new Node("UnitTestNode");

            Assert.That(node.BaseAddress, Is.EqualTo(desiredBaseAddress));
        }

        [Test]
        [Ignore("Waiting on getting base Address set up on Node")]
        public void NodeStartWillOpenAServiceHostConnection()
        {
            //IServiceHostHelper serviceHostHelper = Mocks.DynamicMock<IServiceHostHelper>();

            //serviceHostHelper.Expect(v => v.OpenServiceHost(node, node.BaseAddress));

            //ServiceHostHelper.Helper = serviceHostHelper;

            Assert.Fail();
        }

        [Test]
        public void OnCreationNodeWillInformTheAddressHelperOfTheNodeId()
        {
            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.SetNodeId("UnitTestNode"));

            Mocks.ReplayAll();

            AddressHelper.Helper = addressHelper;

            var node = new Node("UnitTestNode");

            Assert.That(node.Id, Is.EqualTo("UnitTestNode"));
        }
    }
}