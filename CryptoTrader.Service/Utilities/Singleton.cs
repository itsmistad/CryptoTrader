using CryptoTrader.Service.Utilities.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
            try
            {
                Initialize<StopHandler>();
                Initialize<ArgumentsHandler>();
                Initialize<LoggerHandler>();
                Initialize<ConfigHandler>();
                Initialize<TraderHandler>();

                IsInitialized = true;
            }
            catch (Exception) { }
        }

        public static T Get<T>()
        {
            if (!_handlers.ContainsKey(typeof(T)))
                throw new KeyNotFoundException("Trying to reach a handler before it has been initialized: " + typeof(T).Name);
            
            return (T)_handlers[typeof(T)];
        }
        
        private static void Initialize<T>() where T : new()
        {
            // If something failed during any initialization, stop.
            if (typeof(T) != typeof(StopHandler) && Get<StopHandler>().IsStopping)
                throw new Exception("Initialization failed.");
            _handlers.Add(typeof(T), new T());
        }

        #region Properties
        public static bool IsInitialized;
        #endregion
    }
}
