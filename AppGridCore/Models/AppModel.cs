using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppGridCore.AppAdministration;

namespace AppGridCore.Models
{
    public class AppModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int NumberofInstances { get; set; }
        public List<AppAdditionalInstance> Instances { get; set; }
        public AppModel()
        {
            Instances = new List<AppAdditionalInstance>();
        }
    }
}
