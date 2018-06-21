using System;
using System.IO;
using CryptoTrader.Service.Services.Logging;
using CryptoTrader.Service.Utilities;
using CryptoTrader.Service.Utilities.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoTrader.Service.Services.Configuration
{
    /// <summary>
    /// An implementation of IConfigService that load a JSON file into a dictionary.
    /// </summary>
    public class JsonConfigService : IConfigService
    {
        private JObject _configObject;

        public JsonConfigService(string filePath)
        {
            FilePath = filePath;
        }

        public bool TryLoad()
        {
            try
            {
                return (_configObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(FilePath))) != null;
            }
            catch (Exception)
            {
                Log.Error("Uh oh. We couldn't load the config file [{0}]. Does it exist?", FileName);
                return false;
            }
        }

        public bool TrySave()
        {
            try
            {
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(_configObject, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Log.Error("Uh oh. We couldn't save the config file [{0}]. Is access blocked? [{1}]", FileName, ex.Message);
                return false;
            }

            return true;
        }

        #region Properties
        private static ILoggerService Log => Singleton.Get<LoggerHandler>();

        public object this[string key]
        {
            get => (_configObject.TryGetValue(key, out JToken value) ? value : null);
            set => _configObject[key] = (JToken)value;
        }

        public string FilePath { get; }

        public string FileName => Path.GetFileName(FilePath);
        #endregion
    }
}
