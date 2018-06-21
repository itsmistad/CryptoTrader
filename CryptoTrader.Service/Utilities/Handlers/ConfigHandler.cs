using System;
using System.IO;
using CryptoTrader.Service.Services.Configuration;
using CryptoTrader.Service.Services.Logging;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class ConfigHandler
    {
        public ConfigHandler()
        {
            var args = Singleton.Get<ArgumentsHandler>();
            
            if (args.Has("config"))
            {
                switch (args["config"].ToLower())
                {
                    //case "db":
                    //    throw new NotImplementedException();
                    case "json":
                        Service = new JsonConfigService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
                        Service.TryLoad();
                        Log.Info("Successfully loaded configuration.");
                        break;
                    default:
                        Program.Stop($"Invalid configuration flag: {args["config"]}");
                        break;
                }
            }
            else Program.Stop("No configuration defined. Did you set the config flag? ex: -config json");
        }

        #region Properties
        private static ILoggerService Log => Singleton.Get<LoggerHandler>();
        public IConfigService Service { get; }
        #endregion
    }
}
