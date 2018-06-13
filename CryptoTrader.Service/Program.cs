using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoTrader.Service.Services;
using CryptoTrader.Service.Services.Interfaces;
using CryptoTrader.Service.Services.Traders;
using CryptoTrader.Service.Utilities;
using CryptoTrader.Service.Utilities.Handlers;
using Newtonsoft.Json.Linq;

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
            get
            {
                return Console.Title;
            }
            set
            {
                Console.Title = "CryptoTrader by D. | " +  value;
            }
        }
        #endregion
    }
}
