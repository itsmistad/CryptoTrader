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
            Log.Info("Grabbing account info...");
            var info = await Api.GetAccountInfoAsync(_user);

            Log.Info(" - User: {0}", info.User.ApiKey);
            Log.Info(" - Able to trade: {0}", info.Status.CanTrade);
            Log.Info(" - Able to withdraw: {0}", info.Status.CanWithdraw);
            Log.Info(" - Able to deposit: {0}", info.Status.CanDeposit);
            Log.Info(" - Free {0}: {1}", BaseCurrency, info.GetBalance(BaseCurrency).Free);
            Log.Info(" - Locked {0}: {1}", BaseCurrency, info.GetBalance(BaseCurrency).Locked);
            Log.Info(" - Free {0}: {1}", TradeCurrency, info.GetBalance(TradeCurrency).Free);
            Log.Info(" - Locked {0}: {1}", TradeCurrency, info.GetBalance(TradeCurrency).Locked);
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

        public bool PanicCondition
        {
            get
            {
                // TODO     Define the condition that would cause the binance trader service to panic-sell all units of the trading currency currently being held.
                return false;
            }
        }
        public List<IIndicator> Indicators => Singleton.Get<IndicatorHandler>().For(this);
        #endregion
    }
}
