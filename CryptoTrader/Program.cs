using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BinanceTrader.Services;

namespace BinanceTrader
{
    public class Program
    {
        public static FileLoggerService Logger;
        public static JSONConfigService Config;
        public static List<JSONConfigService> Configs;

        private static bool _isStopping;
        private static ManualResetEvent _stopEvent;

        public static void Stop(string reason, params object[] args)
        {
            IsStopping = true;

            var message = "Gently stopping. Reason: " + string.Format(reason, args);
            Title = message;
            Logger.Warn(message);

            #region Gently stop everything.
            foreach (var config in Configs)
                if (config.TrySave())
                    Logger.Info("Successfully saved a config file. [{0}]", config.FileName);
            #endregion

            _stopEvent.Set();
        }

        private static void Initialize()
        {
            Title = "Initializing...";
            Logger = new FileLoggerService("Logs");
            _stopEvent = new ManualResetEvent(false);
            Configs = new List<JSONConfigService>
            {
                (Config = new JSONConfigService("Configs/config.json"))
            };

            Console.CancelKeyPress += (s, a) =>
            {
                Stop("CTRL+C.");
                a.Cancel = true;
            };

            foreach (var config in Configs)
                if (config.TryLoad())
                    Logger.Info("Successfully loaded a config file. [{0}]", config.FileName);
                else
                {
                    Stop("Failed to load a config. [{0}]", config.FileName);
                    break;
                }

            Title = IsStopping ? "" : "Ready. Use CTRL+C to shutdown.";
        }

        private static void HandleStop()
        {
            _stopEvent.WaitOne();

            Logger.Info("Shutting down.");

            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

        private static void Main(string[] args)
        {
            Initialize();

            // Start a TraderService instance. For now, BinanceTraderService.

            HandleStop();
        }

        #region Properties
        public static string Title
        {
            get
            {
                return Console.Title;
            }
            set
            {
                Console.Title = "CrpytoTrader by D. | " +  value;
            }
        }

        public static bool IsStopping
        {
            get
            {
                return _isStopping;
            }
            private set
            {
                _isStopping = value;
            }
        }
        #endregion
    }
}
