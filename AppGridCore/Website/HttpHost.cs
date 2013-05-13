using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Hosting.Self;
using System.Net;
using AppGridCore;

namespace AppGridCore.Website
{
    public class HttpHost
    {
        private int port;
        private string hostname;
        NancyHost host;
        public HttpHost(string hostname,int port)
        {
            this.port = port;
            this.hostname = string.Format("http://{0}:{1}/", hostname, this.port.ToString());
        }
        public void Start()
        {
            Uri[] uris=new Uri[] {new Uri(hostname),new Uri(string.Format("http://localhost:{0}",this.port.ToString())),
                new Uri(string.Format("http://127.0.0.1:{0}",this.port.ToString()))};
            host = new NancyHost(uris);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, this.port);
            host.Start();
            
        }
        public void Stop()
        {
            host.Stop();
        }
    }
}
