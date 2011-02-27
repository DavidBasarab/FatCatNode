using System;

namespace FatCatNode.Logic.Interfaces
{
    public interface ITimeHelper
    {
        void Sleep(TimeSpan timeToSleep);

        void Sleep(int milliseconds);
    }
}