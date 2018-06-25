using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using CryptoTrader.Service.Utilities;
using CryptoTrader.Service.Utilities.Handlers;

namespace CryptoTrader.Service.Services.Logging
{
    /// <inheritdoc />
    /// <summary>
    /// An implementation of ILoggerService that logs various messages to a formatted .log file.
    /// </summary>
    public class FileLoggerService : ILoggerService
    {
        public FileLoggerService()
        {
            _queue = new List<string>();
            _ticker = new Timer(1000);
            _ticker.Elapsed += (sender, args) => LogQueue();
            _ticker.Enabled = true;
            _ticker.Start();

            var logsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            FilePath = Path.Combine(logsPath, "CryptoTrader.Service." + DateTime.UtcNow.ToString("yyyy-MM-ddTHH.mm.ssZ") + ".log");
            Directory.CreateDirectory(logsPath);
        }

        public void LogQueue()
        {
            string[] queue = new string[_queue.Count];
            _queue.CopyTo(queue);
            _queue.Clear();
            
            try
            {
                File.AppendAllLines(FilePath, queue);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Stop()
        {
            _ticker.Stop();
            LogQueue();
        }

        public void Log(string tag, string format, params object[] args)
        {
            var formattedMessage = string.Format("{0}\t{1}\t{2}", TimeStamp, tag, args != null ? string.Format(format, args) : format);
            _queue.Add(formattedMessage);
        }

        public void Info(string format, params object[] args) => Log("INFO", format, args);
        public void Warn(string format, params object[] args) => Log("WARN", format, args);
        public void Error(string format, params object[] args) => Log("ERROR", format, args);
        public void Debug(string format, params object[] args) => Log("DEBUG", format, args);

        #region Properties
        public string DirectoryPath => Path.GetDirectoryName(FilePath);
        public string FileName => Path.GetFileName(FilePath);
        public string FilePath { get; }
        private readonly Timer _ticker;
        private readonly List<string> _queue;
        private static string TimeStamp => DateTime.UtcNow.ToString("s");
        #endregion
    }
}
