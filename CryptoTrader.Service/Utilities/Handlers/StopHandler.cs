using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using CryptoTrader.Service.Services.Logging;
using Console = Colorful.Console;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class StopHandler
    {
        public StopHandler()
        {
            _stopEvent = new ManualResetEvent(false);
        }

        public void Stop(string reason, params object[] args)
        {
            var message = string.Format(reason, args);
            Program.Title = message;
            IsStopping = true;

            if (Singleton.IsInitialized)
            {
                Log.Error("\t~-~-~-~-~ STOPPING ~-~-~-~-~");
                Log.Error("\tReason: " + message);

                Singleton.Get<TraderHandler>().Stop();

                if (Singleton.Get<ConfigHandler>().Service.TrySave())
                    Log.Info("Successfully saved the configuration.");
                else
                    Log.Error("Failed to save the configuration!");

                Log.Info("Successfully shut down.");
            }
            else Console.WriteLine(message, Color.DarkGray);

            _stopEvent.Set();
        }

        public void WaitForStop()
        {
            Program.Title = "Use CTRL+C to shutdown.";
            _handler += Handler;
            SetConsoleCtrlHandler(_handler, true);

            _stopEvent.WaitOne();

            Console.WriteLine("Press any key to continue...", Color.DarkGray);
            Console.ReadKey();
        }

        #region Override Handler
        [DllImport("kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);
        private EventHandler _handler;

        private enum CtrlType
        {
            CtrlCEvent = 0,
            CtrlBreakEvent = 1,
            CtrlCloseEvent = 2,
            CtrlLogoffEvent = 5,
            CtrlShutdownEvent = 6
        }

        private bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CtrlCEvent:
                    Stop("CTRL+C.");
                    break;
                case CtrlType.CtrlBreakEvent:
                    Stop("User broke main thread.");
                    break;
                case CtrlType.CtrlCloseEvent:
                    Stop("User closed window.");
                    break;
                case CtrlType.CtrlLogoffEvent:
                    Stop("User is logging off.");
                    break;
                case CtrlType.CtrlShutdownEvent:
                    Stop("System is shutting down.");
                    break;
            }

            return true;
        }
        #endregion

        #region Properties
        private readonly ManualResetEvent _stopEvent;
        private static ILoggerService Log => Singleton.Get<LoggerHandler>();
        public bool IsStopping;
        #endregion
    }
}
