using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMLocker.Shared
{
    public class SystemNetwork
    {
        public string NetworkInterfaceType { get; set; }
        public string NetworkOperationalStatus { get; set; }

        public string IP { get; set; }
        public string NetMask { get; set; }
        public string Gateway { get; set; }
    }

    public class SystemSSID
    {
        public string SSID { get; set; }
        public string Pass { get; set; }
    }
}
