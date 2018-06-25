using System;
using System.Collections.Generic;
using System.Timers;
using CryptoTrader.Service.Services.Logging;
using CryptoTrader.Service.Services.Trading;
using CryptoTrader.Service.Services.Trading.Indicators;
using Newtonsoft.Json.Linq;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class TraderHandler
    {
        public TraderHandler()
        {
            _traders = new List<ITraderService>();

            CreateTraders();
            HookIndicators();

            _ticker = new Timer(TickRateInMilliseconds);
            _ticker.Elapsed += HandleTick;
            _ticker.Enabled = true;
            _ticker.Start();
        }

        public void Stop()
        {
            _ticker.Stop();
        }

        private void CreateTraders()
        {
            // Here, we can create multiple traders for different currencies.
            // Maybe pass in different parameters upon creation?
            // Where would these parameters be stored? And how?
            Create<BinanceTraderService>();

            foreach (var trader in _traders)
                OnTick += (sender, args) => trader.OnTick();
        }

        private void HookIndicators()
        {
            foreach (var trader in _traders)
                foreach (var indicator in trader.Indicators)
                    OnTick += (sender, args) => indicator.OnTick();
        }

        public void HandleTick(object sender, ElapsedEventArgs args)
        {
            foreach (var trader in _traders)
                try
                {
                    if (trader.PanicCondition)
                        trader.Panic();
                }
                catch (Exception ex)
                {
                    Log.Warn(ex.Message);
                }
            try
            {
                OnTick?.Invoke(sender, args);
            }
            catch (Exception ex)
            {
                Log.Warn(ex.Message);
            }
        }


        public T Create<T>() where T : ITraderService, new()
        {
            var trader = new T();
            _traders.Add(trader);

            return trader;
        }

        #region Properties
        public event EventHandler OnTick;
        private readonly Timer _ticker;
        private readonly List<ITraderService> _traders;
        private static ILoggerService Log => Singleton.Get<LoggerHandler>();
        private static JToken Config => (JToken)Singleton.Get<ConfigHandler>().Service["Traders"];
        private static double TickRateInMilliseconds => (double)Config["TickRateInMilliseconds"];
        #endregion
    }
}