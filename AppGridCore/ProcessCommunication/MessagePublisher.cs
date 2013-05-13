using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;
using AppGrid.Signals;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AppGridCore.ProcessCommunication
{
    public sealed class MessagePublisher:IDisposable
    {
        private System.Net.IPAddress server;
        private int port;
        private Context context;
        private Socket publisher;
        private static readonly Lazy<MessagePublisher> lazy =
       new Lazy<MessagePublisher>(() => new MessagePublisher());
        string filter = "AGr-Serv ";
        public static MessagePublisher Instance { get { return lazy.Value; } }
        public void Init(System.Net.IPAddress server,int port)
        {
            this.server = server;
            this.port = port;
        }

        private MessagePublisher()
        {

        }
        public void Start()
        {
            context = new Context(1);
            publisher = context.Socket(SocketType.PUB);
            publisher.Bind(string.Format("tcp://{0}:{1}",server.ToString(),port.ToString()));
            Console.WriteLine(string.Format("tcp://{0}:{1}", server.ToString(), port.ToString()));
            EnviarMensajeClientes("Se inicio el proceso de publicacion");
        }
        public void ServerStop()
        {
            ServerSignal sStop = new ServerSignal();
            sStop.Signal = SignalTypes.SIGSRVSTP;
            sStop.Message = "the server will stop,all stop";
            byte[] message=SerializeSignal(sStop);
            publisher.Send(message);
        }
        public void StopInstance(int InstancePID)
        {
            ServerSignal iStop=new ServerSignal();
            iStop.Signal=SignalTypes.SIGTERM;
            iStop.Message="The server request to stop the instance";
            iStop.Content=InstancePID;
            byte[] message = SerializeSignal(iStop);
            publisher.Send(message);
        }
        private byte[] SerializeSignal(ServerSignal signal)
        {
            MemoryStream mem=new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(mem, signal);
            return mem.ToArray();
        }
        public void EnviarMensajeClientes(string mensaje)
        {
            publisher.Send(filter+mensaje, Encoding.Unicode);

        }

        public void Stop()
        {
            Dispose();
        }        

        public void Dispose()
        {
            context.Dispose();
            publisher.Dispose();
        }
    }
}
