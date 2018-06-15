using CryptoTrader.Service.Services;
using CryptoTrader.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class ConfigHandler
    {
        private IConfigService _service;

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
                        _service = new JSONConfigService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
                        _service.TryLoad();
                        Log.Info("Successfully loaded configuration.");
                        break;
                    default:
                        Program.Stop("Invalid configuration flag: {0}", args["config"]);
                        break;
                }
            }
            else Program.Stop("No configuration defined. Did you set the config flag? ex: -config json");
        }

        #region Properties
        private ILoggerService Log => Singleton.Get<LoggerHandler>();
        public IConfigService Service => _service;
        #endregion
    }
}
