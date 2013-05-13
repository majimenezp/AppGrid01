using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppGridCore.AppAdministration
{
    public class AppInstance
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<AppAdditionalInstance> Instances { get; set; }
        public AppInstance()
        {
            Instances = new List<AppAdditionalInstance>();
        }
    }
}
