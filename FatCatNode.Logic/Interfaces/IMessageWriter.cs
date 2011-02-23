namespace FatCatNode.Logic.Interfaces
{
    public interface IMessageWriter
    {
        void Message(string message, params object[] args);
    }
}