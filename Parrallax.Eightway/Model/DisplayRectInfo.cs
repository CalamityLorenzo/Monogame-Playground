using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parrallax.Eightway.Model
{
    struct DisplayRectInfo
    {
        public Texture2D Texture { get; private set; }
        public Rectangle DisplayArea { get; private set; }

    }
}
