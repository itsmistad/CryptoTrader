using System;
using System.IO;
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
        public FileLoggerService() { }

        public FileLoggerService(string filePath)
        {
            var logsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            FilePath = Path.Combine(logsPath, "CryptoTrader.Service." + DateTime.UtcNow.ToString("yyyy-MM-ddTHH.mm.ssZ") + ".log");
            Directory.CreateDirectory(logsPath);
        }

        public void Log(string tag, string format, params object[] args)
        {
            try
            {
                var formattedMessage = string.Format("[{0}\t{1}]\t{2}", TimeStamp, tag, args != null ? string.Format(format, args) : format);
                File.AppendAllText(FilePath, formattedMessage + Environment.NewLine);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Info(string format, params object[] args) => Log("INFO", format, args);
        public void Warn(string format, params object[] args) => Log("WARN", format, args);
        public void Error(string format, params object[] args) => Log("ERROR", format, args);
        public void Debug(string format, params object[] args) => Log("DEBUG", format, args);

        #region Properties
        public string DirectoryPath => Path.GetDirectoryName(FilePath);
        public string FileName => Path.GetFileName(FilePath);
        public string FilePath { get; }
        public static string TimeStamp => DateTime.UtcNow.ToString("s");
        #endregion
    }
}
