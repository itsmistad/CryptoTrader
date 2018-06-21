using Binance;
using CryptoTrader.Service.Utilities;
using CryptoTrader.Service.Utilities.Handlers;

namespace CryptoTrader.Service.Services.Conversion
{
    public class CurrencyConverterService
    {
        #region Properties
        private static BinanceApi Api => Singleton.Get<BinanceHandler>().Api;
        #endregion
    }
}
