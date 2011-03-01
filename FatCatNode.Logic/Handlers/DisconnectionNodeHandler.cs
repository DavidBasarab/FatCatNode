using FatCatNode.Logic.Arguments;

namespace FatCatNode.Logic.Handlers
{
    public class DisconnectionNodeHandler
    {
        public void DisconnectNode(object sender, NodeAnnoucementEventArgs e)
        {
            string disconnectingNodeId = NodeConnections.Connections.FindNodeIdByAddress(e.Address);

            NodeConnections.Connections.RemoveNodeFromConnections(e.Address);

            MessageWriter.Writer.Message("Node Id {0} at address {1} has disconnected.", disconnectingNodeId, e.Address);
        }
    }
}