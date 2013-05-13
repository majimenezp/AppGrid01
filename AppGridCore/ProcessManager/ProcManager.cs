using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppGridCore.AppAdministration;
using AppGridCore.ProcessCommunication;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace AppGridCore.ProcessManager
{
    public sealed class ProcManager
    {
        private readonly Subject<InstancesRequest> instanceRequest = new Subject<InstancesRequest>();
        private static ProcManager instance = null;
        private static readonly object padlock = new object();
        private List<AppInstance> appInstances;
        private List<App> apps;

        ProcManager()
        {
            appInstances = new List<AppInstance>();
            apps = new List<App>();
        }
        public string AppDomainPath
        {
            get;
            set;
        }

        public static ProcManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ProcManager();
                    }
                    return instance;
                }
            }
        }

        public IObservable<InstancesRequest> OnInstanceRequest
        {
            get { return instanceRequest.AsObservable(); }
        }

        public void ChangeNumberOfInstances(string AppName, int NumberofInstances)
        {
            int cantInst;
            InstancesRequest parameter = new InstancesRequest();
            AppInstance app = appInstances.FirstOrDefault(x => x.Name == AppName);
            if (app != null)
            {
                cantInst = app.Instances.Count;
                while (cantInst != NumberofInstances)
                {
                    parameter.AppName = AppName;
                    parameter.NumberofInstances = NumberofInstances;
                    if (cantInst < NumberofInstances)
                    {
                        instanceRequest.OnNext(parameter);
                        cantInst = app.Instances.Count;
                    }
                    else
                    {
                        int pid = app.Instances.Last().Pid;
                        MessagePublisher.Instance.StopInstance(pid);
                        app.Instances.Remove(app.Instances.Last());
                        cantInst = app.Instances.Count;
                    }
                    
                }               
                //parameter.AppName = AppName;
                //parameter.NumberofInstances = NumberofInstances;
                //instanceRequest.OnNext(parameter);
            }
        }
        
        public List<AppInstance> AppInstances
        {
            get { return appInstances; }
            set { appInstances = value; }
        }

        public List<App> Apps
        {
            get { return apps; }
            set { apps = value; }
        }
    }
}
