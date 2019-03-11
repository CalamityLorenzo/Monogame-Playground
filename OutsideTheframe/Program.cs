using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OutsideTheframe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutsideTheframe
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            var jsonOptions = "";
            Dictionary<string, string> player1Controls;
            Dictionary<string, string> player2Controls;
            GameOptions Options;
            using (var reader = new StreamReader("opts.json"))
            {
                jsonOptions = reader.ReadToEnd();
                var allOpts = JObject.Parse(jsonOptions);
                var gameOptions = allOpts["Options"].Children().ToList();

                if (allOpts.ContainsKey("Player1Controls"))
                {
                    var playerTokens = allOpts["Player1Controls"];
                    var playerDick = JsonConvert.DeserializeObject<Dictionary<string, string>>(playerTokens.ToString());
                    player1Controls = new Dictionary<string, string>(playerDick);
                }
                else
                {
                    player1Controls = new Dictionary<string, string>();
                }

                if (allOpts.ContainsKey("Player2Controls")) { 
                    List<JToken> player1Tokens = allOpts["Player2Controls"].Children().ToList();
                }
                else
                {
                    player2Controls = new Dictionary<string, string>();
                }
                
            }

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
