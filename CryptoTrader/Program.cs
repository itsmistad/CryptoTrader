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

        private static ManualResetEvent _stopEvent;

        private static void Initialize()
        {
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
                else Stop("Failed to load a config. [{0}]", config.FileName);
        }

        public static void Stop(string reason, params object[] args)
        {
            Logger.Warn("Gently stopping. Reason: " + string.Format(reason, args));

            #region Gently stop everything.
            foreach (var config in Configs)
                if (config.TrySave())
                    Logger.Info("Successfully saved a config file. [{0}]", config.FileName);
            #endregion

            _stopEvent.Set();
        }

        private static void Main(string[] args)
        {
            Initialize();
            Logger.Info("Ready. To shutdown, press CTRL+C.");

            // Start a TraderService instance. For now, BinanceTraderService.

            _stopEvent.WaitOne();

            Logger.Info("Shutting down.");

            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }
    }
}
