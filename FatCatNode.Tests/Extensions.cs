using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatCatNode.Tests
{
    public static class Extensions
    {
        public static bool IsSet<T>(this T input, T match)
        {
            return (Convert.ToUInt32(input) & Convert.ToUInt32(match)) != 0;
        }
    }
}
