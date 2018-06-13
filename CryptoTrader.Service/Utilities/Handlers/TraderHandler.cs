using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CryptoTrader.Service.Services.Interfaces;
using CryptoTrader.Service.Services.Traders;
using CryptoTrader.Service.Utilities;
using CryptoTrader.Service.Utilities.Extensions;
using Newtonsoft.Json.Linq;

namespace CryptoTrader.Service.Utilities.Handlers
{
    public class TraderHandler
    {
        public event EventHandler OnTick;

        private List<ITraderService> _traders;
        private Timer _ticker;

        public TraderHandler()
        {
            _traders = new List<ITraderService>();

            CreateTraders();

            _ticker = new Timer(_tickRateInMilliseconds);
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
            T trader = new T();
            _traders.Add(trader as ITraderService);

            return trader;
        }

        #region Properties
        private ILoggerService Log => Singleton.Get<LoggerHandler>();
        private JToken Config => (JToken)Singleton.Get<ConfigHandler>().Service["Traders"];
        private double _tickRateInMilliseconds => (double)Config["TickRateInMilliseconds"];
        #endregion
    }
}