using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
namespace AppGridCore.AppLoad
{
    internal class ApplicationLoad
    {
        private string appPath;
        private string[] validExtensions = new string[] { ".exe", ".dll" };
        private Type maintype;
        AppGrid.IAppGridApp appInstance;
        AppGrid.Configuration conf;
        public ApplicationLoad(string appPath)
        {
            // TODO: Complete member initialization
            this.appPath = appPath;
        }

        public bool Load()
        {
            Assembly loadedAsm;
            Type[] types;
            
            string[] files = Directory.GetFiles(appPath);
            foreach (string file in files.Where(x=>validExtensions.Contains(Path.GetExtension(x))))
            {
                // The filter for appgridbase it's needed because gives a duplicate library error on invoking
                if (validExtensions.Contains(Path.GetExtension(file)) && !Path.GetFileName(file).StartsWith("AppGridBase"))
                {
                    loadedAsm = Assembly.LoadFrom(file);
                    types = loadedAsm.GetTypes();
                    maintype = (from tipo in types
                               where tipo.GetInterfaces().Any(x => x.FullName == typeof(AppGrid.IAppGridApp).FullName)
                               && tipo.GetConstructor(Type.EmptyTypes) != null
                               select tipo).FirstOrDefault();
                    if (maintype!=null)
                    {
                        appInstance = (AppGrid.IAppGridApp)Activator.CreateInstance(maintype);
                        break;
                    }
                }
            }
            return true;
        }

        public bool Start(string server,int port)
        {
            // TODO: Add some diagnostic and starting validations for errors control
            conf=AppGrid.Utils.ConfigurationAdmin.ConfigurationLoader(Path.Combine(appPath,"Configuration.xml"));
            conf.HttpPort = port;
            conf.Host = server;
            appInstance.Init(conf);
            appInstance.Start();
            return true;
        }

        public bool Stop()
        {
            appInstance.Stop();
            return true;
        }

    }
}
