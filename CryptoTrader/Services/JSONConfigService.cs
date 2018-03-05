using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceTrader.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BinanceTrader.Services
{
    public class JSONConfigService : IConfigService<JToken>
    {
        private JObject _configObject;
        private string _filePath;

        public JSONConfigService(string filePath)
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        }

        public bool TryLoad()
        {
            try
            {
                return (_configObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(_filePath))) != null;
            }
            catch (Exception ex)
            {
                Program.Logger.Error("Uh oh. We couldn't load the config file [{0}]. Does it exist? [{1}]", FileName, ex.Message);
                return false;
            }
        }

        public bool TrySave()
        {
            try
            {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(_configObject));
            }
            catch (Exception ex)
            {
                Program.Logger.Error("Uh oh. We couldn't save the config file [{0}]. Is access blocked? [{1}]", FileName, ex.Message);
                return false;
            }

            return true;
        }

        #region Properties
        public JToken this[string key]
        {
            get
            {
                return (_configObject.TryGetValue(key, out JToken value) ? value : null);
            }
            set
            {
                _configObject[key] = value;
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
