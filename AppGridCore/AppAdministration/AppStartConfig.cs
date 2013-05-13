using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppGridCore.AppAdministration
{
    public class AppStartConfig
    {
        public string AppPath { get; set; }
        public int WebPort { get; set; }
        public int ComPort { get; set; }
        public string Server { get; set; }
    }
}
