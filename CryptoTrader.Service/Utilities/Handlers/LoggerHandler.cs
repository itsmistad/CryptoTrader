using System.Collections.Generic;
using System.Xml.Serialization;
using CryptoTrader.Service.Services.Logging;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class LoggerHandler : ILoggerService
    {
        public LoggerHandler()
        {
            Loggers = new List<ILoggerService>();

            var args = Singleton.Get<ArgumentsHandler>();
            if (args.Has("logger"))
            {
                Loggers.Add(new ConsoleLoggerService());

                switch (args["logger"].ToLower())
                {
                    case "local":
                        Loggers.Add(new FileLoggerService());
                        break;
                    default:
                        Program.Stop($"Invalid logger flag: {args["logger"]}");
                        break;
                }
            }
            else Program.Stop("No logger defined. Did you set the logger flag? ex: -logger local");
        }

        public void Log(string tag, string format, params object[] args)
        {
            if (tag.Length > 5) tag = tag.Substring(0, 5);
            else if (tag.Length < 5) tag += " ";

            foreach (var logger in Loggers)
                logger.Log(tag, format, args);
        }

        public void Debug(string format, params object[] args)
        {
            foreach (var logger in Loggers)
                logger.Debug(format, args);
        }

        public void Error(string format, params object[] args)
        {
            foreach (var logger in Loggers)
                logger.Error(format, args);
        }

        public void Info(string format, params object[] args)
        {
            foreach (var logger in Loggers)
                logger.Info(format, args);
        }

        public void Warn(string format, params object[] args)
        {
            foreach (var logger in Loggers)
                logger.Warn(format, args);
        }

        #region Properties
        public List<ILoggerService> Loggers { get; }
        #endregion
    }
}
