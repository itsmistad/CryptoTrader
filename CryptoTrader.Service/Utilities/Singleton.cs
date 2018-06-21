using CryptoTrader.Service.Utilities.Handlers;
using System.Collections.Generic;
using CryptoTrader.Service.Services.Conversion;

namespace CryptoTrader.Service.Utilities
{
    /// <summary>
    /// A singleton manager that initializes and contains handlers and services.
    /// </summary>
    public class Singleton
    {
        private static Dictionary<object, object> _handlers;

        public static void Initialize()
        {
            _handlers = new Dictionary<object, object>();

            #region Initialize Handlers
            Initialize<StopHandler>();
            Initialize<ArgumentsHandler>();
            Initialize<LoggerHandler>();
            Initialize<ConfigHandler>();
            Initialize<TraderHandler>();
            Initialize<BinanceHandler>();
            #endregion

            #region Initialize Services
            Initialize<CurrencyConverterService>();
            #endregion

            if (!Get<StopHandler>().IsStopping)
                IsInitialized = true;
        }

        public static T Get<T>()
        {
            if (_handlers.ContainsKey(typeof(T)))
                return (T) _handlers[typeof(T)];
            else return default(T);
        }
        
        private static void Initialize<T>() where T : new()
        {
            if (Get<StopHandler>()?.IsStopping ?? false) // Prevents initialization after handler failure.
                return;
            if (!_handlers.ContainsKey(typeof(T))) // Prevents duplicate entries.
                _handlers.Add(typeof(T), new T());
        }

        #region Properties
        public static bool IsInitialized;
        #endregion
    }
}
