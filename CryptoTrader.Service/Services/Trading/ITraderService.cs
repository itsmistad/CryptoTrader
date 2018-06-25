using System.Collections.Generic;
using CryptoTrader.Service.Services.Trading.Indicators;

namespace CryptoTrader.Service.Services.Trading
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
        List<IIndicator> Indicators { get; }
    }
}
