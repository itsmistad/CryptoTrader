using CryptoTrader.Service.Services;
using CryptoTrader.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class LoggerHandler : ILoggerService
    {
        private ILoggerService _service;

        public LoggerHandler()
        {
            var args = Singleton.Get<ArgumentsHandler>();
            if (args.Has("logger"))
            {
                switch (args["logger"].ToLower())
                {
                    case "local":
                        _service = new FileLoggerService();
                        break;
                    default:
                        Program.Stop("Invalid configuration flag: {0}", args["config"]);
                        break;

                }
            }
            else Program.Stop("No logger defined. Did you set the logger flag? ex: -logger local");
        }

        public void Log(string tag, string format, params object[] args) => _service?.Log(tag, format, args);

        public void Debug(string format, params object[] args) => _service?.Debug(format, args);

        public void Error(string format, params object[] args) => _service?.Error(format, args);

        public void Info(string format, params object[] args) => _service?.Info(format, args);

        public void Warn(string format, params object[] args) => _service?.Warn(format, args);
    }
}
