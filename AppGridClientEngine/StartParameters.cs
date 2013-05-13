using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace AppGridClientEngine
{
    public class StartParameters
    {
        public Context context { get; set; }
        public PollItem[] pollitems { get; set; }
    }
}
