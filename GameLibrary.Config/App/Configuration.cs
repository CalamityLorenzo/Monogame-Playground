using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Config.App
{
    public class ConfigurationData
    {
        private List<JObject> ConfigData;
        internal ConfigurationData(List<JObject> configData)
        {
            ConfigData = new List<JObject>(configData);
        }

        public T ToResultType<T>(string propertyName, Func<string, T> mapFunc) where T : class
        {
            var matchedObject = this.ConfigData.First(jobj => jobj.ContainsKey(propertyName));
            return mapFunc(matchedObject[propertyName].ToString());
        }

        // Simple Case leaning on Json.net
        public T ToResultType<T>(string propertyName) where T : class
        {
            return this.ToResultType<T>(propertyName, (str) => JsonConvert.DeserializeObject<T>(str));
        }
    }
    public class Configuration
    {
        private static Lazy<Configuration> configInstance = new Lazy<Configuration>(() => new Configuration());
        private Configuration() { }
        private bool BuildComplete = false;
        public static Configuration Manager => configInstance.Value;

        private HashSet<string> fileNames = new HashSet<string>();
        private List<JObject> LoadedData = new List<JObject>();

        public Configuration LoadJsonFile(string jsonfilePath)
        {
            fileNames.Add(jsonfilePath);
            return this;
        }


        public ConfigurationData Build()
        {
            if (!BuildComplete)
            {
                LoadedData.AddRange(fileNames.Select(fn => JObject.Parse(File.ReadAllText(fn))));
                BuildComplete = true;
                return new ConfigurationData(LoadedData);
            }
            throw new ArgumentOutOfRangeException("Build has already been completed.");
        }

    }
}
