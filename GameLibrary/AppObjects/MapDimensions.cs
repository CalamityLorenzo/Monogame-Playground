using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.AppObjects
{
    public class MapDimensions
    {
        int Width { get; set; }
        int Height { get; set; }
        int TileWidth { get; set; }
        int TileHeight { get; set; }
        int Cols => Width / TileWidth;
        int Rows => Height / TileHeight;
    }
}
