using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrader.Service.Services.Interfaces
{
    /// <summary>
    /// A cryptocurrency market trader service interface.
    /// </summary>
    public interface ITraderService
    {
        void OnTick();
        void Buy();
        void Sell();
        void Panic();
        bool PanicCondition { get; }
    }
}
