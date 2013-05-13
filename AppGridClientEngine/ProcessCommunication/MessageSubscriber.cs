using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using ZMQ;
using System.Threading;
using AppGrid.Signals;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reactive.Subjects;

namespace AppGridClientEngine.ProcessCommunication
{
    public sealed class MessageSubscriber : IDisposable
    {
        private Context context;
        private Socket subscriber;
        private string server;
        private int port;
        private string filter;
        private readonly Subject<ServerSignal> signal = new Subject<ServerSignal>();
        private static readonly Lazy<MessageSubscriber> lazy =
   new Lazy<MessageSubscriber>(() => new MessageSubscriber());
        PollItem[] items = new PollItem[1];
        public static MessageSubscriber Instance { get { return lazy.Value; } }

        public void Init(string server, int port)
        {
            this.server = server;
            this.port = port;
            //this.filter = "AGr-Serv ";
            this.filter = string.Empty;
        }

        private MessageSubscriber()
        {

        }
        public void Start()
        {
            context = new Context(1);
            subscriber = context.Socket(SocketType.SUB);
            subscriber.Connect(string.Format("tcp://{0}:{1}", server, port.ToString()));
            Console.WriteLine(string.Format("tcp://{0}:{1}", server, port.ToString()));
            subscriber.Subscribe(filter, Encoding.Unicode);
            
            items[0] = subscriber.CreatePollItem(IOMultiPlex.POLLIN);
            items[0].PollInHandler += new PollHandler(MessageSubscriber_PollInHandler);           

            Thread hilo = new Thread(StartSubscriber);
            hilo.Start(new StartParameters {  context=this.context, pollitems=this.items});          
        }

        private void StartSubscriber(object parameters)
        {
            StartParameters pollerParameters = (StartParameters)parameters;
            while (true)
            {
                pollerParameters.context.Poll(pollerParameters.pollitems, -1);
            }
        }

        void MessageSubscriber_PollInHandler(Socket socket, IOMultiPlex revents)
        {
            byte[] msgData=subscriber.Recv(10000);
            ServerSignal rcv = DeserializaSignal(msgData);
            Console.WriteLine("Message Received");
            RouteSignal(rcv);            
        }

        private void RouteSignal(ServerSignal rcv)
        {
            signal.OnNext(rcv);
        }

        private ServerSignal DeserializaSignal(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream mem=new MemoryStream(data);
            ServerSignal output=(ServerSignal)formatter.Deserialize(mem);
            return output;
        }

        public IObservable<ServerSignal> OnSignalReceived
        {
            get { return signal.AsObservable(); }
        }

        public void Stop()
        {
            Dispose();
        }
        
        public void Dispose()
        {
            
        }
    }
}
