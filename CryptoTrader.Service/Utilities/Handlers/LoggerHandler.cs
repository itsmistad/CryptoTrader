using CryptoTrader.Service.Services.Logging;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class LoggerHandler : ILoggerService
    {
        public LoggerHandler()
        {
            var args = Singleton.Get<ArgumentsHandler>();
            if (args.Has("logger"))
            {
                switch (args["logger"].ToLower())
                {
                    case "local":
                        Service = new FileLoggerService();
                        break;
                    default:
                        Program.Stop($"Invalid logger flag: {args["logger"]}");
                        break;
                }
            }
            else Program.Stop("No logger defined. Did you set the logger flag? ex: -logger local");
        }

        public void Log(string tag, string format, params object[] args) => Service?.Log(tag, format, args);
        public void Debug(string format, params object[] args) => Service?.Debug(format, args);
        public void Error(string format, params object[] args) => Service?.Error(format, args);
        public void Info(string format, params object[] args) => Service?.Info(format, args);
        public void Warn(string format, params object[] args) => Service?.Warn(format, args);

        #region Properties
        public ILoggerService Service { get; }
        #endregion
    }
}
