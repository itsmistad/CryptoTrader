using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Service.Services.Interfaces
{
    /// <summary>
    /// A log service interface with support for Info, Warn, Error, and Debug messages, as well as a generic Log function for custom tags.
    /// </summary>
    public interface ILoggerService
    {
        void Log(string tag, string format, params object[] args);
        void Info(string format, params object[] args);
        void Warn(string format, params object[] args);
        void Error(string format, params object[] args);
        void Debug(string format, params object[] args);
    }
}
