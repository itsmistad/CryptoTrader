using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoTrader.Service.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Binance;
using System.Reflection;
using CryptoTrader.Service.Utilities.Handlers;
using CryptoTrader.Service.Utilities;
using CryptoTrader.Service.Utilities.Extensions;

namespace CryptoTrader.Service.Services.Traders
{
    /// <summary>
    /// An implementation of ITraderService that uses the Mad Hatter technique to rake in gains.
    /// </summary>
    public class BinanceTraderService : ITraderService
    {
        private BinanceApi _api;
        private BinanceApiUser _user;

        public BinanceTraderService()
        {
            _api = new BinanceApi();
            _user = new BinanceApiUser(_apiKey, _apiSecret);
        }

        public async void OnTick()
        {
            Log.Info("Grabbing account info...");
            var info = await _api.GetAccountInfoAsync(_user);

            Log.Info(" - User: {0}", info.User.ApiKey);
            Log.Info(" - Able to trade: {0}", info.Status.CanTrade);
            Log.Info(" - Able to withdraw: {0}", info.Status.CanWithdraw);
            Log.Info(" - Able to deposit: {0}", info.Status.CanDeposit);
            Log.Info(" - Free {0}: {1}", _baseCurrency, info.GetBalance(_baseCurrency).Free);
            Log.Info(" - Locked {0}: {1}", _baseCurrency, info.GetBalance(_baseCurrency).Locked);
            Log.Info(" - Free {0}: {1}", _tradeCurrency, info.GetBalance(_tradeCurrency).Free);
            Log.Info(" - Locked {0}: {1}", _tradeCurrency, info.GetBalance(_tradeCurrency).Locked);
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
        private JToken Config => ((JToken)Singleton.Get<ConfigHandler>().Service["Traders"])["Binance"];
        private double _tradeDifference => Config["TradeDifference"].ToObject<float>();
        private double _tradeProfit => Config["TradeProfit"].ToObject<float>();
        private int _tradeAmount => Config["TradeAmount"].ToObject<int>();
        private string _baseCurrency => Config["BaseCurrency"].ToString();
        private string _tradeCurrency => Config["TradeCurrency"].ToString();
        private string _apiKey => Config["APIKey"].ToString();
        private string _apiSecret => Config["SecretKey"].ToString();

        private ILoggerService Log => Singleton.Get<LoggerHandler>();

        public bool PanicCondition
        {
            get
            {
                //TODO  Define the condition that would cause the binance trader service to panic-sell all units of the trading currency currently being held.
                return false;
            }
        }
        #endregion
    }
}
