using CryptoTrader.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            if (Log != null)
                Log.Warn(message);
            else
                Console.WriteLine(message);

            if (Singleton.IsInitialized)
            {
                Singleton.Get<TraderHandler>().Stop();

                if (Singleton.Get<ConfigHandler>().Service.TrySave())
                    Log.Info("Successfully saved the configuration.");
                else
                    Log.Error("Failed to save the configuration!");
            }

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

            Log.Info("Shutting down.");
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        #region Properties
        private ManualResetEvent _stopEvent;
        private ILoggerService Log => Singleton.Get<LoggerHandler>();
        public bool IsStopping;
        #endregion
    }
}
