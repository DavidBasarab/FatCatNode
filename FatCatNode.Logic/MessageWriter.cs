using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic
{
    public class MessageWriter : IMessageWriter
    {
        private static IMessageWriter _overridenWriter;

        public static IMessageWriter Writer
        {
            get { return _overridenWriter ?? Nested.Instance; }
            set { _overridenWriter = value; }
        }

        public void Message(string message, params object[] args)
        {
            // Do nothing, if nobody sets this then no messages are written
        }

        private class Nested
        {
            internal static readonly MessageWriter Instance = new MessageWriter();
        }
    }
}