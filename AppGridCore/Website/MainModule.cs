using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using AppGridCore.ProcessCommunication;

namespace AppGridCore.Website
{
    public class MainModule:NancyModule
    {
        public MainModule()
        {
            Get["/"] = x => {
                return View["Index.cshtml"];
            };
            Get["/Test"]=x=>View["Test.cshtml"];
            Get["/Test/{command}"] = x => {
                //MessagePublisher.Instance.EnviarMensajeClientes("Stop");
                MessagePublisher.Instance.ServerStop();
                var respuesta = new {mensaje="Detenidos" };
                return Response.AsJson(respuesta);
            };
            Get["/css/{arch}"] = x =>
            {
                return Response.AsCss("assets/css/" + (string)x.arch);
            };
            Get["/img/{arch}"] = x =>
            {
                return Response.AsImage("assets/img/" + (string)x.arch);
            };
            Get["/js/{arch}"] = x =>
            {
                return Response.AsJs("assets/Js/" + (string)x.arch);
            };
        }        
    }
}
