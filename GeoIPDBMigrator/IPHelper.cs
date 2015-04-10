using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIPDBMigrator
{
    public class IPHelper
    {
        static bool CheckIfIPInRange(string ipStart, string ipEnd, string ip)
        {
            //http://stackoverflow.com/questions/17922400/ip-range-control-on-c-sharp
            long ipRangeStart = 0; //BitConverter.ToInt32(IPAddress.Parse(ipStart).GetAddressBytes(), 0);
            long ipRangeEnd = 0;//BitConverter.ToInt32(IPAddress.Parse(ipEnd).GetAddressBytes(), 0);
            long ipAddress = 0;//BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes(), 0);

            ipRangeStart = ConvertIPToLong(ipStart);
            ipRangeEnd = ConvertIPToLong(ipEnd);
            ipAddress = ConvertIPToLong(ip);

            if (ipAddress >= ipRangeStart && ipAddress <= ipRangeEnd)
            {
                return true;
            }

            return false;
        }

        public static long ConvertIPToLong(string ip)
        {
            string[] ipBytes;
            double num = 0;
            if (!string.IsNullOrEmpty(ip))
            {
                ipBytes = ip.Split('.');
                for (int i = ipBytes.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(ipBytes[i]) % 256) * Math.Pow(256, (3 - i)));
                }
            }
            return (long)num;
        }

        public static string ConvertLongToIP(long longIP)
        {
            string ip = string.Empty;

            for (int i = 0; i < 4; i++)
            {
                int num = (int)(longIP / Math.Pow(256, (3 - i)));

                longIP = longIP - (long)(num * Math.Pow(256, (3 - i)));

                if (i == 0)
                    ip = num.ToString();
                else
                    ip = ip + "." + num.ToString();
            }

            return ip;
        }
    }
}
