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
    public class Configuration
    {
        private static Lazy<Configuration> configInstance = new Lazy<Configuration>();
        private Configuration() { }
        private bool BuildComplete = false;
        private static Configuration Manager => configInstance.Value;

        //public Dictionary<string, string> PlayerOneControls { get; private set; }
        //public Dictionary<string, string> PlayerTwoControls { get; private set; }
        //public GameOptions GameOptions { get; private set; }
         
        private Dictionary<string, string> LoadPlayerControls(string userControlSection, JObject jsonObject)
        {
            if (jsonObject.ContainsKey(userControlSection))
            {
                var controlTokens = jsonObject[userControlSection];
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(controlTokens.ToString());
            }
            else
            {
                return new Dictionary<string, string>();
            }

        }
 
        private List<string> fileNames = new List<string>();
        private List<JObject> LoadedData = new List<JObject>();

        public Configuration LoadJsonFile(string jsonfilePath)
        {
            fileNames.Add(jsonfilePath);
            return this;
        }

        public void Build()
        {
            foreach (var fileName in fileNames)
            {
                using (var reader = new StreamReader(fileName))
                {
                    var jsonOptions = reader.ReadToEnd();
                    LoadedData.Add(JObject.Parse(jsonOptions));  
                    //var GameOptions = LoadGameOptions("Options", allOpts);
                    //var playerOneControls = LoadPlayerControls("Player1Controls", allOpts);
                    //var playerTwoControls = LoadPlayerControls("Player2Controls", allOpts);
                    //this.PlayerOneControls = PlayerOneControls;
                    //this.PlayerTwoControls = PlayerTwoControls;
                    //this.GameOptions = GameOptions;
                }
            }
            BuildComplete = true;

        }

    }
}
