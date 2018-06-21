using System.Collections.Generic;

namespace CryptoTrader.Service.Utilities.Handlers
{
    /// <summary>
    /// A handler for all startup arguments and any optional values associated with each.
    /// </summary>
    public class ArgumentsHandler
    {
        private readonly Dictionary<string, string> _arguments;

        public ArgumentsHandler()
        {
            _arguments = new Dictionary<string, string>();
            var currentKey = "";
            var expectingValue = false;
            foreach (var arg in Program.Args)
            {
                if (arg.StartsWith("-"))
                {
                    currentKey = arg.Substring(1);
                    _arguments.Add(currentKey, "");
                    expectingValue = true;
                }
                else if (expectingValue)
                {
                    _arguments[currentKey] = arg;
                    expectingValue = false;
                }
            }
        }

        public bool Has(string key) => _arguments.ContainsKey(key);

        #region Properties
        public string this[string key] => (Has(key) ? _arguments[key] : null);
        #endregion
    }
}
