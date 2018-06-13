using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTrader.Service.Services.Interfaces;

namespace CryptoTrader.Service.Services
{
    /// <summary>
    /// An implementation of ILoggerService that logs various messages to a formatted .log file.
    /// </summary>
    public class FileLoggerService : ILoggerService
    {
        private string _filePath;

        public FileLoggerService()
        {
            var logsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            _filePath = Path.Combine(logsPath, "CryptoTrader.Service." + DateTime.UtcNow.ToString("yyyy-MM-ddTHH.mm.ssZ") + ".log");
            Directory.CreateDirectory(logsPath);
        }

        public void Log(string tag, string format, params object[] args)
        {
            try
            {
                var formattedMessage = string.Format("[{0}]\t[{1}]\t{2}", TimeStamp, tag, args != null ? string.Format(format, args) : format);
                Console.WriteLine(formattedMessage);
                File.AppendAllText(FilePath, formattedMessage + Environment.NewLine);
            }
            catch (Exception)
            {
                Console.WriteLine("[{0}]\t[{1}]\t{2}", TimeStamp, "ERROR", "Uh oh. That last message didn't log to the file. Is the stream blocked?");
            }
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
        public string DirectoryPath
        {
            get
            {
                return Path.GetDirectoryName(_filePath);
            }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(_filePath);
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string TimeStamp
        {
            get
            {
                return DateTime.UtcNow.ToString("o");
            }
        }
        #endregion
    }
}
