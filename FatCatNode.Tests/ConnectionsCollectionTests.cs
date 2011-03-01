using System;
using System.Net;
using FatCatNode.Logic;
using FatCatNode.Logic.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FatCatNode.Tests
{
    [TestFixture]
    public class ConnectionsCollectionTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            CreateMockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.VerifyAll();

            ResetMessageWriter();
        }

        #endregion

        public MockRepository Mocks { get; set; }

        private void ResetMessageWriter()
        {
            MessageWriter.Writer = null;
        }

        private void CreateMockRepository()
        {
            Mocks = new MockRepository();
        }

        [Test]
        public void ANodeHasConnectedByAddressBeforeNodeConnectedMessageIsWritten()
        {
            IPAddress connectingAddress = IPAddress.Parse("237.237.237.237");

            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(connectingAddress)).Return(null);

            var messageWriter = Mocks.DynamicMock<IMessageWriter>();

            messageWriter.Expect(v => v.Message("Attempting to connect to node at address {0}.", connectingAddress));

            Mocks.ReplayAll();

            MessageWriter.Writer = messageWriter;

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnections.Connections.AddNodeToConnections(connectingAddress);
        }

        [Test]
        public void ANodeHasConnectedByAddressTheNodeConnectionAttemptThrowsExceptionCouldNotConnectStatusReturned()
        {
            IPAddress connectingAddress = IPAddress.Parse("237.237.237.237");

            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(connectingAddress)).Throw(new Exception());

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnectionStatus result = NodeConnections.Connections.AddNodeToConnections(connectingAddress);

            Assert.That(result, Is.EqualTo(NodeConnectionStatus.CouldNotConnect));
        }

        [Test]
        public void ANodeHasConnectedByAddressTheNodeIsConnected()
        {
            IPAddress connectingAddress = IPAddress.Parse("237.237.237.237");

            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(connectingAddress)).Return(null);

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnections.Connections.AddNodeToConnections(connectingAddress);
        }
    }
}