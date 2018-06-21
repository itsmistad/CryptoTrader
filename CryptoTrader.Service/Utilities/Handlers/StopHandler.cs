using System.Threading;
using CryptoTrader.Service.Services.Logging;
using Console = System.Console;

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
                Log.Warn(message);
                Log.Warn("\t~-~-~-~-~ STOPPING ~-~-~-~-~");

                Singleton.Get<TraderHandler>().Stop();

                if (Singleton.Get<ConfigHandler>().Service.TrySave())
                    Log.Info("Successfully saved the configuration.");
                else
                    Log.Error("Failed to save the configuration!");

                Log.Info("Successfully shut down.");
            }
            else Console.WriteLine(message);

            _stopEvent.Set();
        }

        public void WaitForStop()
        {
            Program.Title = "Use CTRL+C to shutdown.";
            Console.CancelKeyPress += (s, a) =>
            {
                Stop("CTRL+C.");
                a.Cancel = true;
            };

            _stopEvent.WaitOne();

            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        #region Properties
        private readonly ManualResetEvent _stopEvent;
        private static ILoggerService Log => Singleton.Get<LoggerHandler>();
        public bool IsStopping;
        #endregion
    }
}
