using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceTrader.Services.Interfaces
{
    public interface IConfigService<T>
    {
        bool TryLoad();
        bool TrySave();
        T this[string key] { get; }
    }
}
