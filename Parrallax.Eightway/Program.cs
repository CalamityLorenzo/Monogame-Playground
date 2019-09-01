using GameLibrary.Config.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parrallax.Eightway
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var configData = Configuration.Manager
                             .LoadJsonFile("opts.json")
                             .LoadJsonFile("opts2.json")
                             .Build();

            using (var game = new ParrallaxHost(configData))
                game.Run();
        }
    }
}
