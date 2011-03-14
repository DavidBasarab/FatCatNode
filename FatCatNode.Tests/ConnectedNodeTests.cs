using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FatCatNode.Logic;
using NUnit.Framework;

namespace FatCatNode.Tests
{
    [TestFixture]
    public class ConnectedNodeTests
    {
        [Test]
        public void AConnectedNodeHasANodeId()
        {
            var node = new ConnectedNode("node1");

            Assert.That(node.NodeId, Is.EqualTo("node1"));
        }
    }
}
