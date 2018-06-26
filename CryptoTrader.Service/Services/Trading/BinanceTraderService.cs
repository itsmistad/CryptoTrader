using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Binance;
using System.Reflection;
using CryptoTrader.Service.Services.Logging;
using CryptoTrader.Service.Services.Trading.Indicators;
using CryptoTrader.Service.Utilities.Handlers;
using CryptoTrader.Service.Utilities;

namespace CryptoTrader.Service.Services.Trading
{
    /// <summary>
    /// An implementation of ITraderService that uses the Binance API.
    /// </summary>
    public class BinanceTraderService : ITraderService
    {
        private readonly BinanceApiUser _user;

        public BinanceTraderService()
        {
            _user = new BinanceApiUser(ApiKey, ApiSecret);

            var indicators = Singleton.Get<IndicatorHandler>();
            foreach (var indicator in ActiveIndicators)
                indicators.Add(this, indicator);
        }

        public async void OnTick()
        {
            // Update Indicators
            Indicators.ForEach(x => x.OnTick());

            if (Locked) return;

            Locked = true;
            var info = await Api.GetAccountInfoAsync(_user);
            // Todo These sorts of calls should be moved to the CurrencyConverterService.
            var basePrice = await Api.GetPriceAsync(Symbol.Cache.Get(BaseCurrency + "USDT"));
            var tradePrice = await Api.GetPriceAsync(Symbol.Cache.Get(TradeCurrency + BaseCurrency));
            Locked = false;

            // Todo These sorts of calculations should be moved to the CurrencyConverterService.
            var holding = (info.GetBalance(BaseCurrency).Free + info.GetBalance(BaseCurrency).Locked) * basePrice.Value;
            var circulating = ((info.GetBalance(TradeCurrency).Free + info.GetBalance(TradeCurrency).Locked) * tradePrice.Value) * basePrice.Value;
            Log.Info("{0} is holding {1} and circulating {2}.", info.User.ApiKey.Substring(0, 8), holding.ToString("C4"), circulating.ToString(("C6")));
        }

        public void Buy()
        {
            throw new NotImplementedException(MethodBase.GetCurrentMethod().Name + " is not defined for " + this.GetType().Name + ".");
        }

        public void Sell()
        {
            throw new NotImplementedException(MethodBase.GetCurrentMethod().Name + " is not defined for " + this.GetType().Name + ".");
        }

        public void Panic()
        {
            throw new NotImplementedException(MethodBase.GetCurrentMethod().Name + " is not defined for " + this.GetType().Name + ".");
        }

        #region Properties
        private static JToken Config => ((JToken)Singleton.Get<ConfigHandler>().Service["Traders"])["Binance"];
        private static string[] ActiveIndicators => Config["ActiveIndicators"].ToObject<string[]>();
        private static double TradeDifference => Config["TradeDifference"].ToObject<float>();
        private static double TradeProfit => Config["TradeProfit"].ToObject<float>();
        private static int TradeAmount => Config["TradeAmount"].ToObject<int>();
        private static string BaseCurrency => Config["BaseCurrency"].ToString();
        private static string TradeCurrency => Config["TradeCurrency"].ToString();
        private static string ApiKey => Config["APIKey"].ToString();
        private static string ApiSecret => Config["SecretKey"].ToString();
        private static BinanceApi Api => Singleton.Get<BinanceHandler>().Api;
        private static ILoggerService Log => Singleton.Get<LoggerHandler>();

        private bool Locked { get; set; }

        public bool PanicCondition
        {
            get { return false; }
        }
        public List<IIndicator> Indicators => Singleton.Get<IndicatorHandler>().For(this);
        #endregion
    }
}
