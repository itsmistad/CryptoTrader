using System;
using CryptoTrader.Service.Utilities;
using CryptoTrader.Service.Utilities.Handlers;

namespace CryptoTrader.Service
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Title = "Initializing...";
            Args = args;

            Singleton.Initialize();
            Singleton.Get<StopHandler>().WaitForStop();
        }

        #region Properties
        public static string[] Args;
        public static void Stop(string reason, params object[] args) => Singleton.Get<StopHandler>().Stop(reason, args);
        public static string Title
        {
            get => Console.Title;
            set => Console.Title = "CryptoTrader by D. | " +  value;
        }
        #endregion
    }
}
