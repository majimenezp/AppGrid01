using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using AppGridCore.ProcessCommunication;
using AppGridCore.Models;
using AppGridCore.ProcessManager;
using AppGridCore.AppAdministration;
namespace AppGridCore.Website
{
    public class AppsModule : NancyModule
    {
        public AppsModule()
            : base("/Apps")
        {
            Get["/"] = x =>
                {
                    return View["Apps.cshtml"];
                };
            Get["/List"] = x =>
            {
                var info = (from app in ProcessManager.ProcManager.Instance.Apps
                            select new
                            {
                                app.Name,
                                Status = ProcessManager.ProcManager.Instance.AppInstances.Exists(a => a.Name == app.Name) ? "Running" : "Stopped",
                                NumProc = ProcessManager.ProcManager.Instance.AppInstances.FirstOrDefault(a => a.Name == app.Name).Instances.Count()
                            }).ToArray();
                return Response.AsJson(info);
            };
            Get["/App/{appname}"] = x =>
                {
                    AppModel model = new AppModel();
                    AppInstance inst;
                    AppAdministration.App app = ProcManager.Instance.Apps.FirstOrDefault(a => a.Name == x.appname);
                    inst = ProcManager.Instance.AppInstances.FirstOrDefault(b => b.Name == x.appname);
                    model.Name = app.Name;
                    model.Path = app.Path;
                    model.Instances.AddRange(inst.Instances);
                    model.NumberofInstances = model.Instances.Count;
                    return View["AppControl.cshtml", model];
                };
            Get["/App/AppsInstance/{appname}/{numberInstances}"] = x =>
                {
                    AppInstance app = ProcManager.Instance.AppInstances.Where(a => a.Name == x.appname).FirstOrDefault();
                    if (app != null)
                    {
                        ProcManager.Instance.ChangeNumberOfInstances(x.appname, x.numberInstances);
                        return Response.AsJson(new { changed = true, Instances = app.Instances });
                    }
                    else
                    {
                        return Response.AsJson(new { changed = false });
                    }
                };
            Get["/NewApp/"] = x =>
                {
                    return View["NewApp.cshtml"];
                };

            Post["/CreateNewApp/"] = x =>
                {
                    HttpFile file = this.Request.Files.FirstOrDefault();
                    ProcessNewAPP(file,this.Request.Form.defhttpport, this.Request.Form.portOption,this.Request.Form.appname);
                    return Response.AsJson("Uploaded");
                };
            Get["/CheckAppName/{appname}/"] = x =>
            {
                if (ProcManager.Instance.Apps.Exists(a => a.Name.ToLower() == x.appname))
                {
                    return Response.AsJson(new { message = "This appname already exist,please select another one.", cssclass = "error",exist=true });
                }
                else
                {
                    return Response.AsJson(new { message = "This appname is safe.", cssclass = "success",exist=false });
                }
            };
        }

        private void ProcessNewAPP(HttpFile file, int port, string portOption, string appname)
        {
            PackageManager manager;
            AppGrid.Configuration conf = new AppGrid.Configuration();
            if(file.Name.EndsWith("zip"))
            {
                manager=new PackageManager();
                conf.Name = appname;
                switch (portOption.ToLower())
                {
                    case "generallist":
                        conf.HttpPortConfig = AppGrid.HttpPortConfigurationOptions.NextGeneralOpenPort;
                        break;
                    case "incremental":
                        conf.HttpPortConfig = AppGrid.HttpPortConfigurationOptions.IncrementallyFromAssigned;
                        break;
                    default:
                        conf.HttpPortConfig = AppGrid.HttpPortConfigurationOptions.NextGeneralOpenPort;
                        break;
                }                
                conf.HttpPort = port;
                manager.ProcessNewApp(file.Value,appname,conf);                
            }
        }
    }
}
