using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Tester
{
    public static class Extensions
    {
        public static string FindIPAddress(this IPHostEntry entry)
        {
            if (entry == null)
            {
                return string.Empty;
            }

            return entry.AddressList
                .Where(i => i.AddressFamily == AddressFamily.InterNetwork)
                .FirstOrDefault()
                .ToString();
        }
    }
}