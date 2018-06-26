using System;
using System.Drawing;
using Console = Colorful.Console;

namespace CryptoTrader.Service.Services.Logging
{
    public class ConsoleLoggerService : ILoggerService
    {
        public void Stop() { }

        public void Log(string tag, string format, params object[] args)
        {
            Console.WriteWithGradient($"{TimeStamp}\t", Color.PeachPuff, Color.MediumSlateBlue, 8);
            
            var tagColor = Color.WhiteSmoke;
            switch (tag)
            {
                case "INFO":
                    tagColor = Color.DarkGray;
                    break;
                case "WARN":
                    tagColor = Color.Gold;
                    break;
                case "ERROR":
                    tagColor = Color.Crimson;
                    break;
                case "DEBUG":
                    tagColor = Color.MediumTurquoise;
                    break;
            }

            Console.Write($"{tag}\t", tagColor);
            Console.WriteLine(args != null ? string.Format(format, args) : format, Color.WhiteSmoke);
        }

        public void Info(string format, params object[] args)
        {
            Log("INFO", format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Log("WARN", format, args);
        }

        public void Error(string format, params object[] args)
        {
            Log("ERROR", format, args);
        }

        public void Debug(string format, params object[] args)
        {
            Log("DEBUG", format, args);
        }

        #region Properties
        public static string TimeStamp => DateTime.UtcNow.ToString("s");
        #endregion
    }
}
