using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace AppGridCore
{
    public class AppGridStart
    {
        private Configuration conf;
        AppGridHost host;
        public AppGridStart(Configuration configuration)
        {
            conf = configuration;
        }

        public void Start()
        {
            host = new AppGridHost(conf);
            host.Start();
            AppGridCore.ProcessCommunication.MessagePublisher.Instance.EnviarMensajeClientes("Starting Appgrid Server:" + DateTime.Now.ToString());

            //var ep = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 8889);
            //Gate.Kayak.KayakGate.Start(new SchedulerDelegate(), ep, AppGridCore.Website.Startup.Configuration);

        }

        public void Stop()
        {
            host.Stop();
        }
    }
}
