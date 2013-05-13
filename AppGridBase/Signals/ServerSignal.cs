using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppGrid.Signals
{
    [Serializable]
    public class ServerSignal
    {
        public SignalTypes Signal { get; set; }
        public string Message { get; set; }
        public object Content { get; set; }
    }
}
