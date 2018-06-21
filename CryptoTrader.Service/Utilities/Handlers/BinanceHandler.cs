using Binance;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class BinanceHandler
    {
        public BinanceHandler()
        {
            Api = new BinanceApi();
        }

        public BinanceApi Api { get; }
    }
}
