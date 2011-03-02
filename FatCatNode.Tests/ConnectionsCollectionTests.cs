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

        private static void ResetMessageWriter()
        {
            MessageWriter.Writer = null;
        }

        private void CreateMockRepository()
        {
            Mocks = new MockRepository();
        }

        public IPAddress ConnectionAddress
        {
            get { return IPAddress.Parse("237.237.237.237"); }
        }

        [Test]
        public void ANodeHasConnectedByAddressBeforeNodeConnectedMessageIsWritten()
        {
            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(ConnectionAddress)).Return(null);

            var messageWriter = Mocks.DynamicMock<IMessageWriter>();

            messageWriter.Expect(v => v.Message("Attempting to connect to node at address {0}.", ConnectionAddress));

            Mocks.ReplayAll();

            MessageWriter.Writer = messageWriter;

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnections.Connections.AddNodeToConnections(ConnectionAddress);
        }

        [Test]
        public void ANodeHasConnectedByAddressTheNodeConnectionAttemptThrowsExceptionCouldNotConnectStatusReturned()
        {
            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(ConnectionAddress)).Throw(new Exception());

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnectionStatus result = NodeConnections.Connections.AddNodeToConnections(ConnectionAddress);

            Assert.That(result, Is.EqualTo(NodeConnectionStatus.ErrorInHandShake));
        }

        [Test]
        public void ANodeHasConnectedByAddressTheNodeIsConnected()
        {
            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(ConnectionAddress)).Return(null);

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnections.Connections.AddNodeToConnections(ConnectionAddress);
        }

        [Test]
        public void ANoneEmptyNodeIdIsReturnedNodeConnectionStatusIsConnected()
        {
            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            var otherNode = Mocks.DynamicMock<INode>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(ConnectionAddress)).Return(otherNode);

            otherNode.Expect(v => v.Handshake("Node2")).Return("OtherNode");

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnections.Connections.SetNodeId("Node2");

            NodeConnectionStatus connectionStatus = NodeConnections.Connections.AddNodeToConnections(ConnectionAddress);

            Assert.That(connectionStatus, Is.EqualTo(NodeConnectionStatus.Connected));
        }

        [Test]
        public void AnEmptyNodeIdIsReturnedNodeConnectionStatusAsNotConnected()
        {
            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            var otherNode = Mocks.DynamicMock<INode>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(ConnectionAddress)).Return(otherNode);

            otherNode.Expect(v => v.Handshake("Node2")).Return(null);

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnections.Connections.SetNodeId("Node2");

            NodeConnectionStatus connectionStatus = NodeConnections.Connections.AddNodeToConnections(ConnectionAddress);

            Assert.That(connectionStatus, Is.EqualTo(NodeConnectionStatus.CouldNotConnect));
        }

        [Test]
        public void AnErrorOccuredOnConnectionThenAMessageIsWritten()
        {
            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(ConnectionAddress)).Throw(new Exception());

            var messageWriter = Mocks.DynamicMock<IMessageWriter>();

            messageWriter.Expect(v => v.Message("Error connecting to node at address {0}.", ConnectionAddress));

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;
            MessageWriter.Writer = messageWriter;

            NodeConnections.Connections.AddNodeToConnections(ConnectionAddress);
        }

        [Test]
        public void AnErrorOnNodeHandShakeWillResultInNodeConnectionStatusSetToError()
        {
            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            var otherNode = Mocks.DynamicMock<INode>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(ConnectionAddress)).Return(otherNode);

            otherNode.Expect(v => v.Handshake("Node2")).Throw(new Exception());

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnections.Connections.SetNodeId("Node2");

            NodeConnectionStatus connectionStatus = NodeConnections.Connections.AddNodeToConnections(ConnectionAddress);

            Assert.That(connectionStatus, Is.EqualTo(NodeConnectionStatus.ErrorInHandShake));
        }

        [Test]
        public void UsingNodeConnectionHandshakeIsCalledNodeIdIsReturned()
        {
            var remoteNodeConnectionHelper = Mocks.DynamicMock<IRemoteNodeConnectionHelper>();

            var otherNode = Mocks.DynamicMock<INode>();

            remoteNodeConnectionHelper.Expect(v => v.OpenRemoteConnection(ConnectionAddress)).Return(otherNode);

            otherNode.Expect(v => v.Handshake("Node2"));

            Mocks.ReplayAll();

            NodeConnections.Connections.RemoteHelper = remoteNodeConnectionHelper;

            NodeConnections.Connections.SetNodeId("Node2");

            NodeConnections.Connections.AddNodeToConnections(ConnectionAddress);
        }
    }
}