using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutsideTheframe.Models
{
    public class PlayerControls
    {
        public string Up { get; set; }
        public string Down { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
        public string Fire { get; set; }
        public string Action { get; set; }
        public string Special { get; set; }

        private static Lazy<PlayerControls> emptyVal = new Lazy<PlayerControls>(() => new PlayerControls { Up = "", Down = "", Left = "", Right = "", Fire = "", Action = "", Special = "" });
        public static PlayerControls Empty => emptyVal.Value;

    }


}
