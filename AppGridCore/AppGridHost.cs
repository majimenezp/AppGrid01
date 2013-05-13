using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Policy;
using System.Reflection;
using System.IO;
using AppGridCore.AppAdministration;
using AppGridCore.Website;
using System.Threading;
namespace AppGridCore
{
    public class AppGridHost
    {
        private Configuration configuration;
        
        
        private string CurrentPath;
        public const string confFileName = "Configuration.xml";
        int CurrenPort;
        HttpHost host;
        public AppGridHost(Configuration config)
        {
            this.configuration = config;
            this.CurrentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (configuration.IPAdrress != null)
            {
                var ips = System.Net.Dns.GetHostAddresses(configuration.Server).Where(x => !x.IsIPv6LinkLocal && !x.IsIPv6Multicast && !x.IsIPv6SiteLocal && !x.IsIPv6Teredo);
                this.configuration.IPAdrress = ips.FirstOrDefault();
            }           
        }
        public void Start()
        {
            string hostname=string.Format("http://{0}:{1}/", configuration.Server, configuration.Port.ToString());            
            Console.WriteLine("Web server host in:" + hostname);   
            AppGridCore.ProcessCommunication.MessagePublisher.Instance.Init(configuration.IPAdrress, configuration.ProccessCommPort1);
            AppGridCore.ProcessCommunication.MessagePublisher.Instance.Start();
            
            host = new HttpHost(configuration.Server, configuration.Port);
            host.Start();
            InitializeApps();
            ProcessManager.ProcManager.Instance.OnInstanceRequest.Subscribe(x =>
            {
                StartNewInstance(x.AppName, configuration.Server);
            });           
        }

        private void InitializeApps()
        {
            AppGridCore.AppAdministration.AppInvoker invoker;
            AppInstance tmpinstance;
            AppAdditionalInstance instance;
            Console.WriteLine("current path:" + this.configuration.ApplicationStorePath);
            ProcessManager.ProcManager.Instance.AppDomainPath = this.configuration.ApplicationStorePath;
            DirectoryInfo dirinfo = new DirectoryInfo(this.configuration.ApplicationStorePath);
            CurrenPort = configuration.InstancesStartingPort;
            Console.WriteLine("Cantidad de directorios:" + dirinfo.EnumerateDirectories().Count().ToString());
            foreach (DirectoryInfo dir in dirinfo.EnumerateDirectories())
            {
                Console.WriteLine("se verifico el directorio:" + dir.Name);
                Console.WriteLine("Config:" + (dir.EnumerateFiles(confFileName).Count().ToString()));
                if (dir.EnumerateFiles(confFileName).Count() > 0)
                {
                    Console.WriteLine("Entro");
                    Console.WriteLine(CurrentPath);
                    invoker = new AppAdministration.AppInvoker(Path.Combine(CurrentPath,"AppGridClientEngine.exe"));
                    try
                    {
                        AppStartConfig conf = new AppStartConfig();
                        conf.AppPath = dir.FullName;
                        conf.WebPort = CurrenPort;
                        conf.Server = configuration.Server;
                        conf.ComPort = configuration.ProccessCommPort1;
                        tmpinstance = new AppInstance();                        
                        instance = invoker.ApplicationStart(conf);
                        tmpinstance.Name = dir.Name;
                        tmpinstance.Path=conf.AppPath;
                        tmpinstance.Instances.Add(instance);
                        ProcessManager.ProcManager.Instance.AppInstances.Add(tmpinstance);
                        ProcessManager.ProcManager.Instance.Apps.Add(new App {  Name=dir.Name,Path=dir.FullName});
                        ++CurrenPort;
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.Message+"|"+ ex1.StackTrace);
                    }
                }
            }
        }

        public void StartNewInstance(string AppName,string server)
        {
            AppGridCore.AppAdministration.AppInvoker invoker;
            AppInstance instance = ProcessManager.ProcManager.Instance.AppInstances.FirstOrDefault(x => x.Name == AppName);
            AppAdditionalInstance otherInstance;
            if(instance!=null)
            {
                otherInstance = new AppAdditionalInstance();
                invoker = new AppAdministration.AppInvoker(Path.Combine(CurrentPath, "AppGridClientEngine.exe"));
                AppStartConfig conf = new AppStartConfig();
                conf.AppPath = instance.Path;
                conf.WebPort = CurrenPort;
                conf.Server = configuration.Server;
                conf.ComPort = configuration.ProccessCommPort1;
                otherInstance = invoker.ApplicationStart(conf);
                instance.Instances.Add(otherInstance);
                ++CurrenPort;
            }
        }

        public void Stop()
        {
            AppGridCore.ProcessCommunication.MessagePublisher.Instance.ServerStop();
            AppGridCore.ProcessCommunication.MessagePublisher.Instance.Stop();
            host.Stop();
        }
    }
}
