using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;
using System.Threading;
using AppGridClientEngine.ProcessCommunication;
using System.Diagnostics;

namespace AppGridClientEngine
{
    class Program
    {
        private static Context ctx;
        private static bool Status;
        private static int Pid;
        private static AppGridCore.AppLoad.ApplicationLoad appload;
        static void Main(string[] args)
        {
            string appPath = args[0];
            string server = "127.0.0.1";
            int webport, comport;
            Pid = Process.GetCurrentProcess().Id;
            if (!int.TryParse(args[1], out webport))
            {
                Console.WriteLine("Error:web port not provided");
                return;
            }
            if (args.Length > 2 && !string.IsNullOrEmpty(args[2]))
            {
                server = args[2];
            }
            else
            {
                Console.WriteLine("Error:server name not provided");
                return;
            }
            if (!int.TryParse(args[3], out comport))
            {
                Console.WriteLine("Error:communication port not provided");
                return;
            }
            Console.WriteLine("Starting...");
            MessageSubscriber.Instance.Init(server, comport);
            Console.WriteLine("Running on port:" + webport.ToString());
            MessageSubscriber.Instance.Start();
            StartApp(appPath, webport, server);
            Status = true;
            SubscribersInit();
            Console.ReadKey();
            StopApp();
        }

        private static void SubscribersInit()
        {
            MessageSubscriber.Instance.OnSignalReceived.Subscribe(x =>
            {
                Console.WriteLine("Signal received:" + x.Message);
                switch (x.Signal)
                {
                    case AppGrid.Signals.SignalTypes.SIGSRVSTP:
                        StopApp();                        
                        System.Environment.Exit(1);
                        break;
                    case AppGrid.Signals.SignalTypes.SIGTERM:
                        if (((int)x.Content) == Pid)
                        {
                            StopApp();
                            System.Environment.Exit(1);
                        }
                        break;
                }
            });
        }

        private static void StopApp()
        {
            MessageSubscriber.Instance.Stop();
            appload.Stop();
            Status = false;
        }
        private static void PauseApp()
        {
            if (Status)
            {
                 //TODO: check how can i save a snapshot of the appdomain or the app's status
            }
        }

        private static void StartApp(string appPath, int port, string server)
        {
            appload = new AppGridCore.AppLoad.ApplicationLoad(appPath);
            appload.Load();
            appload.Start(server, port);
        }
    }
}
