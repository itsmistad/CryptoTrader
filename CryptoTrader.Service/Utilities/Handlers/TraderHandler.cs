using System;
using System.Collections.Generic;
using System.Timers;
using CryptoTrader.Service.Services.Logging;
using CryptoTrader.Service.Services.Trading;
using Newtonsoft.Json.Linq;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class TraderHandler
    {
        public event EventHandler OnTick;

        private readonly List<ITraderService> _traders;
        private readonly Timer _ticker;

        public TraderHandler()
        {
            _traders = new List<ITraderService>();

            CreateTraders();

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
            Create<BinanceTraderService>();

            foreach (var trader in _traders)
                OnTick += (sender, args) => trader.OnTick();
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


        public T Create<T>() where T : new()
        {
            var trader = new T();
            _traders.Add(trader as ITraderService);

            return trader;
        }

        #region Properties
        private static ILoggerService Log => Singleton.Get<LoggerHandler>();
        private static JToken Config => (JToken)Singleton.Get<ConfigHandler>().Service["Traders"];
        private double TickRateInMilliseconds => (double)Config["TickRateInMilliseconds"];
        #endregion
    }
}