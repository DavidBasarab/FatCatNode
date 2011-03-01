using System;
using System.Threading;
using FatCatNode.Logic.Interfaces;

namespace FatCatNode.Logic.Helpers
{
    public class TimeHelper : ITimeHelper
    {
        private static ITimeHelper _overridenHelper;

        public static ITimeHelper Helper
        {
            get { return _overridenHelper ?? Nested.Instance; }
            set { _overridenHelper = value; }
        }

        public void Sleep(TimeSpan timeToSleep)
        {
            Thread.Sleep(timeToSleep);
        }

        public void Sleep(int milliseconds)
        {
            Sleep(TimeSpan.FromMilliseconds(milliseconds));
        }

        private class Nested
        {
            internal static readonly TimeHelper Instance = new TimeHelper();
        }
    }
}