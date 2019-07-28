using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public struct DisplayRectInfo
    {

        public DisplayRectInfo(Texture2D texture2D, Rectangle destination, Rectangle source) : this()
        {
            this.Texture = texture2D;
            this.SourceArea = source;
            this.DestinationArea = destination;
        }

        public Texture2D Texture { get; private set; }
        public Rectangle SourceArea { get; private set; }
        public Rectangle DestinationArea { get; private set; }

    }
}
