using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppGridCore
{
    public class Configuration
    {
        public String ApplicationStorePath { get; set; }
        public string Server { get; set; }
        public System.Net.IPAddress IPAdrress { get; set; }
        public int Port { get; set; }
        public int ProccessCommPort1 { get; set; }
        public int ProccessCommPort2 { get; set; }
        public int InstancesStartingPort { get; set; }
    }
}
