using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Service.Services.Interfaces
{
    /// <summary>
    /// A configuration management service interface that loads, saves, and configures keys in a Dictionary fashion.
    /// </summary>
    public interface IConfigService
    {
        bool TryLoad();
        bool TrySave();
        object this[string key] { get; }
    }
}
