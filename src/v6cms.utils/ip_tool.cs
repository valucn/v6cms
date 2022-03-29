using System;
using System.Net;

namespace v6cms.utils
{
    public static class ip_tool
    {
        public static bool IpAddressInRange(this string input, string begin, string ends)
        {
            uint num = IPToID(input);
            return ((num >= IPToID(begin)) && (num <= IPToID(ends)));
        }

        private static uint IPToID(string addr)
        {
            IPAddress address;
            if (!IPAddress.TryParse(addr, out address))
            {
                return 0;
            }
            byte[] addressBytes = address.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(addressBytes);
            }
            return BitConverter.ToUInt32(addressBytes, 0);
        }
    }
}
