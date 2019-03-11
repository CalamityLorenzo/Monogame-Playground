using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutsideTheframe
{
    public class GameOptions
    {
        int ScreenWidth { get; set; }
        int ScreenHeight { get; set; }
        private static Lazy<GameOptions> gameOptionsEmpty = new Lazy<GameOptions>(() => new GameOptions { ScreenHeight = -1, ScreenWidth = -1 });
        public static GameOptions Empty => gameOptionsEmpty.Value;
    }
}
