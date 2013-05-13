using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using AppGrid;


namespace AppGridCore.AppAdministration
{
    public class AppInvoker
    {
        private string runnerPath;
        Process proc;
        ProcessStartInfo info;
        public AppInvoker(string RunnerPath)
        {
            this.runnerPath = RunnerPath;
        }

        internal AppAdditionalInstance ApplicationStart(AppStartConfig config)
        {
            AppGrid.Configuration conf;
            //AppInstance instance = new AppInstance();
            AppAdditionalInstance inst = new AppAdditionalInstance();
            proc = new Process();
            info = new ProcessStartInfo();
            info.FileName = runnerPath;
            conf=AppGrid.Utils.ConfigurationAdmin.ConfigurationLoader(Path.Combine(config.AppPath, "Configuration.xml"));
            info.Arguments = string.Format(" \"{0}\" {1}  \"{2}\" {3}", config.AppPath, config.WebPort.ToString(), config.Server,config.ComPort.ToString());
            proc.StartInfo = info;
            Console.WriteLine("Se intenta iniciar la app");
            proc.Start();
            inst.Pid = proc.Id;
            inst.Port = config.WebPort;
            inst.Server = config.Server;            
            Console.WriteLine("id del proceso:" +proc.Id.ToString());
            Console.WriteLine("Se inicio el proceso: " + conf.Name);
            return inst;
        }
    }
}
