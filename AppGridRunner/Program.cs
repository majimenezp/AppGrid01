using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace AppGridRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            AppGridCore.Configuration conf = new AppGridCore.Configuration();

            conf.ApplicationStorePath = Properties.Settings.Default.AppDomainPath;
            conf.Port = 5500;
            conf.Server = "cot-cdc-st1813";
            conf.IPAdrress = System.Net.IPAddress.Parse("127.0.0.1");
            conf.ProccessCommPort1 = 5502;
            conf.ProccessCommPort2 = 5503;
            conf.InstancesStartingPort = 35300;
            AppGridCore.AppGridStart starter = new AppGridCore.AppGridStart(conf);
            starter.Start();      

            Console.WriteLine("Press a key to close...");
            Console.ReadKey();
            starter.Stop();
        }
    }
}
