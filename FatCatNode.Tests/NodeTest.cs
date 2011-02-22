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

        private const string NodeId = "UnitTestNode";

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

            addressHelper.Expect(v => v.SetNodeId(NodeId));

            var desiredBaseAddress = new Uri("http://10.30.55.55:7777/UnitTestNode/FatCatNode");

            addressHelper.Expect(v => v.FindBaseAddress()).Return(desiredBaseAddress);

            Mocks.ReplayAll();

            AddressHelper.Helper = addressHelper;

            var node = new Node(NodeId);

            Assert.That(node.BaseAddress, Is.EqualTo(desiredBaseAddress));
        }

        [Test]
        public void NodeStartWillOpenAServiceHostConnection()
        {
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
        public void OnCreationNodeWillInformTheAddressHelperOfTheNodeId()
        {
            var addressHelper = Mocks.DynamicMock<IAddressHelper>();

            addressHelper.Expect(v => v.SetNodeId(NodeId));

            Mocks.ReplayAll();

            AddressHelper.Helper = addressHelper;

            var node = new Node(NodeId);

            Assert.That(node.Id, Is.EqualTo(NodeId));
        }
    }
}