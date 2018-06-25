using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTrader.Service.Services.Trading;
using CryptoTrader.Service.Services.Trading.Indicators;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class IndicatorHandler
    {
        public IndicatorHandler()
        {
            _traders = new Dictionary<ITraderService, List<IIndicator>>();
        }

        public void Add(ITraderService trader, string indicator)
        {
            IIndicator instance = null;
            switch (indicator)
            {
                case "EMA":
                    instance = new EMAIndicator();
                    break;
                case "MACD":
                    instance = new MACDIndicator();
                    break;
            }

            if (!_traders.ContainsKey(trader))
                _traders.Add(trader, new List<IIndicator>() { instance });
            else
                _traders[trader].Add(instance);
        }

        public List<IIndicator> For(ITraderService trader)
        {
            if (!_traders.ContainsKey(trader))
                _traders.Add(trader, new List<IIndicator>());
            return _traders[trader];
        }

        #region Properties
        private readonly Dictionary<ITraderService, List<IIndicator>> _traders;
        #endregion
    }
}
