using System;

namespace CryptoTrader.Service.Services.Logging
{
    public class ConsoleLoggerService : ILoggerService
    {
        public void Log(string tag, string format, params object[] args)
        {
            Console.WriteLine("[{0}\t{1}]\t{2}", TimeStamp, tag, args != null ? string.Format(format, args) : format);
        }

        public void Info(string format, params object[] args)
        {
            // TODO     We need color!
            Log("INFO", format, args);
        }

        public void Warn(string format, params object[] args)
        {
            // TODO     We need color!
            Log("WARN", format, args);
        }

        public void Error(string format, params object[] args)
        {
            // TODO     We need color!
            Log("ERROR", format, args);
        }

        public void Debug(string format, params object[] args)
        {
            // TODO     We need color!
            Log("DEBUG", format, args);
        }

        #region Properties
        public static string TimeStamp => DateTime.UtcNow.ToString("s");
        #endregion
    }
}
