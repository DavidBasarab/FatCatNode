namespace FatCatNode.Logic
{
    public enum NodeConnectionStatus
    {
        None,
        Connected,
        AlreadyConnected,
        CouldNotConnect,
        Removed,
        ErrorInHandShake
    }
}