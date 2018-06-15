using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTrader.Service.Services.Interfaces;
using CryptoTrader.Service.Utilities;
using CryptoTrader.Service.Utilities.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoTrader.Service.Services
{
    /// <summary>
    /// An implementation of IConfigService that load a JSON file into a dictionary.
    /// </summary>
    public class JSONConfigService : IConfigService
    {
        private JObject _configObject;
        private string _filePath;

        public JSONConfigService(string filePath)
        {
            _filePath = filePath;
        }

        public bool TryLoad()
        {
            try
            {
                return (_configObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(_filePath))) != null;
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
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(_configObject, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Log.Error("Uh oh. We couldn't save the config file [{0}]. Is access blocked? [{1}]", FileName, ex.Message);
                return false;
            }

            return true;
        }

        #region Properties
        private ILoggerService Log => Singleton.Get<LoggerHandler>();

        public object this[string key]
        {
            get
            {
                return (_configObject.TryGetValue(key, out JToken value) ? value : null);
            }
            set
            {
                _configObject[key] = (JToken)value;
            }
        }

        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(_filePath);
            }
        }
        #endregion
    }
}
